using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC.CPS
{
    [DbTableSet("CPS_Checkout")]
    public class CPS_Checkout : BaseDbDAO,IConvertResponse<object>
    {

        [DbRowKey("OrderId", false)]
        public int OrderId { get; set; }
        /// <summary>
        /// 紀錄車主的ID
        /// </summary>
        [DbRowKey("MemberId")]
        public int MemberId { get; set; }
        [DbRowKey("ApplyData", DbAction.Update)]
        public string ApplyData { get; set; } = "";//看之後要記錄什麼活動折扣的資料再用

        /// <summary>
        /// CostFix 基本上是負數 用來記錄活動跟使用優惠券所扣除的金額
        /// </summary>
        [DbRowKey("CostFix")]
        public double CostFix { get; set; }
        /// <summary>
        /// Claimant 紀錄車主違停的罰金
        /// </summary>
        [DbRowKey("Claimant")]
        public double Claimant { get; set; } = 0;//車主的總罰金

        [DbRowKey("Date", RowAttribute.Time, false)]
        public DateTime Date { get; set; } = DateTime.Now;
        [DbRowKey("cDate", RowAttribute.CreatTime, true)]
        public DateTime cDate { get; set; }
        [DbRowKey("Url")]
        public string Url { get; set; }
        [DbRowKey("Ip", DbAction.Update, true)]
        public string Ip { get; set; } = "";

        public object convertToResponse()
            => new
            {
                Claimant,
                CostFix,
                Date=Date.toString(),
                Url
            };

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);

        public override int Insert(bool isReturnId = false) => EkiSQL.ekicps.insert(this, isReturnId);
        public override bool Update()
        {
            return EkiSQL.ekicps.update(this);
        }
    }
}