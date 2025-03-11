using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;
using OCPP_1_6;

namespace Eki_Web_MVC.CPS
{
    [DbTableSet("CPS_CpActionValue")]
    public class CPS_CpActionValue : BaseDbDAO
    {
        [DbRowKey("StartTime",RowAttribute.Time, DbAction.Update)]
        public DateTime StartTime { get; set; }
        [DbRowKey("EndTime",RowAttribute.Time, DbAction.Update)]
        public DateTime EndTime { get; set; }

        [DbRowKey("Action")]
        public int Action { get => cpAction.toInt(); set => cpAction = value.toEnum<CpAction>(); }
        [DbRowKey("MemberId",DbAction.Update)]
        public int MemberId { get; set; } = -1;
        [DbRowKey("CpSerial")]
        public string CpSerial { get; set; }
        [DbRowKey("Auth")]
        public string Auth { get; set; } = "";

        [DbRowKey("StartValue", DbAction.Update)]
        public string StartValue { get => startValue.ToString(); set => startValue = ValueObj.parse(value); }
        [DbRowKey("EndValue", DbAction.Update)]
        public string EndValue { get => endValue.ToString(); set => endValue = ValueObj.parse(value); }

        [DbRowKey("Remarker", DbAction.Update)]
        public string Remarker { get; set; } = "";
        [DbRowKey("cDate", RowAttribute.CreatTime, true)]
        public DateTime cDate { get; set; }
        [DbRowKey("uDate", RowAttribute.NowTime, DbAction.UpdateOnly, true)]
        public DateTime uDate { get; set; }


        public ValueObj startValue = new ValueObj();
        public ValueObj endValue = new ValueObj();
        public CpAction cpAction = CpAction.Charge;

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);
        public override int Insert(bool isReturnId = false) => EkiSQL.ekicps.insert(this, isReturnId);
        public override bool Update() => EkiSQL.ekicps.update(this);

        public static bool checkAuth(string cpSerial, string auth)
            => EkiSQL.ekicps.hasData<CPS_CpActionValue>(
                QueryPair.init("CpSerial", cpSerial)
                .addQuery("Auth", auth));

        public class ValueObj:IBaseDAO
        {
            public double Wh { get; set; }//micro degree
            public double A { get; set; }//電流
            public double V { get; set; }//電壓
            public double W { get; set; }//watt
            internal ValueObj() { }            

            internal static ValueObj parse(SampleValue input)
            {
                return new ValueObj
                {
                    Wh = input.valueBy<double>(SampleValue.Unit.Wh),
                    A = input.valueBy<double>(SampleValue.Unit.A),
                    V = input.valueBy<double>(SampleValue.Unit.V),
                    W = input.valueBy<double>(SampleValue.Unit.W),
                };
            }
            internal static ValueObj parse(string input)
            {
                var data = (from d in input.Split(',')
                            select new
                            {
                                title=d.Split(':')[0],
                                value = d.Split(':')[1]
                            });

                return new ValueObj
                {
                    V=(from d in data
                       where d.title=="V"
                       select d.value.toDouble()).FirstOrDefault(),
                    A = (from d in data
                         where d.title == "A"
                         select d.value.toDouble()).FirstOrDefault(),
                    W = (from d in data
                         where d.title == "W"
                         select d.value.toDouble()).FirstOrDefault(),
                    Wh = (from d in data
                         where d.title == "Wh"
                         select d.value.toDouble()).FirstOrDefault(),
                };
            }            

            public void setValue(SampleValue input)
            {
                Wh = input.valueBy<double>(SampleValue.Unit.Wh);
                A = input.valueBy<double>(SampleValue.Unit.A);
                V = input.valueBy<double>(SampleValue.Unit.V);
                W = input.valueBy<double>(SampleValue.Unit.W);
            }

            public bool isEmpty()
            {
                return Wh <= 0 && A <= 0 && W <= 0 && V <= 0;
            }

            public override string ToString()
            {

                return $"V:{V},A:{A},W:{W},Wh:{Wh}";
            }
        }
    }
}