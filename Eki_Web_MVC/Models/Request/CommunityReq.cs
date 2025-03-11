using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eki_Web_MVC.CPS;

namespace Eki_Web_MVC
{
    public class CommunityReq:RequestDAO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string pwd { get; set; }
        public bool beEnable { get; set; }
        public Address address { get; set; }
        public Bank bank { get; set; }
        public RequestObjList<Meter> meter { get; set; } = new RequestObjList<Meter>();


        public class Address:RequestDAO
        {
            public string code { get; set; }
            public string detail { get; set; }
        }
        public class Bank : RequestDAO
        {
            public string name { get; set; }
            public string serial { get; set; }
            public string account { get; set; }
            public string code { get; set; }
            public string sub { get; set; }
        }
        public class Meter:RequestDAO,IRequestConvert<CPS_ElectricMeter>
        {
            public int id { get; set; } = 0;
            public string paySerial { get; set; }
            public string meterSerial { get; set; }
            public string marker { get; set; }
            public bool beEnable { get; set; } = true;

            public CPS_ElectricMeter convertToDbModel()
                => new CPS_ElectricMeter()
                {
                    Id=id,
                    PaySerial=paySerial,
                    MeterSerial=meterSerial,
                    Marker=marker,
                    beEnable=beEnable
                };
        }

    }
}