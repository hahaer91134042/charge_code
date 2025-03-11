using DevLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    public static class CpsApi
    {
        public const string domain = "http://ccodeapi.eki.com.tw/api";

        public abstract class IApi<T>
        {
            public T body { get; set; }


            /// <summary>
            /// 產生連接client
            /// 可自行複寫
            /// </summary>
            /// <returns>WebConnect</returns>
            protected virtual WebConnect connector()
            {
                var c = WebConnect.CreatPost(mapUrl());
                c.ContentType = ClientContentType.ApplicationJson;
                return c;
            }
            protected string mapUrl()
            {
                var attr = this.GetType().getAttribute<Path>();
                return $"{domain}/{attr}";
            }

            public R Connect<R>()
            {
                return connector().setBody(body.toJsonString()).Connect<R>();
            }
        }

        [AttributeUsage(AttributeTargets.Class)]
        public class Path : Attribute
        {
            private string path;
            public Path(string p)
            {
                path = p;
            }
            public override string ToString() => path;
        }


        [Path("admin/StartCharge")]
        public class AdminStartCharge : IApi<object>
        {
            public AdminStartCharge(string orderSerial)
            {
                body = new
                {
                    serial = orderSerial
                };
            }

        }


    }
}