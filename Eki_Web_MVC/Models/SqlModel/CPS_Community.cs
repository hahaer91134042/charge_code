using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;
using Newtonsoft.Json;

namespace Eki_Web_MVC.CPS
{
    [DbTableSet("CPS_Community")]
    public class CPS_Community : BaseDbDAO, IConvertResponse<object>,ISession
    {
        [DbRowKey("Name", DbAction.Update)]
        public string Name { get; set; } = "";
        [DbRowKey("Pwd", DbAction.Update)]
        public string Pwd { get; set; }
        [DbRowKey("Uid", RowAttribute.Guid, true)]
        public Guid Uid { get; set; }
        [DbRowKey("cDate", RowAttribute.CreatTime, true)]
        public DateTime cDate { get; set; }
        [DbRowKey("uDate", RowAttribute.NowTime, DbAction.UpdateOnly, true)]
        public DateTime uDate { get; set; }
        [DbRowKey("beEnable", DbAction.Update)]
        public bool beEnable { get; set; } = true;
        [DbRowKey("AddressId")]
        public int AddressId { get; set; }

        [DbRowKey("LoginIP", DbAction.Update)]
        public string LoginIP { get; set; }

        public DateTime LoginTime;

        public CPS_Address address 
        {
            get
            {
                if (_addr == null)
                {
                    _addr = new CPS_Address();
                    if (AddressId > 0)
                        _addr.CreatById(AddressId);
                }
                return _addr;
            }
            set
            {
                _addr = value;
            }
        }
        public CPS_Bank bank
        {
            get
            {
                if (_bank == null)
                {
                    _bank = EkiSQL.ekicps.count("select count(*) from CPS_Bank where beEnable=1 and CommunityId=@cid ", new { cid = Id }) > 0 ?
                        EkiSQL.ekicps.data<CPS_Bank>("where beEnable=1 and CommunityId=@cid", new { cid = Id }) :
                        new CPS_Bank();
                }
                return _bank;
            }
            set
            {
                _bank = value;
            }
        }
        public string decodePwd
        {
            get => EkiEncoder.AdminUser.decode(Pwd).pwd;
        }
        public List<CPS_ElectricMeter> meter
        {
            get
            {
                if (_meter == null)
                {
                    _meter = EkiSQL.ekicps.table<CPS_ElectricMeter>(
                        "select * from CPS_ElectricMeter where CommunityId = @cid",
                        new { cid = Id }
                        ).toSafeList();
                }
                return _meter;
            }
        }

        public List<CPS_Location> location
        {
            get
            {
                if (_loc == null)
                {
                    _loc = EkiSQL.ekicps.table<CPS_Location>(
                        "select * from CPS_Location where CommunityId = @cid",
                        new { cid = Id }
                        ).toSafeList();
                }
                return _loc;
            }
        }
        public DbObjList<CPS_House> house
        {
            get
            {
                if (_house == null)
                    _house = new DbObjList<CPS_House>(
                        EkiSQL.ekicps.table<CPS_House>(
                            "select * from CPS_House where CommunityId=@cid",
                            new { cid = Id }));

                return _house;
            }
        }

        private CPS_Address _addr = null;
        private List<CPS_ElectricMeter> _meter = null;
        private List<CPS_Location> _loc = null;
        private DbObjList<CPS_House> _house = null;
        private CPS_Bank _bank = null;

        public object convertToResponse()
            => new
            {
                Id,
                Name,
                Pwd=decodePwd,
                LoginIP,
                beEnable,
                cDate,
                Uid,
                Address=address.convertToResponse(),
                Bank = bank.convertToResponse(),
                Meter=(from m in meter
                       select m.convertToResponse())
            };

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);
        public override int Insert(bool isReturnId = false)
        {
            AddressId = address.Insert(true);
            var cid= EkiSQL.ekicps.insert(this, isReturnId);
            bank.CommunityId = cid;
            bank.Insert();

            return cid;
        }
        public override bool Update()
        {
            if (!(address.Update()&&bank.Update()))
                return false;
            
            return EkiSQL.ekicps.update(this);
        }

        public static bool updateIp(CPS_Community com,string ip)
        {
            com.LoginIP = ip;
            return EkiSQL.ekicps.update("set LoginIP=@cip", new { cip = ip }, com);
        }

        public void setBank(string name, CPS_Bank.BankDecode data)
        {
            var crypto = EkiEncoder.AES.encode(data);

            bank.Name = name;
            bank.Salt = crypto.publicKey;
            bank.Cipher = crypto.cipher;
        }

        public static string creatPwdCipher(string text)
        {
            return EkiEncoder.AdminUser.encode(new AdminPwd { pwd = text }).cipher;
        }

        public static CPS_Community creat(string acc, string pwd)
        {
            //var ciper = EncryptPwd(pwd);
            var ciper = creatPwdCipher(pwd);
            //Log.d($"SysManager Cipher->{ciper}");

            return EkiSQL.ekicps.data<CPS_Community>("where Name=@name and Pwd=@pwd",
                new { name = acc, pwd = ciper });

            //return (from m in EkiSQL.ekicps.table<SysManager>()
            //        where m.Account == acc
            //        where m.Pwd == ciper
            //        select m).FirstOrDefault();
        }
        public static CPS_Community fromSession() => new CPS_Community().getSession<CPS_Community>();
        public string sessionFlag() => AppFlag.CommunityUser;
    }
}