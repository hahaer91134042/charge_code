using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// EkiCrypto 的摘要描述
/// </summary>
namespace Eki_Web_MVC
{
    public class EkiHashCode
    {
        public static IHashCodeSet SHA1 = new EkiHash_SHA1();
        public static IHashCodeSet AdminUser = new EkiHash_AdminUser();
        public static IHashCodeSet Data = new EkiHash_Data();

        private class EkiHash_Data : IHashCodeSet
        {
            public HashFormat format() => HashFormat.SHA256;

            public string secret() => AppConfig.PrivateKey_Data;
        }

        private class EkiHash_SHA1 : IHashCodeSet
        {
            public HashFormat format() => HashFormat.SHA1;
            public string secret() => AppConfig.PrivateSecret;
        }

        private class EkiHash_AdminUser : IHashCodeSet
        {
            public HashFormat format() => HashFormat.NONE;

            public string secret() => AppConfig.AdminUserSecret;
        }
    }
}
