using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC
{
    /// <summary>
    /// MemberPayInfo 的摘要描述
    /// </summary>
    [DbTableSet("CPS_MemberPayInfo")]
    public class CPS_MemberPayInfo : BaseDbDAO
    {
        [DbRowKey("MemberId", false)]
        public int MemberId { get; set; }
        [DbRowKey("Neweb", DbAction.Update)]
        public string Neweb { get => neweb.toJsonString(); set => neweb = value.toObj<Info_Neweb>(); }
        [DbRowKey("cDate", RowAttribute.CreatTime, true)]
        public DateTime cDate { get; set; }

        public Info_Neweb neweb = new Info_Neweb();

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);

        public override int Insert(bool isReturnId = false) => EkiSQL.ekicps.insert(this);

        public override bool Update() => EkiSQL.ekicps.update(this);

        public override bool Delete() => EkiSQL.ekicps.delete(this);

        public class Info_Neweb
        {
            /// <summary>
            /// 約定信用卡token
            /// </summary>
            public string Token { get; set; } = "";

            /// <summary>
            /// 約定信用卡token到期日
            /// yyyy-MM-dd
            /// </summary>
            public string TokenLife { get; set; } = "";

            public DateTime expiryDate()
            {
                if (TokenLife.isNullOrEmpty())
                    return DateTime.MinValue;

                return DateTime.ParseExact(TokenLife,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            }

            /// <summary>
            /// 是否正常可用
            /// true=可用
            /// </summary>
            /// <returns></returns>
            public bool useable()
            {
                if (Token.isNullOrEmpty() || TokenLife.isNullOrEmpty())
                    return false;

                return expiryDate() > DateTime.Now;
            }
        }
    }
}
