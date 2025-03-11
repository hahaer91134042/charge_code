using DevLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eki_Web_MVC
{
    /// <summary>
    /// BaseHandler 的摘要描述
    /// </summary>
    public abstract class BaseHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            GetRequest(context);
        }

        protected abstract void GetRequest(HttpContext context);

        bool IHttpHandler.IsReusable => false;

    }
}
