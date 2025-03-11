using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    public class UserReq:RequestDAO
    {
        public int id { get; set; }
        public string acc { get; set; }
        public string pwd { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public int lv { get; set; } = ManagerLv.None.toInt();
        public bool beEnable { get; set; } = true;

        public override bool cleanXss()
        {
            try
            {
                acc = cleanXssStr(acc);
                pwd = cleanXssStr(pwd);
                name = cleanXssStr(name);
                phone = cleanXssStr(phone);
                email = cleanXssStr(email);

                return true;
            }
            catch (Exception)
            {

            }
            return false;
        }

    }
}