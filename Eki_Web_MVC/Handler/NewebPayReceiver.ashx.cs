using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;
using Eki_NewebPay;
using Eki_Web_MVC.CPS;

namespace Eki_Web_MVC.Handler
{
    /// <summary>
    /// NewebPayReceiver 的摘要描述
    /// </summary>
    public class NewebPayReceiver : BaseHandler
    {
        override protected void GetRequest(HttpContext context)
        {
            var ord = context.Request["ord"];

            var order = EkiSQL.ekicps.data<CPS_Order>(
                "where Serial=@s and beEnable=1",
                new { s = ord });

            if (order.isNullOrEmpty())
                throw new ArgumentNullException();

            Log.d($"NewebPay Receiver order->{order.Serial}");

            try
            {

                var returnModel = NewebPayMPGReturn.Load(context.Request);

                if (returnModel.Parse())
                {
                    #region 紀錄約定信用卡
                    if (returnModel.MPG.hasPayToken())
                    {
                        var mPayInfo = EkiSQL.ekicps.data<CPS_MemberPayInfo>(
                            "where MemberId=@mid",
                            new { mid = order.MemberId });
                        if (mPayInfo == null)
                        {
                            mPayInfo = new CPS_MemberPayInfo
                            {
                                MemberId = order.MemberId,
                                neweb = new CPS_MemberPayInfo.Info_Neweb
                                {
                                    Token = returnModel.MPG.Result.TokenValue,
                                    TokenLife = returnModel.MPG.Result.TokenLife
                                }
                            };

                            mPayInfo.Insert();
                        }
                        else
                        {
                            mPayInfo.neweb.Token = returnModel.MPG.Result.TokenValue;
                            mPayInfo.neweb.TokenLife = returnModel.MPG.Result.TokenLife;
                            mPayInfo.Update();
                        }
                    }
                    #endregion


                    NewebPayReturn.Load(returnModel).Also(rModel =>
                    {
                        rModel.OrderId = order.Id;
                        rModel.Insert();

                        new OrderCheckOutProcess(order)
                        {
                            checkSuccess = () => returnModel.Status == "SUCCESS",
                            tradeNo = () => rModel.TradeNo,
                            card4No = () => rModel.Card4No
                        }.run();

                    });
                }
                else
                {
                    //rBuilder.Append("Can`t parse model");
                    //PayStatusText.Text = "交易結果出現異常";
                }
            }
            catch (Exception e)
            {

                Log.e("NewebPay Receiver error", e);
            }

        }
    }
}