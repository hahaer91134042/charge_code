using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC.CPS
{
    [DbTableSet("CPS_ElectricMeter")]
    public class CPS_ElectricMeter : BaseDbDAO, IConvertResponse<object>
    {
        [DbRowKey("CommunityId")]
        public int CommunityId { get; set; }
        [DbRowKey("PaySerial",DbAction.Update)]
        public string PaySerial { get; set; }//電號 電費單上
        [DbRowKey("MeterSerial",DbAction.Update)]
        public string MeterSerial { get; set; }//表號 電表上
        [DbRowKey("Marker", DbAction.Update)]
        public string Marker { get; set; } = "";
        [DbRowKey("cDate", RowAttribute.CreatTime, true)]
        public DateTime cDate { get; set; }
        [DbRowKey("Uid", RowAttribute.Guid, true)]
        public Guid Uid { get; set; }
        [DbRowKey("beEnable", DbAction.Update)]
        public bool beEnable { get; set; } = true;

        public DbObjList<CPS_ChargePoint> CP
        {
            get
            {
                if (_cp == null)
                {
                    _cp = new DbObjList<CPS_ChargePoint>(
                        EkiSQL.ekicps.table<CPS_ChargePoint>(
                            "select * from CPS_ChargePoint where MeterId=@mid",
                            new { mid = Id })
                        );
                }

                return _cp;
            }
        }

        public bool hasCP()
        {
            return EkiSQL.ekicps.count<CPS_ChargePoint>(
                QueryPair.init("MeterId", Id)) > 0;
        }

        private DbObjList<CPS_ChargePoint> _cp = null;

        public object convertToResponse()
            => new
            {
                Id,
                PaySerial,
                MeterSerial,
                Marker,
                cDate,
                beEnable
            };

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);

        public override int Insert(bool isReturnId = false) => EkiSQL.ekicps.insert(this, isReturnId);

        public override bool Update()
        {
            return EkiSQL.ekicps.update(this);
        }
    }
}