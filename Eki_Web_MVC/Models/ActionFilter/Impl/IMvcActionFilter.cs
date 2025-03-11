using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

/// <summary>
/// BaseFilterAttr 的摘要描述
/// </summary>
namespace Eki_Web_MVC
{
    public abstract class IMvcActionFilter : ActionFilterAttribute
    {
        protected HttpContextBase httpContext;
        protected ActionExecutingContext actionContext;


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Log("OnActionExecuting", filterContext.RouteData);        
            actionContext = filterContext;
            httpContext = filterContext.HttpContext;
            actionExecuteing(filterContext);
        }


        internal string getHeaderValue(string key)
        {
            var header = httpContext.Request.Headers;
            if (header.AllKeys.Contains(key))
                return header.GetValues(key).FirstOrDefault();
            return "";
        }

        protected void Redirect(string path)
        {
            httpContext.Response.Redirect(path);
        }


        protected abstract void actionExecuteing(ActionExecutingContext context);

    }
}
