using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;
using Eki_Web_MVC.CPS;

namespace Eki_Web_MVC
{
    public class AdminModel:IBaseManagerModel
    {
        public SysManager user;
        public AdminModel(SysManager u)
        {
            user = u;
        }


        #region LeftMenu
        public List<MenuSession> getUserMenu()
        {
            var leftMenu = findMenu(EkiSQL.ekicps.table<SysMenu>(), 0);

            //Log.d($"menu data->{leftMenu.toJsonString()}");

            return leftMenu;
        }

        private List<MenuSession> findMenu(IEnumerable<SysMenu> menus, int parentID)
        {
            if (user == null)
                return new List<MenuSession>();

            var result = (from main in menus
                          where main.beEnable
                          where user.Group.Lv >= main.GroupLv
                          //where 0 >= main.GroupLv
                          where main.ParentId == parentID
                          orderby main.Sort ascending
                          select new MenuSession()
                          {
                              Main = new MenuSession.Item().copyRowFrom(main),
                              Sub = findMenu(menus.Where(m => m.ParentId != parentID), main.Id)
                          });

            return result.toSafeList();
        }
        #endregion

        #region Community
        public IEnumerable<object> getCommunity()
            => (from c in EkiSQL.ekicps.table<CPS_Community>()

                select c.convertToResponse());

        public void addCommunity(CommunityReq req)
        {
            var community = new CPS_Community().Also(com =>
            {
                com.Name = req.name;
                com.Pwd = CPS_Community.creatPwdCipher(req.pwd);
                com.beEnable = req.beEnable;
                com.address = new CPS_Address()
                {
                    Code=req.address.code,
                    Detail=req.address.detail
                };
                com.setBank(req.bank.name, new CPS_Bank.BankDecode
                {
                    Serial=req.bank.serial,
                    Account=req.bank.account,
                    Code=req.bank.code,
                    Sub=req.bank.sub
                });
                

            });

            var comID = community.Insert(true);
            if (comID < 1)
                throw new ArgumentException();

            var success = req.meter.convertToDbObjList<CPS_ElectricMeter>()
                .InsertByObj(meter =>
                {
                    meter.CommunityId = comID;
                });

            if (!success)
                throw new ArgumentException();
        }

        public void editCommunity(CommunityReq req)
        {
            var com = new CPS_Community().Also(c => c.CreatById(req.id));

            com.Name = req.name;
            com.Pwd = CPS_Community.creatPwdCipher(req.pwd);
            com.beEnable = req.beEnable;

            com.address.Code = req.address.code;
            com.address.Detail = req.address.detail;

            com.setBank(req.bank.name, new CPS_Bank.BankDecode
            {
                Serial = req.bank.serial,
                Account = req.bank.account,
                Code = req.bank.code,
                Sub = req.bank.sub
            });

            if (!com.Update())
                throw new ArgumentException();

        }

        public void addCommunityMeter(CommunityReq req)
        {
            try
            {
                var meters = req.meter.convertToDbObjList<CPS_ElectricMeter>();

                meters.InsertByObj(m =>
                {
                    m.CommunityId = req.id;
                });

            }
            catch (Exception)
            {
                throw new ArgumentException();
            }         
        }

         public void removeMeter(CommunityReq req)
        {
            var community = new CPS_Community().Also(c => c.CreatById(req.id));

            //要刪除的meter沒有包含在該社區內
            if (!req.meter.Any(d => community.meter.Any(m => m.PaySerial == d.paySerial)))
                throw new ArgumentException();

            //這邊檢查該電表底下有無CP 無CP才能刪除
            var meters = new DbObjList<CPS_ElectricMeter>(
                from m in req.meter.convertToDbObjList<CPS_ElectricMeter>()
                where !m.hasCP()
                select m);

            
            if (meters.Count<1)
                throw new ArgumentException();

            if (!meters.DeleteInDb())
                throw new ArgumentException();

        }

        public void editMeter(CommunityReq req)
        {
            var community = new CPS_Community().Also(c => c.CreatById(req.id));

            //要修改的meter沒有包含在該社區內
            if (!req.meter.Any(d => community.meter.Any(m => m.Id == d.id)))
                throw new ArgumentException();

            var meters = new DbObjList<CPS_ElectricMeter>(
                from rMeter in req.meter
                join m in community.meter on rMeter.id equals m.Id
                select m.Also(meter =>
                {
                    meter.PaySerial = rMeter.paySerial;
                    meter.MeterSerial = rMeter.meterSerial;
                    meter.Marker = rMeter.marker;
                    meter.beEnable = rMeter.beEnable;
                }));

            if (!meters.UpdateByObj())
                throw new ArgumentException();

        }
        public object getLocation(LocationReq req) => getLocation(req.cID);
        public object getLocation(int cID)
        {
            var community = (from c in EkiSQL.ekicps.table<CPS_Community>()
                             where c.Id == cID
                             select c).FirstOrDefault();
            if (community == null)
                return new List<object>();


