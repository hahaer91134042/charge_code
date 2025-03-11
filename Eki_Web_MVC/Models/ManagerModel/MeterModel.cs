using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;
using Eki_Web_MVC.CPS;

namespace Eki_Web_MVC
{
    public class MeterModel : IBaseManagerModel
    {
        public CPS_ElectricMeter getMeterDetail(string serial)
        {
            var result = (from m in EkiSQL.ekicps.table<CPS_ElectricMeter>()
                          where m.PaySerial==serial
                          select m).FirstOrDefault();
            //if (result.Count() < 1)
            //    return null;

            return result;
        }

        public List<object> getMeterCP(ChargePointReq req)
        {
            var meter = new CPS_ElectricMeter().Also(m => m.CreatById(req.mID));
            return meter.CP.convertResponseList<object>();
        }

        public void addMeterCP(ChargePointReq req)
        {
            try
            {
                var cp = req.convertToDbModel();
                cp.CommunityId = new CPS_ElectricMeter().Also(m => m.CreatById(cp.MeterId)).CommunityId;
                cp.Insert();
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }
        }

        public void removeMeterCP(ChargePointReq req)
        {
            var cp = (from c in EkiSQL.ekicps.table<CPS_ChargePoint>()
                      where c.Id == req.id
                      select c).FirstOrDefault();
            if (cp == null)
                throw new ArgumentException();
            if (!cp.Delete())
                throw new ArgumentException();
        }

        public void editMeterCP(ChargePointReq req)
        {
            var cp = (from c in EkiSQL.ekicps.table<CPS_ChargePoint>()
                      where c.Id == req.id
                      select c).FirstOrDefault();
            if (cp == null)
                throw new ArgumentException();

            cp.Remarker = req.remarker;
            cp.Serial = req.serial;
            cp.beEnable = req.beEnable;

            if (!cp.Update())
                throw new ArgumentException();
        }

        public object getCpAuth(string cpSerial)
        {
            return (from a in EkiSQL.ekicps.table<CPS_Auth>()
                    where a.CpSerial.Equals(cpSerial,StringComparison.Ordinal)
                    select a.convertToResponse());
        }

        public void addCpAuth(CpAuthReq req)
        {
            if (!req.cleanXss())
                throw new ArgumentException();

            var auth = req.convertToDbModel();

            if (auth.Insert(true) < 1)
                throw new ArgumentException();
        }

        public void removeAuth(CpAuthReq req)
        {
            var auth = (from a in EkiSQL.ekicps.table<CPS_Auth>()
                        where a.Id==req.id
                        select a).FirstOrDefault();
            if (auth == null)
                throw new ArgumentException();

            if (!auth.Delete())
                throw new ArgumentException();
        }

        public void editAuth(CpAuthReq req)
        {
            if (!req.cleanXss())
                throw new ArgumentException();

            var auth = (from a in EkiSQL.ekicps.table<CPS_Auth>()
                        where a.Id == req.id
                        select a).FirstOrDefault();
            if (auth == null)
                throw new ArgumentException();

            auth.CpSerial = req.cpSerial;
            auth.Auth = req.auth;
            auth.beEnable = req.beEnable;

            if (!auth.Update())
                throw new ArgumentException();
        }
    }
}