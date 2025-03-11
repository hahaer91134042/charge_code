using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    public class LocationReq:RequestDAO
    {
        public int cID { get; set; }//社區的ID
        public int id { get; set; }//該地點的Id
        public string serNum { get; set; }//地點編號
        public Info info { get; set; }
        public int role { get; set; }
        public string cpSerial { get; set; } = "";
        public bool beEnable { get; set; } = true;
        


        public class Info : RequestDAO
        {
            public string content { get; set; } = "";
        }

        public override bool cleanXss()
        {
            try
            {
                cpSerial = cleanXssStr(cpSerial);
                serNum = cleanXssStr(serNum);
                info.content = cleanXssStr(info.content);

                return true;
            }
            catch (Exception)
            {

            }
            return false;
        }
    }
}