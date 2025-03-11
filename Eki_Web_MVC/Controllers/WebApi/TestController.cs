using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DevLibs;

namespace Eki_Web_MVC.Api
{
    [RoutePrefix("api/Test")]
    public class TestController : IBaseApiController
    {
        [HttpPost]
        [Route("StartCharge")]
        public object StartCharge()
        {
            return new CpsApi.AdminStartCharge("CPS16607237646993754")
                .Connect<object>();
        }

        [HttpPost]
        [Route("GetData")]
        public object GetData()
        {
            try
            {
                var url = "http://cpsapi.eki.com.tw/api/test/echo";
                var conn = WebConnect.CreatPost(url);
                conn.setBody("");
                var result = conn.Connect<object>();

                return new
                {
                    Result = result
                };
            }
            catch (Exception e)
            {
                return new
                {
                    Error = e.ToString()
                };
            }

        }
    }
}