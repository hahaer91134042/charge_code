using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// IPhoneMap 的摘要描述
/// </summary>
namespace Eki_Web_MVC
{
    public interface IPhoneMap
    {
        string countryCode();
        string phone();
    }
}