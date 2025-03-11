using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    public static class IPayCost
    {
        public interface CurrencySet
        {
            CurrencyUnit costUnit();
            double rawCost();
        }
        //public interface Rount : Currency//四捨五入
        //{

        //}
        //public interface Floor : Currency//捨去小數
        //{

        //}
        //public interface Ceiling : Currency//無條件進位
        //{

        //}

        public static double currency(this CurrencySet set,double input,int digits=0)
        {
            switch (set.costUnit())
            {
                case CurrencyUnit.USD:
                    return input;
                default:
                    return Math.Round(input,digits);
            }
        }

        public static double payCost(this CurrencySet pay)
        {
            switch (pay.costUnit())
            {
                case CurrencyUnit.USD:
                    return pay.rawCost();
                default:
                    return Math.Round(pay.rawCost());
            }
        }
    }
}