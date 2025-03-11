using DevLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    public static class EkiSQL
    {
        public static CpsSql ekicps = new CpsSql();


        /// <summary>
        /// 社區能源系統 
        /// </summary>
        public class CpsSql : ISql
        {
            public IDbConfig dbConfig() => new EkiCpsDbConfig();

            public int timeOut() => 10000;
        }
    }
}
