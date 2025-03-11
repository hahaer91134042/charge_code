using DevLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

/// <summary>
/// EkiEncoder 的摘要描述
/// </summary>
namespace Eki_Web_MVC
{
    public class EkiEncoder
    {
        public static IEncoder AES = new EkiAESencoder();
        public static AdminUserEncoder AdminUser = new AdminUserEncoder();
        public static IEncoder Data = new EkiDataEncoder();

        private class EkiDataEncoder : IEncoder
        {
            public override T decode<T>(string cipher, string key = "")
            {
                var obj = Activator.CreateInstance<T>();
                var hash = obj.creatHash();

                using (var md5 = new MD5CryptoServiceProvider())
                using (var aes = new AesCryptoServiceProvider())
                {
                    aes.Key = md5.ComputeHash(Encoding.ASCII.GetBytes(hash.Substring(0, 16)));
                    aes.IV = md5.ComputeHash(Encoding.ASCII.GetBytes(hash.Substring(8, 24)));
                    aes.Mode = CipherMode.CBC;

                    return SecurityBuilder.DecryptTextFromBase64(cipher, aes.CreateDecryptor())
                        .toObj<T>();
                }
            }

            public override CryptoContent encode<T>(T data)
            {
                var hash = data.creatHash();

                using (var md5 = new MD5CryptoServiceProvider())
                using (var aes = new AesCryptoServiceProvider())
                {
                    aes.Key = md5.ComputeHash(Encoding.ASCII.GetBytes(hash.Substring(0, 16)));
                    aes.IV = md5.ComputeHash(Encoding.ASCII.GetBytes(hash.Substring(8, 24)));
                    aes.Mode = CipherMode.CBC;

                    return new CryptoContent
                    {
                        cipher = SecurityBuilder.EncryptTextToBase64(data.toJsonString(), aes.CreateEncryptor())
                    };
                }
            }
        }

        private class EkiAESencoder : IEncoder
        {
            public override T decode<T>(string cipher, string key = "")
            {
                var obj = Activator.CreateInstance<T>();
                var hash = obj.creatHash(key);
                return cipher.decryptByAES<T>(hash);
            }

            public override CryptoContent encode<T>(T data)
            {
                var key = SecurityBuilder.CreateSaltKey();
                var hash = data.creatHash(key);
                return new CryptoContent()
                {
                    publicKey = key,
                    cipher = data.encryptByAES(hash)
                };
            }
        }

        public class AdminUserEncoder : IEncoder.Type<AdminPwd>
        {
            public override AdminPwd decode(string cipher, string key = "")
            {
                var encode = new AdminPwd();
                var cipherText = cipher.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encode.hashSet().secret(), new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        encode.pwd = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }

                return encode;
            }

            public override CryptoContent encode(AdminPwd data)
            {
                var content = new CryptoContent();
                byte[] clearBytes = Encoding.Unicode.GetBytes(data.pwd);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(data.hashSet().secret(), new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        content.cipher = Convert.ToBase64String(ms.ToArray());
                    }
                }

                return content;
            }

        }
    }
}
