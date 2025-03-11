using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC.CPS
{
    [DbTableSet("CPS_Member")]
    public class CPS_Member : BaseDbDAO,
                                                 IPhoneMap,
                                                 IConvertResponse<object>,
                                                 IPushSet,ICryptoPwd,IToken
    {
        [DbRowKey("Mail", DbAction.Update)]
        public string Mail { get; set; }
        [DbRowKey("Phone", DbAction.Update, false)]
        public string Phone { get; set; }
        [DbRowKey("Pwd", DbAction.Update)]
        public string Pwd { get; set; }
        [DbRowKey("Salt", DbAction.Update)]
        public string Salt { get; set; }
        [DbRowKey("Ip", DbAction.Update, true)]
        public string Ip { get; set; }
        [DbRowKey("beEnable", DbAction.Update)]
        public bool beEnable { get; set; }
        [DbRowKey("Uid", RowAttribute.Guid, true)]
        public Guid Uid { get; set; }
        [DbRowKey("cDate", RowAttribute.CreatTime, true)]
        public DateTime cDate { get; set; }
        [DbRowKey("uDate", RowAttribute.NowTime, DbAction.UpdateOnly, true)]
        public DateTime uDate { get; set; }
        [DbRowKey("sDate", RowAttribute.Time, DbAction.UpdateOnly, true)]
        public DateTime sDate { get; set; }

        [DbRowKey("MobileType", DbAction.Update, true)]
        public int MobileType { get { return mobilType.toInt(); } set { mobilType = value.toEnum<MobilType>(); } }
        [DbRowKey("PushToken", DbAction.Update)]
        public string PushToken { get; set; }
        [DbRowKey("Lan", DbAction.Update, true)]
        public string Lan { get { return memberLan.ToString(); } set { memberLan = value.toEnum<LanguageFamily>(); } } 
        [DbRowKey("Lv", DbAction.Update)]
        public int Lv { get; set; } = 0;//等級
        [DbRowKey("InfoID")]
        public int InfoID { get; set; }

        public CPS_MemberInfo info
        {
            get
            {
                if (_info == null)
                {
                    _info = new CPS_MemberInfo().Also(i => i.CreatById(InfoID));
                }
                return _info;
            }
        }

        public List<CPS_House> house
        {
            get
            {
                if (_house == null)
                    _house = (from mh in EkiSQL.ekicps.table<CPS_MemberHouse>()
                              where mh.MemberId == Id
                              join h in EkiSQL.ekicps.table<CPS_House>() on mh.HouseId equals h.Id
                              where h.beEnable
                              select h).toSafeList();
                return _house;
            }
        }
        
        public MobilType mobilType = MobilType.Android;
        public LanguageFamily memberLan = LanguageFamily.TC;

        private CPS_MemberInfo _info;
        private List<CPS_House> _house;

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);

        public override int Insert(bool isReturnId = false)
        {
            InfoID = info.Insert(true);
            return EkiSQL.ekicps.insert(this, isReturnId);
        }

        public override bool Update()
        {
            info.Update();
            return EkiSQL.ekicps.update(this);
        }

        public override bool CreatByUniqueId(string uniqueId)
        {
            return EkiSQL.ekicps.loadDataByQueryPair(QueryPair.init("Uid", uniqueId), this);
        }

        public string countryCode() => info.CountryCode;
        public string phone() => Phone;
        public string socketID() => Uid.ToString();
        public string fcmToken() => PushToken;
        public MobilType device() => mobilType;

        public object convertToResponse()
            => new
            {
                Mail,
                Phone,
                Info=info.convertToResponse(),
                Lan,
                Lv
            };

        public string newSalt() => SecurityBuilder.CreateSaltKey(5);
        public void setSalt(string salt) => Salt = salt;
        public void setCipher(string cipher) => Pwd = cipher;
        public string salt() => Salt;
        public string cipher() => Pwd;
        public HashFormat format() => HashFormat.SHA256;
        public string tokenRaw() => Uid.ToString();
    }
}