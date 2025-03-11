using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC
{
    [DbTableSet("SysGroup")]
    public class SysGroup : BaseDbDAO,IConvertResponse<object>
    {
        [DbRowKey("Title", DbAction.Update)]
        public string Title { get; set; } = "";
        [DbRowKey("Describe", DbAction.Update)]
        public string Describe { get; set; } = "";
        [DbRowKey("cDate", RowAttribute.CreatTime, true)]
        public DateTime cDate { get; set; }
        [DbRowKey("beEnable", DbAction.Update)]
        public bool beEnable { get; set; } = true;
        [DbRowKey("Lv", DbAction.Update)]
        public int Lv { get; set; } = 0; //表示權限等級

        public object convertToResponse()
            => new
            {
                Title,
                Describe,
                Lv
            };

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);

        public override int Insert(bool isReturnId = false) => EkiSQL.ekicps.insert(this, isReturnId);

        public override bool Update() => EkiSQL.ekicps.update(this);
    }
}