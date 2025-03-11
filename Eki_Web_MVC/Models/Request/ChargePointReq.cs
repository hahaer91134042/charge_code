using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eki_Web_MVC.CPS;

namespace Eki_Web_MVC
{
    public class ChargePointReq:RequestDAO,IRequestConvert<CPS_ChargePoint>
    {
        public int cID { get; set; } = 0;//社區ID
        public int mID { get; set; } = 0;//電表的ID
        public int id { get; set; }
        public string remarker { get; set; }
        public string serial { get; set; }
        public bool beEnable { get; set; }

        public string uid { get; set; }

        public CPS_ChargePoint convertToDbModel()
            => new CPS_ChargePoint()
            {
                CommunityId = cID,
                MeterId = mID,
                Remarker = remarker,
                Serial = serial,
                beEnable = beEnable
            };
    }
}