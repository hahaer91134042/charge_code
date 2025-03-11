using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    public class MenuReq:RequestDAO
    {
        public int id { get; set; }
        public string name { get; set; }
        public int parentId { get; set; }

        public int groupLv { get; set; }
        public string path { get; set; } = "";
        public int sort { get; set; } = 0;
        public int type { get; set; } = 0;
        public bool beEnable { get; set; } = true;

        public override bool cleanXss()
        {
            try
            {
                name = cleanXssStr(name);
                path = cleanXssStr(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}