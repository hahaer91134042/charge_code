using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eki_FirestoreDB;
using Google.Cloud.Firestore;
using Eki_Web_MVC.CPS;
using System.Collections;

namespace Eki_Web_MVC
{
    public class DocFrag_OrderSerial : IDocFrag
    {
        public DbPath docPath;
        public DocFrag_OrderSerial(string uid,string cpSerial,string orderSerial)
        {
            docPath = new DbPath(Path_Cps.doc_OrderSerial);
            docPath.AddMap(Path_Cps.Flag.uid, uid);
            docPath.AddMap(Path_Cps.Flag.cpSerial,cpSerial);
            docPath.AddMap(Path_Cps.Flag.orderSerial, orderSerial);
        }

        public override string path => docPath.ToString();


        public void save(CPS_Order.Response order)
        {
            var data = Data.creat(order);

            save(data, SaveMode.Overwrite);
        }



        [FirestoreData]
        public class Data
        {
            public static Data creat(CPS_Order.Response resp)
            {
                return new Data
                {
                    Serial = resp.Serial,
                    Status = resp.Status,
                    Cost = resp.Cost,
                    Rate = resp.Rate,
                    Unit = resp.Unit,
                    Member = resp.Member.copyTo<OrderMember>(),
                    Card4No = resp.Card4No,
                    cDate = resp.cDate.toDateTime().toUtcStamp(),
                    Loc = new Location()
                    {
                        Serial = resp.Loc.Serial,
                        Content = resp.Loc.Content
                    },
                    Time = new TimeRange
                    {
                        Start = resp.Time.Start.toDateTime().toUtcStamp(),
                        End = resp.Time.End.toDateTime().toUtcStamp()
                    },
                    Value = new CpValue
                    {
                        Start = resp.Value.Start,
                        End = resp.Value.End,
                        Unit = resp.Value.Unit
                    }
                };
            }

            [FirestoreProperty]
            public string Serial { get; set; }
            [FirestoreProperty]
            public int Status { get; set; }
            [FirestoreProperty]
            public double Cost { get; set; }
            [FirestoreProperty]
            public double Rate { get; set; }
            [FirestoreProperty]
            public int Unit { get; set; }
            [FirestoreProperty]
            public OrderMember Member { get; set; }
            [FirestoreProperty]
            public string Card4No { get; set; }
            [FirestoreProperty]
            public Timestamp cDate { get; set; }
            [FirestoreProperty]
            public Location Loc { get; set; }
            [FirestoreProperty]
            public TimeRange Time { get; set; }
            [FirestoreProperty]
            public CpValue Value { get; set; }

            public class OrderMember : IFirestoreData_Map
            {
                public string Phone { set => this["Phone"] = value; }
                public string Name { set => this["Name"] = value; }
            }

            public class Location : IFirestoreData_Map
            {
                public string Serial { set => this["Serial"] = value; }
                public string Content { set => this["Content"] = value; }
            }
            public class TimeRange: IFirestoreData_Map
            {
                public Timestamp Start { set => this["Start"] = value; }
                public Timestamp End { set => this["End"] = value; }
            }
            public class CpValue : IFirestoreData_Map
            {
                public double Start { set => this["Start"] = value; }
                public double End { set => this["End"] = value; }
                public string Unit { set => this["Unit"] = value; }
            }
        }
    }
}