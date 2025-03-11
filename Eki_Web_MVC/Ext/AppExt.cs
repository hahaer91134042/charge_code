using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;
//using EkiJwt;
using Eki_Web_MVC.CPS;
using EkiSocket;
using System.Threading.Tasks;

namespace Eki_Web_MVC
{
    public static class AppExt
    {
        #region IPhoneMap
        public static string getSmsCode(this IPhoneMap input)
        {
            return input.countryCode().Equals("886") ? input.phone() : $"{input.countryCode()}{input.phone()}";
        }
        #endregion
        #region Session
        public static void saveSession(this ISession obj) => obj.saveSession(obj.sessionFlag());
        public static void saveSession(this ISession obj, string flag)
        {
            HttpContext.Current.Session.Remove(flag);
            HttpContext.Current.Session[flag] = obj;
        }
        public static T getSession<T>(this ISession obj) where T : ISession
            => obj.getSession<T>(obj.sessionFlag());
        public static T getSession<T>(this ISession obj, string flag) where T : ISession
        {
            try
            {
                return (T)HttpContext.Current.Session[flag];
            }
            catch (Exception)
            {

            }
            return default(T);
        }
        #endregion
        #region IEncodeSet
        public static string creatHash(this IEncodeSet input, string publicKey)
            => input.creatHash(publicKey, input.hashSet().secret());
        public static string creatHash(this IEncodeSet input, params string[] array)
            => input.hashSet().creatHash(array.Count() > 0 ? array : new string[] { input.hashSet().secret() });
        public static string encryptByAES(this IEncodeSet input, string hashText = null)
        {
            if (hashText == null)
                hashText = input.creatHash();
            return SecurityBuilder.EncryptTextByAES(input.toJsonString(), hashText);
        }
        public static T decryptByAES<T>(this string input, string hashText = null) where T : IEncodeSet
        {
            if (hashText == null)
                hashText = Activator.CreateInstance<T>().creatHash();
            return SecurityBuilder.DecryptTextByAES(input, hashText).toObj<T>();
        }
        #endregion
        #region IHashFormat
        public static string creatHash(this IHashFormat input, params string[] valus) =>
            SecurityBuilder.CreateHashCode(input.format(), valus);
        #endregion
        #region IToken
        //public static string token(this IToken input)
        //    => JwtBuilder.GetEncoder()
        //            .setUser(input.tokenRaw())
        //            .setExpDate(DateTime.Now.AddDays(JwtConfig.TokenLifeDay))
        //            .encode();
        #endregion
        #region ICryptoPwd
        public static void creatCipher(this ICryptoPwd input, string rawText)
        {
            var salt = input.newSalt();
            var cipher = input.creatHash(salt, rawText);
            input.setSalt(salt);
            input.setCipher(cipher);
        }
        public static bool checkCipher(this ICryptoPwd input, string rawText)
            => input.creatHash(input.salt(), rawText).Equals(input.cipher());
        #endregion
        #region JwtAuthObject
        //public static CPS_Member getMember(this JwtAuthObject obj) => new CPS_Member().Also(m => m.CreatByUniqueId(obj.user));
        #endregion
        #region Guid
        public static bool equal(this Guid uid, string id) => uid.ToString().Equals(id, StringComparison.OrdinalIgnoreCase);
        public static bool equal(this string id, Guid uid) => uid.equal(id);
        #endregion
        #region BroadCastMsg    
        public static void sendTo(this IBroadCastMsg msg, IPushSet set)
        {
            var socket = BroadcastSocket.Instance;
            using (var fcmManager = FcmManager.New())
            {
                try
                {
                    if (!socket.SendTo(set.socketID(), msg))
                    {
                        fcmManager.SendTo(set.fcmToken(), set.device(), msg);
                    }
                }
                catch (Exception e)
                {
                    Log.e($"SendBroadCast msg->{msg}", e);
                }
            }
        }
        public static void sendTo(this IBroadCastMsg msg, IEnumerable<IPushSet> list)
        {
            //有socket先送 沒有再送Fcm 
            var socket = BroadcastSocket.Instance;
            using (var fcmManager = FcmManager.New())
            {
                var socketMsg = msg.toSocketMsg();
                foreach (var set in list)
                {
                    try
                    {
                        if (!socket.SendTo(set.socketID(), socketMsg.toJsonString()))
                        {
                            fcmManager.SendTo(set.fcmToken(), set.device(), msg);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.e("SendBroadCast", e);
                    }
                }
            }
        }
        #endregion
        #region BaseProcess
        public static Task runAsync(this BaseProcess process)
            => Task.Run(() => process.run());
        #endregion
    }
}