using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevLibs;

namespace Eki_Web_MVC.CPS
{
    [DbTableSet("CPS_ElectricityRate")]
    public class CPS_ElectricityRate : BaseDbDAO
    {
        [DbRowKey("Year",DbAction.Update)]
        public int Year { get; set; }//year =-1 表示 是整個系統的預設
        [DbRowKey("LowRate",DbAction.Update)]
        public string LowRate { get => lowRate.ToString(); set => lowRate = value.toMapObj<LowRateValue>(); }
        [DbRowKey("HighRate",DbAction.Update)]
        public string HighRate { get => highRate.ToString(); set => highRate = value.toMapObj<HighRateValue>(); }
        [DbRowKey("beEnable", DbAction.Update)]
        public bool beEnable { get; set; } = true;
        [DbRowKey("cDate", RowAttribute.CreatTime, true)]
        public DateTime cDate { get; set; }
        [DbRowKey("Remarker", DbAction.Update)]
        public string Remarker { get; set; } = "";

        public LowRateValue lowRate = new LowRateValue();
        public HighRateValue highRate = new HighRateValue();
        //public HighRateValue highRate = new HighRateValue()
        //{
        //    May=10,
        //    June=11
        //};

        public static CPS_ElectricityRate defaultRate = EkiSQL.ekicps
                .data<CPS_ElectricityRate>("where beEnable=1 and Year=@year", new { year = -1 });

        public override bool CreatById(int id) => EkiSQL.ekicps.loadDataById(id, this);
        public override int Insert(bool isReturnId = false) => EkiSQL.ekicps.insert(this, isReturnId);
        public override bool Update() => EkiSQL.ekicps.update(this);
        public override bool Delete() => EkiSQL.ekicps.delete(this);

        [RateTimeRange("07:30:00~22:30:00")]
        public class HighRateValue : RateValue
        {

        }
        [RateTimeRange("00:00:00~07:30:00", "22:30:00~24:00:00")]
        public class LowRateValue : RateValue
        {

        }

        /// <summary>
        /// 電力費率
        /// (每度電/元)
        /// </summary>
        public class RateValue : MapString.Root
        {

            [MapString(0)]
            public double Jan { get; set; } = AppConfig.DefaultRate;//1月值
            [MapString(1)]
            public double Feb { get; set; } = AppConfig.DefaultRate;//2
            [MapString(2)]
            public double March { get; set; } = AppConfig.DefaultRate;
            [MapString(3)]
            public double April { get; set; } = AppConfig.DefaultRate;
            [MapString(4)]
            public double May { get; set; } = AppConfig.DefaultRate;
            [MapString(5)]
            public double June { get; set; } = AppConfig.DefaultRate;
            [MapString(6)]
            public double July { get; set; } = AppConfig.DefaultRate;
            [MapString(7)]
            public double August { get; set; } = AppConfig.DefaultRate;
            [MapString(8)]
            public double September { get; set; } = AppConfig.DefaultRate;
            [MapString(9)]
            public double October { get; set; } = AppConfig.DefaultRate;
            [MapString(10)]
            public double November { get; set; } = AppConfig.DefaultRate;
            [MapString(11)]
            public double December { get; set; } = AppConfig.DefaultRate;

            public string mapSymbol() => ",";

            public double getMonthValue(int month)
            {
                var value = (from p in this.GetType().GetProperties()
                             where p.isDefinedAttr<MapString>()
                             where (p.getAttribute<MapString>().index + 1) == month
                             select (double)p.GetValue(this)).FirstOrDefault();
                return value;
            }

            public override string ToString()
            {
                return this.toSymbolString();
            }
        }

        public RateValue findApplyRate(DateTime time)
        {
            //離峰時間的費率先判斷 
            var lowSet = lowRate.GetType().getAttribute<RateTimeRange>();
            if (lowSet.range.Any(r => r.between(time.TimeOfDay)))
                return lowRate;

            //沒找到再使用高費率
            return highRate;
        }


        /// <summary>
        /// 先找電錶>社區>預設
        /// </summary>
        /// <param name="time"></param>
        /// <param name="cpSerial"></param>
        /// <returns></returns>
        public static double findRate(DateTime time,string cpSerial=null)
        {
            if (cpSerial != null)
            {
                var cp = EkiSQL.ekicps.data<CPS_ChargePoint>("where serial=@ser", new { ser = cpSerial });
                if(EkiSQL.ekicps.hasData<CPS_MeterRate>("where MeterId=@mid",new { mid = cp.MeterId }))
                {
                    var mRate = (from mr in EkiSQL.ekicps.table<CPS_MeterRate>()
                                 where mr.MeterId == cp.MeterId
                                 join r in EkiSQL.ekicps.table<CPS_ElectricityRate>() on mr.RateId equals r.Id
                                 where r.beEnable
                                 where r.Year == time.Year
                                 select r.findApplyRate(time).getMonthValue(time.Month)).FirstOrDefault();
                    return mRate <= 0 ? defaultRate.findApplyRate(time).getMonthValue(time.Month) : mRate;
                }

                var meter = new CPS_ElectricMeter().Also(m => m.CreatById(cp.MeterId));
                if(EkiSQL.ekicps.hasData<CPS_CommunityRate>("where CommunityId=@cid",new { cid = meter.CommunityId }))
                {
                    var cRate = (from cr in EkiSQL.ekicps.table<CPS_CommunityRate>()
                                 where cr.CommunityId == meter.CommunityId
                                 join r in EkiSQL.ekicps.table<CPS_ElectricityRate>() on cr.RateId equals r.Id
                                 where r.beEnable
                                 where r.Year == time.Year
                                 select r.findApplyRate(time).getMonthValue(time.Month)).FirstOrDefault();
                    return cRate <= 0 ? defaultRate.findApplyRate(time).getMonthValue(time.Month) : cRate;
                }

            }
            //var data = EkiSQL.ekicps
            //    .data<CPS_ElectricityRate>("where beEnable=1 and Year=@year", new { year = -1 });

            return defaultRate.findApplyRate(time).getMonthValue(time.Month);
        }
    }
}