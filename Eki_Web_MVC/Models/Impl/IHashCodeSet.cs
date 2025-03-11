using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// ICrypto 的摘要描述
/// </summary>
namespace Eki_Web_MVC
{
    public interface IHashCodeSet : IHashFormat
    {
        // EncryptFormat format();
        string secret();//密鑰
    }
}
