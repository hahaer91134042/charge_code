using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC.CPS
{
    [DbTableSet("CPS_ChargePoint")]
    public class CPS_ChargePoint : BaseDbDAO, IConvertResponse<object>
    {
        [DbRowKey("CommunityId")]
        public int CommunityId { get; set; }//一定要綁定社區
        [DbRowKey("MeterId")]
        public int MeterId { get; set; }//可以不用綁定電表
        [DbRowKey("Serial",DbAction.Update)]
        public string Serial { get; set; }
        [DbRowKey("Remarker", DbAction.Update)]
        public string Remarker { get; set; } = "";
        [DbRowKey("cDate", RowAttribute.CreatTime, true)]
        public DateTime cDate { get; set; }
        [DbRowKey("Uid", RowAttribute.Guid, true)]
        public Guid Uid { get; set; }
        [DbRowKey("beEnable", DbAction.Update)]
        public bool beEnable { get; set; } = true;

        public DbObjList<CPS_Auth> Auth
        {
            get
            {   
                if (_auth == null)
                {
                    _auth = new DbObjList<CPS_Auth>(
                        EkiSQL.ekicps.table<CPS_Auth>(
                            "select * from CPS_Auth where CpSerial=@serial",
                            new { serial=Serial})
                        );
                }
                return _auth;
            }
        }

        private DbObjList<CPS_Auth> _auth = null;        

        public object convertToResponse()
            => new
            {
                Id,
                Remarker,
                Serial,
                beEnable,
                cDate
            };

        public CPS_Community findCommunity()
        {
            return new CPS_Community().Also(c => c.CreatById(CommunityId));
            //var meter = new CPS_ElectricMeter().Also(m => m.CreatById(MeterId));
            //return new CPS_Community().Also(c => c.CreatById(meter.CommunityId));
        }

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);
        public override int Insert(bool isReturnId = false) => EkiSQL.ekicps.insert(this, isReturnId);
        public override bool Update() => EkiSQL.ekicps.update(this);
        public override bool Delete() => EkiSQL.ekicps.delete(this);

        public static CPS_ChargePoint CreatBySerial(string serial)
        {
            //if (!EkiSQL.ekicps.hasData<CPS_ChargePoint>(QueryPair.init("Serial", serial)))
            //    return null;
            return (from cp in EkiSQL.ekicps.table<CPS_ChargePoint>()
                    where cp.Serial == serial
                    select cp).FirstOrDefault();
        }
        public static CPS_Community findCommunity(string cpSerial)
        {
            return (from cp in EkiSQL.ekicps.table<CPS_ChargePoint>()
                    where cp.Serial == cpSerial
                    //where cp.beEnable
                    //join m in EkiSQL.ekicps.table<CPS_ElectricMeter>() on cp.MeterId equals m.Id
                    join c in EkiSQL.ekicps.table<CPS_Community>() on cp.CommunityId equals c.Id
                    select c).FirstOrDefault();
        }
    }
}