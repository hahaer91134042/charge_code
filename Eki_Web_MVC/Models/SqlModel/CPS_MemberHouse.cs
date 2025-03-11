using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC.CPS
{
    [DbTableSet("CPS_MemberHouse")]
    public class CPS_MemberHouse : BaseDbDAO
    {
        [DbRowKey("MemberId")]
        public int MemberId { get; set; }
        [DbRowKey("HouseId")]
        public int HouseId { get; set; }

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);
        public override int Insert(bool isReturnId = false) => EkiSQL.ekicps.insert(this, isReturnId);
        public override bool Delete() => EkiSQL.ekicps.delete(this);
    }
}