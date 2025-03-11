using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;
using Eki_Web_MVC;

namespace Eki_Web_MVC.CPS
{
    [DbTableSet("CPS_ChargeConfig")]
    public class CPS_ChargeConfig : BaseDbDAO, IPayCost.CurrencySet,IConvertResponse<object>
    {
        [DbRowKey("LocationId",false)]
        public int LocationId { get; set; }

        /// <summary>
        /// 每小時費率(元/小時)
        /// </summary>
        [DbRowKey("Price", DbAction.Update)]
        public double Price { get; set; } = 0.0;

        /// <summary>
        /// 貨幣計價單位
        /// </summary>
        [DbRowKey("Unit",DbAction.Update)]
        public int Unit { get => unit.toInt(); set => unit = value.toEnum<CurrencyUnit>(); }
        [DbRowKey("beEnable", DbAction.Update)]
        public bool beEnable { get; set; } = true;
        [DbRowKey("cDate", RowAttribute.CreatTime, true)]
        public DateTime cDate { get; set; }
        [DbRowKey("uDate", RowAttribute.NowTime, DbAction.Update, true)]
        public DateTime uDate { get; set; }



        public CurrencyUnit unit = CurrencyUnit.TWD;

        public CurrencyUnit costUnit() => unit;
        public double rawCost() => Price;

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);

        public override int Insert(bool isReturnId = false) => EkiSQL.ekicps.insert(this, isReturnId);
        public override bool Update() => EkiSQL.ekicps.update(this);

        public object convertToResponse()
            => new
            {
                Id,
                LocationId,
                Price,
                Unit,
                beEnable
            };
    }
}