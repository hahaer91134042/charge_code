using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eki_FirestoreDB;

namespace Eki_Web_MVC
{
    public class ColFrag_CpOrder:IColFrag
    {
        public DbPath colPath;
        public ColFrag_CpOrder(string uid,string cpSerial)
        {
            colPath = new DbPath(Path_Cps.col_CpOrder);
            colPath.AddMap(Path_Cps.Flag.uid, uid);
            colPath.AddMap(Path_Cps.Flag.cpSerial, cpSerial);
        }

        public override string path => colPath.ToString();
    }
}