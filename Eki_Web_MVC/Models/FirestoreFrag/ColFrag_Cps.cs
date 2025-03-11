using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eki_FirestoreDB;

namespace Eki_Web_MVC
{
    public class ColFrag_Cps : IColFrag
    {
        public override string path => Path_Cps.root.ToString();
    }
}