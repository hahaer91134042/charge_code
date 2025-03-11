using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eki_FirestoreDB;

namespace Eki_Web_MVC
{
    /// <summary>
    /// 注意 這邊不要拿去操作 因為是static 只是用來記錄路徑的模板而已
    /// </summary>
    public class Path_Cps
    {
        public static DbPath root = new DbPath(AppConfig.Firestore_Root);
        public static DbPath doc_Community = new DbPath($"{root.raw}/%{Flag.uid}%");
        public static DbPath col_CommunityCp = new DbPath($"{doc_Community.raw}/cp");
        public static DbPath doc_CpSerial = new DbPath($"{col_CommunityCp.raw}/%{Flag.cpSerial}%");//底下有info
        public static DbPath col_CpOrder = new DbPath($"{doc_CpSerial.raw}/order");
        public static DbPath doc_OrderSerial = new DbPath($"{col_CpOrder.raw}/%{Flag.orderSerial}%");

        ////public static DbPath col_CommunityUid = new DbPath( $"{doc_Community.raw}/%{Flag.uid}%");
        ////0=community uid
        //public static DbPath doc_CommunityInfo = new DbPath($"{col_CommunityUid.raw}/info");
        //public static DbPath doc_CommunityCp = new DbPath($"{col_CommunityUid.raw}/cp");
        //public static DbPath col_CpSerial = new DbPath($"{doc_CommunityCp.raw}/%{Flag.cpSerial}%");
        ////0=community uid 1=cp serial
        //public static DbPath doc_CpInfo = new DbPath($"{col_CpSerial.raw}/info");
        //public static DbPath doc_CpOrder = new DbPath($"{col_CpSerial.raw}/order");
        //public static DbPath col_CpOrderSerial = new DbPath($"{doc_CpOrder.raw}/%{Flag.orderSerial}%");
        //public static DbPath doc_OrderInfo = new DbPath($"{col_CpOrderSerial.raw}/info");


        public class Flag
        {
            public const string uid = "uid";
            public const string cpSerial = "cpSerial";
            public const string orderSerial = "oderSerial";
        }
    }
}