using Eki_NewebPay;
using Eki_Web_MVC.CPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    /// <summary>
    /// 訂單付款完成之後處理流程
    /// </summary>
    public class OrderCheckOutProcess:BaseProcess
    {
        private CPS_Order order;
        public Func<bool> checkSuccess = () => false;//必填
        public Func<string> tradeNo = () => "";//選填
        public Func<string> card4No = () => "";//必填

        public OrderCheckOutProcess(CPS_Order o)
        {
            order = o;
        }

        public override void run()
        {
            if (checkSuccess())
            {
                //現在Pay/Return裡面 也有
                order.status = OrderStatus.End;
                order.Update();

                startInvoice(order);
            }
            //else
            //{

            //}

            
        }

        private void startInvoice(CPS_Order order)
        {
            try
            {
                var invoice = order.invoice;

                var member = order.member;

                var taxAmt = (order.Cost * (EkiNewebPay.Invoice.TaxRate / 100.0)).Round().toInt();
                var amt = order.Cost.toInt() - taxAmt;
                var request = invoice != null ? new NewebPayInvoiceModel
                {
                    TransNum = tradeNo(),
                    MerchantOrderNo = invoice.SerNO,
                    Category = invoice.BuyerUBN.isNullOrEmpty() ? NewebPayInvoice.Category_B2C : NewebPayInvoice.Category_B2B,
                    BuyerName = invoice.Name.isNullOrEmpty() ? member.Phone : invoice.Name,
                    BuyerUBN = invoice.BuyerUBN.isNullOrEmpty() ? "" : invoice.BuyerUBN,
                    BuyerAddress = invoice.Address,
                    BuyerEmail = invoice.Email.isNullOrEmpty() ? member.Mail : invoice.Email,
                    CarrierType = invoice.payCarrierType(),
                    CarrierNum = invoice.payCarrierType().isNullOrEmpty() ? "" : invoice.CarrierNum,
                    LoveCode = invoice.payCarrierType().isNullOrEmpty() ? invoice.BuyerUBN.isNullOrEmpty() ? invoice.LoveCode : "" : "",
                    PrintFlag = invoice.BuyerUBN.isNullOrEmpty() ? invoice.payCarrierType().isNullOrEmpty() && invoice.LoveCode.isNullOrEmpty() ? NewebPayInvoice.PrintFlag_Y : NewebPayInvoice.PrintFlag_N : NewebPayInvoice.PrintFlag_Y,
                    ItemName = EkiNewebPay.Invoice.ItemName,
                    ItemCount = 1,
                    ItemUnit = EkiNewebPay.Invoice.ItemUnit,
                    TaxRate = EkiNewebPay.Invoice.TaxRate
                } : new NewebPayInvoiceModel
                {
                    TransNum = tradeNo(),
                    MerchantOrderNo = order.Serial,
                    Category = NewebPayInvoice.Category_B2C,
                    BuyerName = member.Phone,
                    BuyerUBN = "",
                    BuyerAddress = "",
                    BuyerEmail = member.Mail,
                    CarrierType = "",
                    CarrierNum = "",
                    LoveCode = EkiNewebPay.Invoice.LoveCode,
                    ItemName = EkiNewebPay.Invoice.ItemName,
                    ItemCount = 1,
                    ItemUnit = EkiNewebPay.Invoice.ItemUnit,
                    TaxRate = EkiNewebPay.Invoice.TaxRate
                };

                request.Amt = amt;
                request.TaxAmt = taxAmt;

                request.TotalAmt = order.Cost.toInt();
                request.ItemAmt = invoice.BuyerUBN.isNullOrEmpty() ? order.Cost.toInt() : amt;

                //request.ItemPrice = invoice.BuyerUBN.isNullOrEmpty() ? order.Cost.toInt() : amt;

                request.Comment = $"信用卡末四碼:{card4No()}";

                var response = NewebPay.Invo().Post(request);

                //Log.print($"Invoice response->{response.toJsonString()}");

                InvoiceReturn.Load(response).Also(result =>
                {
                    result.MemberId = order.MemberId;
                    result.OrderId = order.Id;
                    result.InvoiceId = invoice?.Id ?? 0;
                }).Insert();
            }
            catch (Exception e)
            {
                Log.e("Checkout invoice error", e);
            }

        }
    }
}