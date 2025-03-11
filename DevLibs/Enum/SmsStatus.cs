using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevLibs
{
    public class SmsStatus
    {
        public static SmsStatus Success = new SmsStatus("00000");
        public static SmsStatus PhoneError = new SmsStatus("00100");

        public string value;
        public SmsStatus(string c)
        {
            this.value = c;
        }
    }
}