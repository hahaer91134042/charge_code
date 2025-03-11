using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    public class AdminPwd:IBaseDAO,IEncodeSet
    {
        public string pwd { get; set; }
        public IHashCodeSet hashSet() => EkiHashCode.AdminUser;
    }
}