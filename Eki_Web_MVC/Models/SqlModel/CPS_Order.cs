using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;
using OCPP_1_6;

namespace Eki_Web_MVC.CPS
{
    [DbTableSet("CPS_Order")]
    public class CPS_Order : BaseDbDAO, EkiSerial.Model, IConvertResponse<CPS_Order.Response>, IPayCost.CurrencySet
    {
        [DbRowKey("ActionId",DbAction.Update)]
        public int ActionId { get; set; } = -1;//對應的是CpActionValue
        [DbRowKey("MemberId",DbAction.Update)]
        public int MemberId { get; set; } = -1;
        [EkiSerial.Property]
        [DbRowKey("Serial")]
        public string Serial { get; set; }//訂單自身的序號
        [DbRowKey("CpSerial")]
        public string CpSerial { get; set; }//對應的充電樁
        [DbRowKey("Status", DbAction.Update)]
        public int Status { get => status.toInt(); set => status = value.toEnum<OrderStatus>(); }

        /// <summary>
        /// 計算費率 (元/度)
        /// </summary>
        [DbRowKey("Rate", DbAction.Update)]
        public double Rate { get; set; } = -1;

        /// <summary>
        /// 使用定價計費(元/小時) -1=使用費率計算
        /// </summary>
        [DbRowKey("CpPrice",DbAction.Update)]
        public double CpPrice { get; set; } = -1;

        /// <summary>
        /// 充電設定時長(min) CpPrice !=-1 時使用
        /// </summary>
        [DbRowKey("Duration", DbAction.Update)]
        public int Duration { get; set; } = -1;
        [DbRowKey("Cost",DbAction.Update)]
        public double Cost { get; set; }
        [DbRowKey("Unit",DbAction.Update)]
        public int Unit { get => unit.toInt(); set => unit = value.toEnum<CurrencyUnit>(); }
        [DbRowKey("cDate", RowAttribute.CreatTime, true)]
        public DateTime cDate { get; set; }
        [DbRowKey("Uid", RowAttribute.Guid, true)]
        public Guid Uid { get; set; }
        [DbRowKey("beEnable", DbAction.Update)]
        public bool beEnable { get; set; } = true;

        public OrderStatus status = OrderStatus.CheckOut;
        public CurrencyUnit unit = CurrencyUnit.TWD;

        public CPS_Member member
        {
            get
            {
                if (_member == null)
                {
                    _member = EkiSQL.ekicps.data<CPS_Member>(
                        "where Id=@mid",
                        new { mid = MemberId });
                }
                return _member;
            }
        }

        public CPS_Invoice invoice
        {
            get
            {
                if (_invo==null)
                {
                    _invo = EkiSQL.ekicps.data<CPS_Invoice>(
                        "where OrderId=@oid",
                        new { oid = Id });
                }
                return _invo;
            }
        }

        public CPS_Checkout checkout
        {
            get
            {
                if (_checkout == null)
                    _checkout = EkiSQL.ekicps.data<CPS_Checkout>(
                        "where OrderId=@oid",
                        new { oid = Id });

                return _checkout;
            }
        }
        public CPS_CpActionValue action
        {
            get
            {
                if (_action == null)
                    _action = EkiSQL.ekicps.data<CPS_CpActionValue>(
                        "where Id=@aid", new { aid = ActionId }
                        );

                return _action;
            }
        }

        private CPS_Checkout _checkout;
        private CPS_CpActionValue _action;
        private CPS_Invoice _invo;
        private CPS_Member _member;

        public EkiSerial.Builder applySerial() => EkiSerial.order;

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);

        public override int Insert(bool isReturnId = false)
        {
            this.setSerial();
            return EkiSQL.ekicps.insert(this, isReturnId);
        }
        public override bool Update()
        {
            return EkiSQL.ekicps.update(this);
        }

        public static CPS_Order creatBySerial(string serial)
            => EkiSQL.ekicps.data<CPS_Order>(
                "where beEnable=1 and Serial=@ser",
                new { ser = serial }
                );

        public Response convertToResponse()
        {
            var loc = CPS_Location.findByCpSerial(CpSerial);

            return new Response
            {
                Serial = Serial,
                Cost = this.payCost(),
                Status = Status,
                Rate = Rate,
                Unit = Unit,
                cDate = cDate.toString(),
                Member = action.MemberId < 1 ? null : new CPS_Member().Let(m =>
                {
                    m.CreatById(action.MemberId);                    
                    return new Response.OrderMember
                    {
                        Phone=m.Phone,
                        Name=m.info.Name
                    };
                }),
                Loc = loc == null ? null : new Response.Location
                {
                    Serial = loc.SerNum,
                    Content = loc.info.InfoContent
                },
                Time = action == null ? null : new Response.TimeRange
                {
                    Start = action.StartTime.toString(),
                    End = action.EndTime.toString()
                },
                Value = action == null ? null : new Response.CpValue
                {
                    Start = action.startValue.Wh,
                    End = action.endValue.Wh,
                    Unit = SampleValue.Unit.Wh.ToString()
                }
            };
        }
            //=> new
            //{
            //    Serial,
            //    Status,
            //    Cost,
            //    Rate,
            //    CheckoutUrl=checkout?.Url
            //};

        public CurrencyUnit costUnit() => unit;

        public double rawCost() => Cost;

        public string payDesc()
        {
            return $"充電帳單{Serial} \r\n結帳時間：{checkout.Date.toString()}";
        }

        public static class Calculator
        {
            //區塊電私有充電樁計算
            public static Result calOrder(CPS_Order order,CPS_CpActionValue action)
            {
                var startWh = action.startValue.Wh;
                var endWh = action.endValue.Wh;
                var rate = order.Rate;


                return new Result
                {
                    Cost = order.currency(calCost(endWh - startWh, rate))
                };
            }
            
            public static double calCost(double wh,double rate)
            {
                //轉成千瓦/小時
                return (wh / 1000d) * rate;
            }

            public class Result
            {
                public double Cost { get; set; }
            }

        }
    
        
        public class Response
        {
            public string Serial { get; set; }
            public int Status { get; set; }
            public double Cost { get; set; }
            public double Rate { get; set; }
            public int Unit { get; set; }

            public OrderMember Member { get; set; }
            public string Card4No { get; set; }
            public string cDate { get; set; }

            public Location Loc { get; set; }
            public TimeRange Time { get; set; }
            public CpValue Value { get; set; }

            public class OrderMember
            {
                public string Phone { get; set; }
                public string Name { get; set; }
            }
            public class TimeRange
            {
                public string Start { get; set; }
                public string End { get; set; }
            }
            public class Location
            {
                public string Serial { get; set; }
                public string Content { get; set; }
            }
            public class CpValue
            {
                public double Start { get; set; }
                public double End { get; set; }
                public string Unit { get; set; }
            }
        }
    
    }
}