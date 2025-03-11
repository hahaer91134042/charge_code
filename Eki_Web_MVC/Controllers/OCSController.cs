using DevLibs;
using Eki_Web_MVC.CPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Eki_Web_MVC
{
    [RoutePrefix("OCS")]
    public class OCSController : IBaseViewController<OCSModel>
    {
        // GET: OCS
        [Route("")]
        public ActionResult Index()
        {
            

            return View();
        }

        [Route("Register")]
        [Route("Register/{uid}")]
        public ActionResult Register(string uid="")
        {
            //Log.d($"register uid->{uid}");

            return View();
        }

        #region Charge
        [Route("Charge/{serial}")]
        public ActionResult Charge(string serial)
        {
            if (serial.isNullOrEmpty())
                return Error404Page();

            var cp = EkiSQL.ekicps.data<CPS_ChargePoint>(
                "where Serial=@ser",
                new { ser = serial });

            if (cp == null)
                return Error404Page();

            if (!cp.beEnable)
                return Error404Page();
            //Log.d($"charge cp ->{cp.toJsonString()}");

            var loc = (from l in EkiSQL.ekicps.table<CPS_Location>()
                       where l.CommunityId == cp.CommunityId
                       where l.CpSerial == cp.Serial
                       where l.beEnable
                       select l).FirstOrDefault();

            if(loc==null)
                return Error404Page();

            var community = new CPS_Community().Also(c => c.CreatById(loc.CommunityId));

            return View(new
            {
                Community=community.Uid.ToString(),
                Cp= cp.convertToResponse(),
                Config=loc?.config?.convertToResponse()
            });
        }
        #endregion

        #region OCS/RegisterUser
        [HttpPost]
        [Route("RegisterUser")]
        public JsonResult RegisterUser(RegisterReq req)
        {
            try
            {

                if (model.addMember(req))
                {
                    return Json(new OCSResponse(true));
                }
                else
                {
                    return Json(ResponseError(EkiErrorCode.E001));
                }
            }
            catch (AccountException)
            {
                return Json(ResponseError(EkiErrorCode.E009));
            }
            catch (Exception)
            {

            }
            return Json(ResponseError());
        }
        #endregion

        #region OCS/SmsConfirm
        [HttpPost]
        [Route("SmsConfirm")]
        public JsonResult SmsConfirm(SendSmsReq req)
        {
            try
            {
                if (req.isEmpty() || !req.isValid())
                    throw new InputFormatException();

                if (EkiSQL.ekicps.count<CPS_Member>(new QueryPair().addQuery("Phone", req.phone)) > 0)
                    throw new PhoneExistException();

                var temp = ResUtil.GetApiRes(req.Lan, "SmsConfirmMsg");

                var random = new Random().Next(0000, 9999).ToString().PadLeft(4, '0');

                var msg = string.Format(temp, random);

                var mobile = req.getSmsCode();

                var result = EkiSms.builder(mobile).setMsg(msg).send();

                if (result.code.Equals(SmsStatus.Success.value))
                {
                    return Json(new OCSResponse(true)
                    {
                        info = new
                        {
                            Code = random
                        }
                    });
                }
                else if (result.code.Equals(SmsStatus.PhoneError.value))
                {
                    return Json(ResponseError(EkiErrorCode.E015));
                }
                else
                {
                    return Json(ResponseError());
                }

                //return Json();
            }
            catch (InputFormatException)
            {
                return Json(ResponseError(EkiErrorCode.E001));
            }
            catch (PhoneExistException)
            {
                return Json(ResponseError(EkiErrorCode.E009));
            }
            catch (Exception e)
            {
                Log.e("SmsConfirm error", e);
            }
            return Json(ResponseError());
        }
        #endregion

        #region OCS/CheckUserPhone
        [HttpPost]
        [Route("CheckUserPhone")]
        public JsonResult CheckUserPhone(SearchReq req)
        {
            try
            {
                if (req.serial.isNullOrEmpty())
                    throw new ArgumentNullException();                

                var user = EkiSQL.ekicps.data<CPS_Member>(
                    "where Phone=@p",
                    new { p = req.serial.cleanXss() });
                if (user == null||!(user?.beEnable??false))
                    throw new ArgumentNullException();

                return Json(new OCSResponse(true));
            }
            catch (ArgumentNullException)
            {
                return Json(ResponseError(EkiErrorCode.E003));
            }
            catch (Exception)
            {

            }
            return Json(ResponseError());
        }
        #endregion

        #region OCS/CheckOut
        [HttpPost]
        [Route("CheckOut")]
        public async Task<JsonResult> CheckOut(OcsCheckOutReq req)
        {
            try
            {
                var member = EkiSQL.ekicps.data<CPS_Member>(
                    "where Phone=@p and beEnable=1",
                    new { p = req.phone });

                if (member == null)
                    throw new ArgumentNullException();

                var cp = EkiSQL.ekicps.data<CPS_ChargePoint>(
                    "where Serial=@ser",
                    new { ser = req.cp });
                if (cp == null)
                    throw new ArgumentNullException();

                //Log.d($"req ->{req.toJsonString()}");
                //Log.d($"CheckOut m=>{member?.toJsonString()}");
                //Log.d($"cp->{cp?.toJsonString()}");

                var result = await model.checkoutOrder(member, cp,
                    req.chargeMin,
                    req.payMethod.toEnum<PayMethod>(),
                    req.invo.toCpsInvo(member));

                Log.d($"checkout result->{result.toJsonString()}");

                return Json(new OCSResponse(true)
                {
                    info = new
                    {
                        Url=result.Url
                    }
                });
            }
            catch (ArgumentNullException)
            {
                return Json(ResponseError(EkiErrorCode.E003));
            }
            catch (Exception)
            {

            }
            return Json(ResponseError());
        }
        #endregion


        public class OCSResponse : IResponseInfo<object>
        {
            public OCSResponse(bool successful) : base(successful)
            {
            }
        }
    }
}