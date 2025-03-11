using DevLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    public class EkiCpsDbConfig : IDbConfig
    {
        public string connetString() => Properties.Settings.Default.cpsDB;
    }
}