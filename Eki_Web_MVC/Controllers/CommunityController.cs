using DevLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eki_Web_MVC.CPS;
using System.Net;

namespace Eki_Web_MVC
{

    /// <summary>
    /// 這邊是給社區成員登入使用
    /// </summary>
    [RoutePrefix("")]
    public class CommunityController : IBaseViewController<CommunityModel>, IBaseManagerModel.IModelConfig
    {
        [Route("TestView")]
        public ActionResult Test()
        {
            return View();
        }

        #region Login
        [Route("")]
        public ActionResult Login()
        {
            HttpContext.Response.StatusCode = HttpStatusCode.NotFound.toInt();

            ViewBag.ip = clientIp();
            var com = CPS_Community.fromSession();

            if (com != null)
                return new RedirectResult($"~/{com.Name}/Main");
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult Login(LoginCommunity req)
        {
            //Log.d($"Login com->{req.toJsonString()}");

            ViewBag.ErrorMsg = "Test";

            if (!req.isValid())
            {
                ViewBag.ErrorMsg = " 請輸入 [帳號/密碼/驗證碼] ";
                return View();
            }

            var vCode = ValidateCode.fromSession();
            //Log.d($"Community Login vCode->{vCode.toJsonString()}");
            if (vCode.code != req.ValidateCode)
            {
                ViewBag.ErrorMsg = " 驗證碼錯誤! ";
                return View();
            }

            var community = CPS_Community.creat(req.ComName, req.Password);

            //Log.d($"Community->{community.toJsonString()}");

            if (community == null)
            {
                ViewBag.ErrorMsg = "帳號/密碼錯誤  請重新輸入";
                return View();
            }

            if (!community.beEnable)
            {
                ViewBag.ErrorMsg = "該帳號已停權";
                return View();
            }

            community.LoginTime = DateTime.Now;
            CPS_Community.updateIp(community, clientIp());

            //Log.d($"login ip->{clientIp()}");
            community.saveSession();

            return new RedirectResult($"~/{community.Name}/Main");
            //return new RedirectResult($"~/TestView");
        }
        #endregion

        #region Logout
        [Route("Logout")]
        public ActionResult Logout()
        {
            Session.Clear();
            return new RedirectResult("~/");
        }
        #endregion

        #region Main
        [Route("~/{name}/Main")]
        [CpsUserFilter]
        public ActionResult Main(string name)
        {
            ViewBag.Title = $"{name}";


            return View();
        }
        #endregion

        #region Meter
        [Route("~/Meter/{meterSerial}")]
        [CpsUserFilter]
        public ActionResult Meter(string meterSerial)
        {
            if (!model.community.meter.Any(m => m.MeterSerial == meterSerial))
                return Error404Page();

            ViewBag.Title = $"{model.community.Name}|{meterSerial}";

            return View();
        }
        #endregion


        #region Cps/User
        [HttpPost]
        [Route("Cps/User")]
        public JsonResult GetUser()
        {
            try
            {
                var user = model.community.convertToResponse();

                return Json(new CpsResponse(true)
                {
                    info = user
                });
            }
            catch (Exception)
            {

            }
            return Json(ResponseError());
        }
        #endregion

        #region Cps/MeterValue
        [HttpPost]
        [Route("Cps/MeterOrder")]
        public JsonResult MeterOrder(SearchReq req)
        {
            try
            {
                object result = null;
                //之後再擴充搜尋方式
                if (!req.time.isNullOrEmpty())
                    result = model.meterOrder(req.time.toDateTime());

                return Json(new CpsResponse(true)
                {
                    info=result
                });
            }
            catch (Exception)
            {

            }
            return Json(ResponseError());
        }
        #endregion

        #region MeterCp
        [HttpPost]
        [Route("Cps/MeterCp")]
        public JsonResult MeterCp(SearchReq req)
        {
            try
            {
                object result = null;

                if (req.serNum.Count>0)
                    result = model.meterCp(req.serNum);

                return Json(new CpsResponse(true)
                {
                    info = result
                });
            }
            catch (ArgumentNullException)
            {
                return Json(ResponseError(EkiErrorCode.E001));
            }
            catch (Exception)
            {

            }

            return Json(ResponseError());
        }
        #endregion

        public class CpsResponse : IResponseInfo<object>
        {
            public CpsResponse(bool successful) : base(successful)
            {
            }
        }

        public object[] modelConstructor()
            => new object[] { CPS_Community.fromSession() };
    }
}