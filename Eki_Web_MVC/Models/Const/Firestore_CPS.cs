using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Eki_FirestoreDB;
using Eki_Web_MVC.CPS;

namespace Eki_Web_MVC
{
    public class Firestore_CPS : IFirestoreConnect
    {
        private Firestore_CPS() { }
        public static EkiFirestore db
        {
            get
            {
                if (_db == null)
                    start();
                return _db;
            }
        }
        private static EkiFirestore _db;
        public static void start()
        {
            _db = EkiFirestore.start(new Firestore_CPS());
        }


        public override string projectId => "cps-firebase-5abf9";

        public override string jsonCredit
        {
            get
            {
                var bytes = Properties.Resources.cps_firebase_5abf9;
                return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            }
        }

        public override string root => "cps";


        #region Charge point db
        public class Cp
        {
            private static Dictionary<CPS_ChargePoint, DocFrag_CpSerial> cpDic = new Dictionary<CPS_ChargePoint,DocFrag_CpSerial>();

            public static bool addCpStore(string cpSerial)
            {
                try
                {
                    if (cpDic.Any(p => p.Key.Serial == cpSerial))
                        return false;

                    var cp = CPS_ChargePoint.CreatBySerial(cpSerial);
                    if (cp == null)
                        return false;


                    var community = CPS_ChargePoint.findCommunity(cpSerial);
                    if (community == null)
                        return false;
                    //var cpDoc = new DocFrag_CpInfo(community.Uid.ToString(), cpSerial);

                    var cpDoc = db.findDoc<DocFrag_CpSerial>(community.Uid.ToString(), cpSerial);

                    cpDic.Add(cp, cpDoc);
                    var log = (from p in cpDic
                               select new
                               {
                                   Cp=p.Key.Serial,
                                   Doc=p.Value.path
                               });
                    Log.d($"cp store list->{log.toJsonString()}");
                    return true;
                }
                catch (Exception e)
                {
                    Log.d("Firestore_CP addCpStore error", e);
                }
                return false;
            }

            //public static ColFrag_CommunityCp cpCol(string cpSerial)
            //{
            //    return doc(cpSerial)?.parent();
            //}

            public static DocFrag_CpSerial doc(string cpSerial)
            {

                return (from p in cpDic
                        where p.Key.Serial == cpSerial
                        select p.Value).FirstOrDefault();
            }

        }
        #endregion

    }
}