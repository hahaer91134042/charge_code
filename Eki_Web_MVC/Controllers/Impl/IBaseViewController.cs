using DevLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Eki_Web_MVC
{
    public abstract class IBaseViewController<T> : Controller where T:IBaseManagerModel
    {
        //protected T model = (T)Activator.CreateInstance(typeof(T),new { });
        protected T model 
        { 
            get {
                if (m == null)
                {
                    if(this is IBaseManagerModel.IModelConfig)
                    {
                        m = (T)Activator.CreateInstance(typeof(T),
                            ((IBaseManagerModel.IModelConfig)this).modelConstructor());
                    }
                    else
                    {
                        m = Activator.CreateInstance<T>();
                    }
                }
                return m;
            } 
        }
        private T m=null;
        protected ActionResult Error404Page() => RedirectToAction("404", "Error");
        protected void redirectToError(int code = 404) => HttpContext.Response.Redirect($"~/Error/{code}");
        protected string clientIp()
        {
            var ip= WebUtil.clientIP();
            if (ip.Length < 6) ip = "";
            return Server.HtmlEncode(ip);
        }

        protected object ResponseError(EkiErrorCode code=EkiErrorCode.E004) => new ErrorResponse(code);

    }
}