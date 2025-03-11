using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC
{
    [DbTableSet("SysAction")]
    public class SysAction : BaseDbDAO
    {
        [DbRowKey("Name")]
        public string Name { get; set; }
        [DbRowKey("ActKey")]
        public string ActKey { get; set; }

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);

        public override int Insert(bool isReturnId = false) => EkiSQL.ekicps.insert(this, isReturnId);
    }
}