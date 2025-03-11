using DevLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    public static class EkiSerial
    {
        public static Project project = new Project();//要給藍新使用的訂單序號
        public static Order order = new Order();//專案內部訂單序號

        public static void setSerial(this Model obj)
        {
            var builder = obj.applySerial();

            (from p in obj.GetType().GetProperties()
             where p.isDefinedAttr<Property>()
             select p).Foreach(prop =>
             {
                 prop.SetValue(obj, builder.build());
             });
        }

        /// <summary>
        /// 標記用的Flag
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)]
        public class Property:Attribute
        {

        }

        public interface Model
        {
            Builder applySerial();
        }

        public interface Builder
        {
            string build();
        }

        public class Project : Builder
        {
            internal Project() { }
            public string build()
            {
                return $"{EkiProject.PPYP_Charg}{TimeUtil.toStamp(DateTime.Now)}{RandomUtil.GetRandonNumStr(2)}";
            }
        }

        public class Order : Builder
        {
            internal Order() { }
            public string build()
            {
                return $"{EkiProject.ChargeCode}{TimeUtil.toStamp(DateTime.Now)}{RandomUtil.GetRandonNumStr(5)}";
            }
        }

    }
}