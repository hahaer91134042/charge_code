using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC.CPS
{
    /// <summary>
    /// 設定該電表套用的費率
    /// 基本邏輯是 先找電表有沒有套用費率
    /// 沒有再找Community
    /// 沒有就直接找預設 (該年分)
    /// </summary>
    [DbTableSet("CPS_MeterRate")]
    public class CPS_MeterRate : BaseDbDAO
    {
        [DbRowKey("MeterId")]
        public int MeterId { get; set; }
        [DbRowKey("RateId")]
        public int RateId { get; set; }

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);

        public override int Insert(bool isReturnId = false) => EkiSQL.ekicps.insert(this, isReturnId);
    }
}