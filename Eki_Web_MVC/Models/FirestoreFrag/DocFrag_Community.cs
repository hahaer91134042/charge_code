using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eki_FirestoreDB;

namespace Eki_Web_MVC
{
    public class DocFrag_Community : IDocFrag    
    {
        public string uid;
        public DbPath docPath;
        public DocFrag_Community(string id)
        {
            uid = id;
            docPath = new DbPath(Path_Cps.doc_Community);
            docPath.AddMap(Path_Cps.Flag.uid, uid);
        }

        public override string path => docPath.ToString();
    }
}