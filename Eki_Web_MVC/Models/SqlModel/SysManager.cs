using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC
{
    [DbTableSet("SysManager")]
    public class SysManager : BaseDbDAO,ISession,IConvertResponse<object>
    {
        [DbRowKey("GroupId",DbAction.Update)]
        public int GroupId { get; set; }
        [DbRowKey("Name",DbAction.Update)]
        public string Name { get; set; }
        [DbRowKey("Email",DbAction.Update)]
        public string Email { get; set; }
        [DbRowKey("Phone",DbAction.Update)]
        public string Phone { get; set; }
        [DbRowKey("Uid",RowAttribute.Guid,true)]
        public Guid Uid { get; set; }
        [DbRowKey("beEnable", DbAction.Update)]
        public bool beEnable { get; set; } = true;
        [DbRowKey("Account")]
        public string Account { get; set; }
        [DbRowKey("Pwd",DbAction.Update)]
        public string Pwd { get; set; }
        [DbRowKey("cDate", RowAttribute.CreatTime, true)]
        public DateTime cDate { get; set; }
        [DbRowKey("uDate", RowAttribute.NowTime, DbAction.UpdateOnly, true)]
        public DateTime uDate { get; set; }
        [DbRowKey("Ip",DbAction.Update)]
        public string Ip { get; set; }

        public ManagerLv Lv { get => Group == null ? ManagerLv.None : Group.Lv.toEnum<ManagerLv>(); }

        public string decodePwd 
        {
            get => EkiEncoder.AdminUser.decode(Pwd).pwd;
        }

        public DateTime LoginTime;

        public SysGroup Group 
        { 
            get {
                if (group == null)
                    group = (from g in EkiSQL.ekicps.table<SysGroup>()
                             where g.Id==GroupId
                             where g.beEnable
                             select g).FirstOrDefault();
                return group;
            } 
        }

        private SysGroup group = null;

        public override bool CreatById(int id)
            => EkiSQL.ekicps.loadDataById(id, this);

        public override int Insert(bool isReturnId = false)
            => EkiSQL.ekicps.insert(this, isReturnId);

        public override bool Update()
        {
            return EkiSQL.ekicps.update(this);
        }

        public static SysManager fromSession() => new SysManager().getSession<SysManager>();
        public static string creatPwdCipher(string text)
        {
            return EkiEncoder.AdminUser.encode(new AdminPwd { pwd = text }).cipher;
        }
        public static SysManager creat(string acc,string pwd)
        {
            //var ciper = EncryptPwd(pwd);
            var ciper = creatPwdCipher(pwd);
            //Log.d($"SysManager Cipher->{ciper}");

            return (from m in EkiSQL.ekicps.table<SysManager>()
                    where m.Account==acc
                    where m.Pwd==ciper
                    select m).FirstOrDefault();
        }

        public string sessionFlag() => AppFlag.AdminUser;

        public object convertToResponse()
            => new
            {
                Id,
                Name,
                Email,
                Account,
                Pwd = decodePwd,
                Phone,
                beEnable,
                cDate,
                Lv,
                Ip,
                Group
            };

        //編碼
        //public static string EncryptPwd(string clearText)
        //{
        //    byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(AppConfig.AdminUserSecret, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(clearBytes, 0, clearBytes.Length);
        //                cs.Close();
        //            }
        //            clearText = Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //    return clearText;
        //}

        //解碼
        //public static string DecryptPwd(string cipherText)
        //{
        //    cipherText = cipherText.Replace(" ", "+");
        //    byte[] cipherBytes = Convert.FromBase64String(cipherText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(AppConfig.AdminUserSecret, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(cipherBytes, 0, cipherBytes.Length);
        //                cs.Close();
        //            }
        //            cipherText = Encoding.Unicode.GetString(ms.ToArray());
        //        }
        //    }
        //    return cipherText;
        //}

        public interface IUserCheck
        {
            string acc();
            string pwd();
        }
    }
}