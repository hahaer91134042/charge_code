using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC
{
    /// <summary>
    /// OrderLinePay 的摘要描述
    /// </summary>
    [DbTableSet("OrderLinePay")]
    public class OrderLinePay : BaseDbDAO
    {
        [DbRowKey("RecordId")]
        public int RecordId { get; set; }
        [DbRowKey("TransactionId")]
        public long TransactionId { get; set; }
        [DbRowKey("ReserveResult", DbAction.Update)]
        public string ReserveResult { get; set; } = "";
        [DbRowKey("ConfirmResult", DbAction.Update)]
        public string ConfirmResult { get; set; } = "";

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);

        public override int Insert(bool isReturnId = false) => EkiSQL.ekicps.insert(this, isReturnId);
        public override bool Update() => EkiSQL.ekicps.update(this);
    }
}
