using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC.CPS
{
    /// <summary>
    /// 這張表 主要是記錄社區底下的停車位資訊 之後可以跟PPYP裡面的Location對應
    /// </summary>
    [DbTableSet("CPS_Location")]
    public class CPS_Location:BaseDbDAO,IConvertResponse<object>
    {
        [DbRowKey("CommunityId")]
        public int CommunityId { get; set; }
        [DbRowKey("InfoId")]
        public int InfoId { get; set; }
        [DbRowKey("CpSerial", DbAction.Update)]
        public string CpSerial { get; set; }
        [DbRowKey("SerNum", DbAction.Update)]
        public string SerNum { get; set; }//之後再考慮要不要加入
        [DbRowKey("Role",DbAction.Update)]
        public int Role { get => locRole.toInt(); set => locRole = value.toEnum<LocRole>(); }
        //[DbRowKey("Lat", DbAction.Update)]
        //public double Lat { get; set; }
        //[DbRowKey("Lng", DbAction.Update)]
        //public double Lng { get; set; }
        [DbRowKey("beEnable", DbAction.Update)]
        public bool beEnable { get; set; } = true;
        [DbRowKey("cDate", RowAttribute.CreatTime, true)]
        public DateTime cDate { get; set; }
        [DbRowKey("Uid", RowAttribute.Guid, true)]
        public Guid Uid { get; set; }

        public CPS_LocationInfo info
        {
            get
            {
                if (_info == null)
                    _info = new CPS_LocationInfo().Also(i => i.CreatById(InfoId));
                return _info;
            }
        }
        public CPS_ChargeConfig config
        {
            get
            {
                if (_config == null)
                    _config = EkiSQL.ekicps.data<CPS_ChargeConfig>(
                        "where LocationId=@loc",
                        new { loc = Id });
                return _config;
            }
        }
        

        public LocRole locRole = LocRole.Private;

        private CPS_LocationInfo _info;
        private CPS_ChargeConfig _config;

        public object convertToResponse()
            => new
            {
                Id,
                CpSerial,
                SerNum,
                Info=info.convertToResponse(),
                Role=Role,
                cDate,
                beEnable,
                Config=config?.convertToResponse()
            };

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);

        public override int Insert(bool isReturnId = false)
        {

            return EkiSQL.ekicps.insert(this, isReturnId);
        }
        public override bool Update()
        {
            if (!info.Update())
                return false;            

            return EkiSQL.ekicps.update(this);
        }
        public override bool Delete()
        {
            if (!info.Delete())
                return false;

            return EkiSQL.ekicps.delete(this);
        }

        public static CPS_Location findByCpSerial(string cpSerial)
        {
            return EkiSQL.ekicps
                .data<CPS_Location>(
                "where beEnable=1 and CpSerial=@cSer",
                new { cSer = cpSerial });
        }
        
        public enum LocRole
        {
            Private=0,//私人停車位
            Public=1//公用停車位
        }
    }
}