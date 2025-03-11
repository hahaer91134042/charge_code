using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevLibs
{
    public class EkiSms
    {
        private const string acc = "ekiwebdesign123";
        private const string pwd = "qaz123wsx";
        private const string templet = "http://api.twsms.com/json/sms_send.php?username={0}&password={1}&mobile={2}&message={3}";

        private EkiSms() { }
        public static SmsBuilder builder(string mobile) => new SmsBuilder(mobile);


        public class SmsBuilder
        {
            public string mobile = "";
            public string msg = "";

            internal SmsBuilder(string m) { mobile = m; }

            public SmsBuilder setMsg(string msg)
            {
                this.msg = msg;
                return this;
            }

            public SmsResult send()
            {
                var url = string.Format(templet, acc, pwd, mobile, HttpUtility.UrlEncode(msg));

                return WebConnect.CreatGet(url).Connect<SmsResult>();
            }
        }

        public class SmsResult
        {
            public string code { get; set; }
            public string text { get; set; }
            public int msgid { get; set; }
        }
    }
}