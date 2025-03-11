using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Eki_Web_MVC
{
    public abstract class IApiActionFilter: ActionFilterAttribute
    {
        protected HttpActionContext context;
        protected HttpRequestHeaders headers { get; private set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            context = actionContext;
            headers = context.Request.Headers;
            actionExecuteing(context);
            base.OnActionExecuting(actionContext);
        }

        protected string getHeaderValue(string key)
        {
            if (headers.Contains(key))
                return headers.GetValues(key).FirstOrDefault();
            return "";
        }

        protected void ResponseError(EkiErrorCode errorCode = EkiErrorCode.E011)
        {
            var response = context.Request.CreateResponse(HttpStatusCode.Unauthorized, new ErrorResponse(errorCode));
            context.Response = response;
        }

        protected abstract void actionExecuteing(HttpActionContext context);

        private class ErrorResponse : ResponseDAO
        {
            public ErrorResponse(EkiErrorCode code) : base(false)
            {
                setErrorCode(code);
            }
        }

    }
}