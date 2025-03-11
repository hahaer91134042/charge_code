using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eki_FirestoreDB;
using OCPP_1_6;

namespace Eki_Web_MVC
{
    public class DocFrag_CpSerial : IDocFrag
    {
        public DbPath docPath;
        public DocFrag_CpSerial(string uid,string cpSerial)
        {
            docPath = new DbPath(Path_Cps.doc_CpSerial);
            docPath.AddMap(Path_Cps.Flag.uid, uid);
            docPath.AddMap(Path_Cps.Flag.cpSerial, cpSerial);
        }

        public override string path => docPath.ToString();

        public ColFrag_CommunityCp parent()
        {
            return new ColFrag_CommunityCp(docPath.First(p=>p.key==Path_Cps.Flag.uid).value);
        }

        [DocValue("status")]
        public string status { get; set; } = OCPP_Status.CP.Unavailable.ToString();

        public DocFrag_OrderSerial orderDoc(string serial)
        {
            var uid = docPath.First(p => p.key == Path_Cps.Flag.uid);
            var cp = docPath.First(p => p.key == Path_Cps.Flag.cpSerial);

            return new DocFrag_OrderSerial(
                uid.value,
                cp.value,
                serial);
        }

    }
}