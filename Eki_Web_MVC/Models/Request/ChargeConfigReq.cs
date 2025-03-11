using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eki_Web_MVC.CPS;

namespace Eki_Web_MVC
{
    public class ChargeConfigReq:RequestDAO,IRequestConvert<CPS_ChargeConfig>
    {
        public int id { get; set; }
        public int locationId { get; set; }
        public double price { get; set; }
        public int unit { get; set; }
        public bool beEnable { get; set; }

        public CPS_ChargeConfig convertToDbModel()
            => new CPS_ChargeConfig()
            {
                Id=id,
                LocationId=locationId,
                Price=price,
                Unit=unit,
                beEnable=beEnable
            };
    }
}