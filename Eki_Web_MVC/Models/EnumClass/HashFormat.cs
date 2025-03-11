using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// EncryptEnum 的摘要描述
/// </summary>
namespace Eki_Web_MVC
{
    public enum HashFormat
    {
        NONE,//不使用Hash

        SHA1,
        SHA256,
        SHA384,
        SHA512
    }
}
