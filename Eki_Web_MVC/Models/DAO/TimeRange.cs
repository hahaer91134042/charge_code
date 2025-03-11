using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    public class TimeRange:IRange<TimeSpan>
    {
        public TimeSpan start;
        public TimeSpan end;
        public TimeRange(TimeSpan s,TimeSpan e)
        {
            start = s;
            end = e;
        }

        public bool between(TimeSpan other)
        {
            return start <= other && other <= end;
        }
    }
}