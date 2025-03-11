using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;
using Eki_Web_MVC.CPS;

namespace Eki_Web_MVC
{
    public class HouseReq : RequestDAO, IRequestConvert<CPS_House>
    {
        public int cID { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public List<string> loc { get; set; } = new List<string>();
        public bool beEnable { get; set; }

        public override bool cleanXss()
        {
            try
            {
                name = cleanXssStr(name);
                loc = (from s in loc
                       select cleanXssStr(s)).toSafeList();

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public CPS_House convertToDbModel()
        {
            var h = new CPS_House()
            {
                CommunityId = cID,
                Name = name,
                beEnable=beEnable
            };
            //去掉沒有包含在資料庫的
            var list = (from s in loc
                        join l in EkiSQL.ekicps.table<CPS_Location>() on cID equals l.CommunityId into Loc
                        where Loc.Any(l => l.SerNum.Equals(s, StringComparison.Ordinal))
                        select Loc.FirstOrDefault(l=>l.SerNum==s));
            //Log.d($"house convert list->{list.toJsonString()}");
            h.locList.AddRange(list);
            //h.locList.AddRange((from loc in EkiSQL.ekicps.table<CPS_Location>()
            //                    where loc.CommunityId==cID
            //                    join s in loc on loc.SerNum equals s
            //                    select loc.SerNum));
            return h;
        }
    }
}