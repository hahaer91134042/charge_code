using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC.CPS
{
    [DbTableSet("CPS_LocationInfo")]
    public class CPS_LocationInfo : BaseDbDAO,IConvertResponse<object>
    {
        [DbRowKey("InfoContent",DbAction.Update)]
        public string InfoContent { get; set; }

        public object convertToResponse()
            => new
            {
                Content=InfoContent
            };

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);

        public override int Insert(bool isReturnId = false) => EkiSQL.ekicps.insert(this, isReturnId);

        public override bool Update() => EkiSQL.ekicps.update(this);
        public override bool Delete() => EkiSQL.ekicps.delete(this);
    }
}