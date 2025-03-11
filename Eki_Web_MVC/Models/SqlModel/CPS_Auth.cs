using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC.CPS
{
    [DbTableSet("CPS_Auth")]
    public class CPS_Auth : BaseDbDAO,IConvertResponse<object>
    {
        [DbRowKey("CpSerial", DbAction.Update, false)]
        public string CpSerial { get; set; }
        [DbRowKey("Auth",DbAction.Update,false)]
        public string Auth { get; set; }
        [DbRowKey("beEnable", DbAction.Update)]
        public bool beEnable { get; set; } = true;
        [DbRowKey("cDate", RowAttribute.CreatTime, true)]
        public DateTime cDate { get; set; }

        public object convertToResponse()
            => new
            {
                Id,
                CpSerial,
                Auth,
                beEnable,
                cDate
            };
        public static bool checkAuth(string cpSerial, string auth)
            => EkiSQL.ekicps.hasData<CPS_Auth>(QueryPair.init("CpSerial", cpSerial)
                .addQuery("Auth", auth)
                .addQuery("beEnable", 1));

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id,this);

        public override int Insert(bool isReturnId = false) => EkiSQL.ekicps.insert(this, isReturnId);

        public override bool Update()
        {
            return EkiSQL.ekicps.update(this);
        }
        public override bool Delete()
        {
            return EkiSQL.ekicps.delete(this);
        }
    }
}