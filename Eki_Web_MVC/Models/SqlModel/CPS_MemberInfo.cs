using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC.CPS
{
    [DbTableSet("CPS_MemberInfo")]
    public class CPS_MemberInfo : BaseDbDAO,IConvertResponse<object>
    {
        [DbRowKey("Name", DbAction.Update)]
        public string Name { get; set; }
        [DbRowKey("CountryCode",DbAction.Update)]
        public string CountryCode { get; set; }
        [DbRowKey("Phone", DbAction.Update)]
        public string Phone { get; set; }
        [DbRowKey("Img", DbAction.Update)]
        public string Img { get; set; }

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);
        public override int Insert(bool isReturnId = false) => EkiSQL.ekicps.insert(this, isReturnId);
        public override bool Update() => EkiSQL.ekicps.update(this);
        public override bool Delete() => EkiSQL.ekicps.delete(this);

        public object convertToResponse()
            => new
            {
                Name,
                CountryCode,
                Phone
            };
    }
}