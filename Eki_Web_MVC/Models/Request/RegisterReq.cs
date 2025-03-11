using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    public class RegisterReq:RequestDAO
    {
        public string phone { get; set; }
        public string mail { get; set; }
        public string pwd { get; set; }


    }
}