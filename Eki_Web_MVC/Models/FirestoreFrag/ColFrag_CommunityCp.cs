using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eki_FirestoreDB;

namespace Eki_Web_MVC
{
    public class ColFrag_CommunityCp : IColFrag
    {
        public string uid;
        public DbPath colPath;
        public ColFrag_CommunityCp(string id)
        {
            uid = id;
            colPath = new DbPath(Path_Cps.col_CommunityCp);
            colPath.AddMap(Path_Cps.Flag.uid, uid);
        }
        public override string path => colPath.ToString();
    }
}