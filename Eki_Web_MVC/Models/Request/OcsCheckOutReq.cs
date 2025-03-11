using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eki_Web_MVC.CPS;

namespace Eki_Web_MVC
{
    public class OcsCheckOutReq:RequestDAO
    {
        public string phone { get; set; } = "";
        public string cp { get; set; } = "";
        public int chargeMin { get; set; } = 0;
        public int payMethod { get; set; } = 0;
        public Invoice invo { get; set; }


        public class Invoice
        {
            public int type { get => typeEnum.toInt(); set => typeEnum = value.toEnum<InvoiceType>(); }
            public string serial { get; set; } = "";
            public string address { get; set; } = "";//紙本才需要 (type=統編才需要)
            public string name { get; set; }//統編公司名稱

            public InvoiceType typeEnum = InvoiceType.ezPay;

            public CPS_Invoice toCpsInvo(CPS_Member member)
            {
                var invo = new CPS_Invoice() { Type = type, MemberId = member.Id };

                switch (typeEnum)
                {
                    case InvoiceType.Donate:
                        invo.Name = member.Phone;
                        invo.LoveCode = serial;
                        break;
                    case InvoiceType.Paper:
                        //目前紙本只提供給公司統編的方式
                        invo.Name = name;
                        invo.BuyerUBN = serial;
                        invo.Address = address;
                        break;
                    case InvoiceType.Certificate:
                    case InvoiceType.Phone:
                        invo.Name = member.Phone;
                        invo.CarrierNum = serial;
                        break;
                    case InvoiceType.ezPay:
                        invo.Name = member.Phone;
                        invo.CarrierNum = member.Phone;
                        break;
                    default:
                        break;
                }

                return invo;
            }
        }

    }
}