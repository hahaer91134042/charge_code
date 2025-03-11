using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// IFcmSet 的摘要描述
/// </summary>
namespace Eki_Web_MVC
{
    public interface IFcmSet
    {
        string fcmToken();
        MobilType device();
    }
}
