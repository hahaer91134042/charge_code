using DevLibs;
using Eki_NewebPay;
using Eki_Web_MVC.CPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eki_Web_MVC
{
    [RoutePrefix("Pay")]
    public class PayController : IBaseViewController<PayModel>
    {

        [Route("Credit/{serial}")]
        public ActionResult Index(string serial)
        {
            var order = EkiSQL.ekicps.data<CPS_Order>(
                $"where Serial=@s and Status={OrderStatus.CheckOut.toInt()} and beEnable=1",
                new { s = serial });
            if (order == null)
                HttpContext.Response.StatusCode = 404;

            var member = new CPS_Member().Also(m => m.CreatById(order.MemberId));
            var checkout = order.checkout;

            Log.d($"Pay Credit order->{order.toJsonString()}");

            var record = OrderPayRecord.NewebPay(order.Id);
            record.Insert();

            var creditModel = new NewebPayMPGModel.CreditCard()
            {
                TimeStamp = NewebPayUtil.ConvertToStamp(checkout.Date).ToString(),
                //TimeStamp = NewebPayUtil.ConvertToStamp(DateTime.Now).ToString(),
                LangType = NewebPayMPG.Config.LangType_Tw,
                MerchantOrderNo = record.EkiOrderSerial,
                //MerchantOrderNo="EKI11111222333",
                Amt = order.Cost.toInt(),//目前沒有貨幣類別的換算
                ItemDesc = "停車費用",
                ReturnURL = EkiNewebPay.MPG.config().returnUrl(order.Serial),
                NotifyURL = EkiNewebPay.MPG.config().notifyUrl(order.Serial),
                TokenTerm = member.Phone,
                //ClientBackURL=WebUtil.getWebURL(),
                Email = member.Mail
            };

            NewebPay.MPG().Post(creditModel);


            return View(new
            {
                Serial = serial,
                Order = order,
                Record = record,
                Credit = creditModel
            });
        }

        [Route("Credit/Return/{serial}")]
        public ActionResult Return(string serial)
        {
            var order = EkiSQL.ekicps.data<CPS_Order>(
                $"where Serial=@s and beEnable=1",
                new { s = serial });
            if (order == null)
                HttpContext.Response.StatusCode = 404;

            var mpgReturn = NewebPayMPGReturn.Load(Request);

            if (mpgReturn.Parse())
            {
                if (mpgReturn.Status == "SUCCESS")
                {
                    //要開啟充電
                    ViewBag.Msg = "交易完成 開啟充電";

                    order.status = OrderStatus.End;
                    order.Update();

                    new CpsApi.AdminStartCharge(order.Serial)
                        .Connect<object>();

                    var cp = EkiSQL.ekicps.data<CPS_ChargePoint>(
                        "where Serial=@s",
                        new { s = order.CpSerial });

                    return new RedirectResult($"~/ocs/charge/{cp.Serial}");
                }
                else
                {

                    ViewBag.Msg = "交易失敗";
                }
            }
            else
            {
                ViewBag.Msg = "交易失敗";
            }

            return View(mpgReturn.MPG);
        }

        [Route("Line/Confirm")]
        public ActionResult LineConfirm()
        {


            return View();
        }

        [Route("Line/Cancel")]
        public ActionResult LineCancel()
        {


            return View();
        }
    }
}