            return community.location.Select(l => l.convertToResponse());
        }

        public void addLocation(LocationReq req)
        {
            if (!req.cleanXss())
                throw new ArgumentException();
            try
            {
                var locInfo = new CPS_LocationInfo()
                {
                    InfoContent = req.info.content
                };

                var loc = new CPS_Location()
                {
                    CommunityId=req.cID,
                    SerNum=req.serNum,
                    InfoId=locInfo.Insert(true),
                    Role=req.role,
                    CpSerial=req.cpSerial,
                    beEnable=req.beEnable
                };

                loc.Insert();

            }
            catch (Exception)
            {
                throw new ArgumentException();
            }
        }

        public void editLocation(LocationReq req)
        {
            if (!req.cleanXss())
                throw new ArgumentException();
            var loc = (from l in EkiSQL.ekicps.table<CPS_Location>()
                       where l.CommunityId == req.cID
                       where l.Id == req.id
                       select l).FirstOrDefault();
            if (loc == null)
                throw new ArgumentException();

            loc.CpSerial = req.cpSerial;
            loc.SerNum = req.serNum;
            loc.info.InfoContent = req.info.content;
            loc.Role = req.role;
            loc.beEnable = req.beEnable;

            if (!loc.Update())
                throw new ArgumentException();
        }

        public void removeLocation(LocationReq req)
        {
            var loc = (from l in EkiSQL.ekicps.table<CPS_Location>()
                       where l.CommunityId == req.cID
                       where l.Id == req.id
                       select l).FirstOrDefault();
            if (loc == null)
                throw new ArgumentException();
            if (!loc.Delete())
                throw new ArgumentException();
        }

        public object getCommunityHouse(HouseReq req)
        {

            return (from h in EkiSQL.ekicps.table<CPS_House>()
                    where h.CommunityId == req.cID
                    select h.convertToResponse()).toSafeList();
        }

        public void addCommunityHouse(HouseReq req)
        {
            if (!req.cleanXss())
                throw new ArgumentException();
            var house = req.convertToDbModel();

            //Log.d($"add com house ->{house.toJsonString()}");

            if (house.Insert(true) < 1)
                throw new ArgumentException();

        }

        public void editCommunityHouse(HouseReq req)
        {
            if (!req.cleanXss())
                throw new ArgumentException();
            
            var house = new CPS_House().Also(h => h.CreatById(req.id));

            if (house.Id < 1)
                throw new ArgumentException();

            //去掉沒有包含在資料庫的
            var list = (from s in req.loc
                        join l in EkiSQL.ekicps.table<CPS_Location>() on req.cID equals l.CommunityId into Loc
                        where Loc.Any(l => l.SerNum.Equals(s, StringComparison.Ordinal))
                        select Loc.FirstOrDefault(l=>l.SerNum==s));

            house.Name = req.name;
            house.locList.Clear();
            house.locList.AddRange(list);
            house.beEnable = req.beEnable;

            if (!house.Update())
                throw new ArgumentException();
        }

        public void updateChargeConfig(ChargeConfigReq req)
        {
            var config = new CPS_ChargeConfig().Also(c => c.CreatById(req.id));

            config.Price = req.price;
            config.Unit = req.unit;
            config.beEnable = req.beEnable;

            config.Update();

        }

        public void addChargeConfig(ChargeConfigReq req)
        {
            var nConfig = req.convertToDbModel();
            nConfig.Insert();

        }

        public void removeCommunityHouse(HouseReq req)
        {
            var house = new CPS_House().Also(h => h.CreatById(req.id));

            if (house.Id < 1)
                throw new ArgumentException();

            if (!house.Delete())
                throw new ArgumentException();
        }

        public object getCommunityCp(int cID)
        {
            return (from c in EkiSQL.ekicps.table<CPS_ChargePoint>()
                    where c.CommunityId == cID
                    select c.convertToResponse());
        }

        public bool addCommunityCp(CPS_ChargePoint cp)
        {
            try
            {
                return cp.Insert() > 0;
            }
            catch (Exception e)
            {
                Log.e("add new cp error", e);
            }
            return false;
        }

        public CPS_ChargePoint editCp(ChargePointReq req)
        {
            var cp = new CPS_ChargePoint().Also(c => c.CreatById(req.id));

            if (cp.Id < 1)
                return null;

            cp.Serial = req.serial;
            cp.Remarker = req.remarker;
            cp.beEnable = req.beEnable;

            if (cp.Update())
                return cp;

            return null;
        }

        public void removeCp(CPS_ChargePoint cp)
        {
            cp.Delete();
        }
        #endregion

    }
}