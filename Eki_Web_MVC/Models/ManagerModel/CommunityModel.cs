using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using Dapper;
using Eki_Web_MVC.CPS;
using DevLibs;


namespace Eki_Web_MVC
{
    public class CommunityModel:IBaseManagerModel
    {
        public CPS_Community community;
        public CommunityModel(CPS_Community com)
        {
            community = com;
        }

        public object meterOrder(DateTime time)
        {
            //var result = (from m in community.meter
            //              select m.CP).SelectMany(cps => (from cp in cps
            //                                              join o in EkiSQL.ekicps.table<CPS_Order>() on cp.Serial equals o.CpSerial into Order
            //                                              select (from o in Order
            //                                                      where o.beEnable
            //                                                      where o.status==OrderStatus.CheckOut
            //                                                      where o.action.StartTime.monthEqual(time)
            //                                                      select o)))
            //              .SelectMany(ords=>ords);

            var result = (from m in community.meter
                          select new
                          {
                              m.Id,
                              m.PaySerial,
                              m.MeterSerial,
                              m.Marker,
                              m.cDate,
                              m.beEnable,
                              Order = (from cp in m.CP
                                       join o in EkiSQL.ekicps.table<CPS_Order>() on cp.Serial equals o.CpSerial into Orders
                                       select (from o in Orders
                                               where o.beEnable
                                               where o.status >= OrderStatus.CheckOut
                                               where time == null ? true : o.action.StartTime.monthEqual(time)
                                               select o.convertToResponse()))
                                             .SelectMany(orders => orders)
                          });

            Log.d($"Meter Order->{result.toJsonString()}");

            return result;
        }

        public object meterCp(List<string> meterSerial)
        {

            return (from ms in meterSerial
                    join meter in community.meter on ms equals meter.MeterSerial
                    select new
                    {
                        Meter = ms,
                        Data = (from c in meter.CP
                                join l in community.location on c.Serial equals l.CpSerial into LOC

                                select new
                                {
                                    Cp = c.convertToResponse(),
                                    Loc = (from loc in LOC
                                           where loc.CpSerial == c.Serial
                                           select loc.convertToResponse()).FirstOrDefault(),
                                    House = (from h in community.house
                                             where LOC.Any(l => h.locList.Any(hl => hl.SerNum == l.SerNum))
                                             select new
                                             {
                                                 h.Id,
                                                 h.Name,
                                                 h.beEnable,
                                                 h.cDate
                                             }).FirstOrDefault()
                                })
                    });
        }








        ////SetSession
        //public void SetSession(string SessionName, object SessionValue)
        //{
        //    HttpContext.Current.Session.Remove(SessionName);
        //    HttpContext.Current.Session[SessionName] = SessionValue;
        //}

        ////GetSession
        //public object GetSession(string SessionName)
        //{
        //    return HttpContext.Current.Session[SessionName];
        //}
    }
}