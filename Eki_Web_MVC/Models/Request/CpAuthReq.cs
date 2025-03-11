using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eki_Web_MVC.CPS;

namespace Eki_Web_MVC
{
    public class CpAuthReq:RequestDAO,IRequestConvert<CPS_Auth>
    {
        public int id { get; set; }
        public string cpSerial { get; set; }
        public string auth { get; set; }
        public bool beEnable { get; set; }

        public CPS_Auth convertToDbModel()
            => new CPS_Auth()
            {
                CpSerial=cpSerial,
                Auth=auth,
                beEnable=beEnable
            };

        public override bool cleanXss()
        {
            try
            {
                cpSerial = cleanXssStr(cpSerial);
                auth = cleanXssStr(auth);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
    }
}