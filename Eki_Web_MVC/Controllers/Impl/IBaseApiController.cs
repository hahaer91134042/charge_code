using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Eki_Web_MVC
{
    public abstract class IBaseApiController : ApiController
    {
        protected object ResponseError(EkiErrorCode code = EkiErrorCode.E004) => new ErrorResponse(code);



    }
}