using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    public static class AppConfig
    {
        
        public const string DateFormat = "yyyy-MM-dd";
        public const string TimeFormat = "HH:mm:ss";
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public const string AdminUserSecret = "abc123";
        public const string PrivateSecret = "!qaz2WSX#edc";//私鑰

        //public const string CPSFlag = "CPS";


        public const double DefaultRate = 8;//預設的每度電費率

        public const string PrivateKey_Data = "Eki1qaz@WSX123";
        
        public const string Firestore_Root = "cps_test";//測試
        //public const string Firestore_Root = "cps";//正式

        public static class Url
        {
            //測試站
            public const string CPS_Api = "http://ccodeapi.eki.com.tw";
            //測試站
            public const string CPS_Web = "http://chargecode.eki.com.tw";
        }


    }
}