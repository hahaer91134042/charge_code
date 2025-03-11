using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    public class LoginCommunity:RequestDAO
    {
        public string ComName { get; set; }
        public string Password { get; set; }
        public string ValidateCode { get; set; }

        public override bool isValid()
        {
            var name = cleanXssStr(ComName);
            var pwd = cleanXssStr(Password);
            var code = cleanXssStr(ValidateCode);

            return !(name.isNullOrEmpty() || pwd.isNullOrEmpty() || code.isNullOrEmpty());
        }

    }
}