using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC.CPS
{
    [DbTableSet("CPS_Address")]
    public class CPS_Address : BaseDbDAO,IConvertResponse<object>
    {
        [DbRowKey("Code", DbAction.Update)]
        public string Code { get; set; } = "";
        [DbRowKey("Detail", DbAction.Update)]
        public string Detail { get; set; } = "";

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);

        public override int Insert(bool isReturnId = false) => EkiSQL.ekicps.insert(this, isReturnId);
        public override bool Update() => EkiSQL.ekicps.update(this);
        public override bool Delete() => EkiSQL.ekicps.delete(this);

        public object convertToResponse()
            => new
            {
                Code,
                Detail
            };
    }
}