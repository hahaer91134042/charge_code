using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// EkiBroadCastMethod 的摘要描述
/// </summary>
namespace Eki_Web_MVC
{
    public class EkiBroadCastMethod
    {
        public static EkiBroadCastMethod SocketEvent = new EkiBroadCastMethod("SocketEvent");

        public string Name;
        public EkiBroadCastMethod(string n)
        {
            Name = n;
        }
    }
}
