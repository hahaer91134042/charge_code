using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Eki_Web_MVC.Api
{
    [RoutePrefix("Sms")]
    public class SmsController : IBaseApiController
    {

        [HttpPost]
        [Route("Confirm")]
        public object Confirm()
        {
            try
            {

            }
            catch (Exception)
            {

                
            }
            return ResponseError();
        }


        public class SmsResponse : IResponseInfo<object>
        {
            public string checkCode;

            public SmsResponse(bool successful) : base(successful)
            {
            }
        }
    }
}
