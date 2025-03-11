using Eki_Web_MVC.CPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;
using Eki_LinePayApi_v3;
using System.Threading.Tasks;

namespace Eki_Web_MVC
{
    public class OCSModel : IBaseManagerModel
    {
        public bool addMember(RegisterReq req)
        {
            if (EkiSQL.ekicps.hasData<CPS_Member>("where Phone=@p", new { p = req.phone.cleanXss() }))
                throw new AccountException();

            var newMember = new CPS_Member();
            newMember.Phone = req.phone;
            newMember.Mail = req.mail;
            newMember.Ip = WebUtil.clientIP();
            newMember.beEnable = true;
            //以後有密碼在搞吧
            newMember.info.Phone = req.phone;
            newMember.info.CountryCode = "886";

            return newMember.Insert(true) > 0;
        }

        /// <summary>
        /// 區塊電公有充電樁結帳
        /// </summary>
        /// <param name="member"></param>
        /// <param name="cp"></param>
        /// <param name="chargeMin"></param>
        /// <param name="payMethod"></param>
        /// <param name="invo"></param>
        /// <returns>CPS_Checkout</returns>
        public async Task<CPS_Checkout> checkoutOrder(CPS_Member member,
            CPS_ChargePoint cp, int chargeMin,
            PayMethod payMethod, CPS_Invoice invo)
        {
            var loc = EkiSQL.ekicps.data<CPS_Location>(
                "where CpSerial=@cs",
                new { cs = cp.Serial });
            if (loc == null)
                throw new ArgumentNullException();

            var oid = new CPS_Order()
            {
                MemberId = member.Id,
                CpSerial = cp.Serial,
                CpPrice = loc.config.Price,
                Duration = chargeMin,
                Cost = OrderCalculator.calOrderCost(chargeMin, loc.config)
            }.Insert(true);            

            var order = new CPS_Order().Also(o => o.CreatById(oid));

            //插入發票序號
            invo.OrderId = oid;
            invo.SerNO = order.Serial;
            invo.Insert(true);

            Log.d($"ocs chargeout order->{order.toJsonString()}");
            Log.d($"ocs chargeout invoice->{invo.toJsonString()}");

            //紀錄結帳資訊
            var checkout = new CPS_Checkout
            {
                OrderId = oid,
                MemberId = member.Id,
                Ip = WebUtil.clientIP()
            };

            switch (payMethod)
            {
                case PayMethod.LinePay:
                    //這邊只有line pay要直接預約 
                    var record = OrderPayRecord.LinePay(order.Id);

                    var linePay = await getLinePayResult(order, record,"充電費用");

                    if (linePay.returnCode == LineCode.Request.Success)
                    {
                        var rid = record.Insert(true);

                        new OrderLinePay
                        {
                            RecordId = rid,
                            TransactionId = linePay.info.transactionId,
                            ReserveResult = linePay.toJsonString()
                        }.Insert();

                        checkout.Url = linePay.info.paymentUrl.web;
                    }
                    break;
                default://藍新信用卡
                    //到付款畫面在進行相關動作
                    checkout.Url = EkiNewebPay.mpgUrl(order.Serial);
                    break;
            }

            checkout.Insert();

            return checkout;
        }

        private async Task<LinePayResult.Reserve> getLinePayResult(
            CPS_Order order,OrderPayRecord record,string itemDesc="")
        {
            var lineReserve = new LinePayReserve()
            {
                orderId = record.EkiOrderSerial,
                redirectUrls = new LinePayReserve.RedirectUrl
                {
                    confirmUrl = EkiLinePay.Config.confirmUrl(order.Serial),
                    cancelUrl = EkiLinePay.Config.cancelUrl()
                },
                packages = new LinePayReserve.Packages()
                {
                    new LinePayReserve.Package()
                    {
                        products=new List<LinePayReserve.Product>
                         {
                             new LinePayReserve.Product()
                              {
                                 id = order.Serial,
                                 name = itemDesc,
                                 price = order.Cost.toInt()
                              }
                         }
                    }
                }
            };

            var client = new LinePay.Client(EkiLinePay.Config);

            return await client.ReserveAsync(lineReserve);
        }

        public class OrderCalculator
        {
            public static double calOrderCost(int min, CPS_ChargeConfig config)
            {
                return config.currency(config.Price * (min / 60d));
            }

        }


    }
}