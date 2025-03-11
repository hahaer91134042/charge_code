using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Property|AttributeTargets.Parameter)]
    public class RateTimeRange:Attribute
    {
        public List<TimeRange> range = new List<TimeRange>();

        /// <summary>
        /// 時間間隔字串
        /// </summary>
        /// <param name="args">"HH:mm:ss~HH:mm:ss"</param>
        public RateTimeRange(params string[] args)
        {

            (from item in args
             select item.Split('~')).Foreach(arr =>
             {
                 range.Add(new TimeRange(
                     arr[0].toTimeSpan(),
                     arr[1].toTimeSpan()
                     ));
             });
            //range = args;
        }
    }
}