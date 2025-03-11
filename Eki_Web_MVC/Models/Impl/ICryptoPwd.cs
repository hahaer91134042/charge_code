using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// ICryptoPwd 的摘要描述
/// </summary>
namespace Eki_Web_MVC
{
    public interface ICryptoPwd : IHashFormat
    {
        string newSalt();
        void setSalt(string salt);
        void setCipher(string cipher);
        string salt();
        string cipher();
    }
}
