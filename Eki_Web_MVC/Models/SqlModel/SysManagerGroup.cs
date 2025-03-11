using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC
{
    //目前先不使用
    [Obsolete]
    [DbTableSet("SysManagerGroup")]
    public class SysManagerGroup : BaseDbDAO
    {
        [DbRowKey("ManagerId")]
        public int ManagerId { get; set; }
        [DbRowKey("GroupId",DbAction.Update)]
        public int GroupId { get; set; }

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);

        public override int Insert(bool isReturnId = false) => EkiSQL.ekicps.insert(this, isReturnId);

        public override bool Update() => EkiSQL.ekicps.update(this);
    }
}