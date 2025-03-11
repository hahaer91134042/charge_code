using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC
{
    [Obsolete]
    [DbTableSet("SysMenuGroup")]
    public class SysMenuGroup : BaseDbDAO
    {
        [DbRowKey("Name",DbAction.Update)]
        public string Name { get; set; }
        [DbRowKey("beEnable", DbAction.Update)]
        public bool beEnable { get; set; } = true;


        /// <summary>
        /// 需跟Group的權限比
        /// if group Lv > GroupLv
        /// 表示可以使用
        /// </summary>
        [DbRowKey("GroupLv",DbAction.Update)]
        public int GroupLv { get; set; }


        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);

        public override int Insert(bool isReturnId = false) => EkiSQL.ekicps.insert(this, isReturnId);

        public override bool Update() => EkiSQL.ekicps.update(this);

    }
}