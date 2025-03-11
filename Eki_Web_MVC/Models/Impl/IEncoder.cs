using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// ISecret 的摘要描述
/// </summary>
namespace Eki_Web_MVC
{
    public abstract class IEncoder
    {
        public abstract T decode<T>(string cipher, string key = "") where T : IEncodeSet;
        public abstract CryptoContent encode<T>(T data) where T : IEncodeSet;

        public class CryptoContent
        {
            public string publicKey { get; set; }
            public string cipher { get; set; }
        }

        public abstract class Type<E>:IEncoder where E : IEncodeSet
        {
            public override T decode<T>(string cipher, string key = "")
            {
                return default(T);
            }
            public override CryptoContent encode<T>(T data)
            {
                return null;
            }
            public abstract new E decode(string cipher, string key = "");
            public abstract new CryptoContent encode(E data);
        }
    }
}
