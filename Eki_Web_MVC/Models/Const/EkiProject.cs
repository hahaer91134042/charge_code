using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    public class EkiProject
    {
        /// <summary>
        /// PPYP專案標記
        /// </summary>
        public static Main Ppyp = new Main("PAY");

        /// <summary>
        /// 共享車位
        /// </summary>
        public static Branch Sharing = new Branch("SR");

        /// <summary>
        /// 充電樁分享(公共停車場)
        /// </summary>
        public static Branch ChargeCode = new Branch("CA");

        /// <summary>
        /// 充電樁系統(社區型)ChargeBolck
        /// </summary>
        public static Branch Community = new Branch("CB");


        public static Tail None_Tail = new Tail("0");


        public static EkiProject PPYP_Share = new EkiProject(Ppyp, Sharing, None_Tail);
        public static EkiProject PPYP_Charg = new EkiProject(Ppyp, ChargeCode, None_Tail);
        public static EkiProject PPYP_CB = new EkiProject(Ppyp, Community, None_Tail);

        public string symbol;
        private EkiProject(Main main,Branch branch,Tail t)
        {
            symbol = main.ToString() + branch.ToString() + t.ToString();
        }

        public override string ToString()
        {
            return symbol;
        }

        /// <summary>
        /// 主專案代碼(3碼)
        /// </summary>
        public class Main
        {
            private string symbol;
            internal Main(string s) => symbol = s;
            public override string ToString() => symbol;
        }

        /// <summary>
        /// 分支代碼(2碼)
        /// </summary>
        public class Branch
        {
            private string symbol;
            internal Branch(string s) => symbol = s;
            public override string ToString() => symbol;
        }

        /// <summary>
        /// 尾碼(1碼)
        /// </summary>
        public class Tail
        {
            private string symbol;
            internal Tail(string s) => symbol = s;
            public override string ToString() => symbol;
        }
    }
}