using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    public class LoginUser : RequestDAO
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string ValidateCode { get; set; }

        public override bool isValid()
        {
            var acc = cleanXssStr(Account);
            var pwd = cleanXssStr(Password);
            var code = cleanXssStr(ValidateCode);

            return !(acc.isNullOrEmpty() || pwd.isNullOrEmpty() || code.isNullOrEmpty());
        }
    }
}