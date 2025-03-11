using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevLibs;
using Eki_Web_MVC.CPS;

namespace Eki_Web_MVC
{
    [RoutePrefix("Admin/Meter")]
    public class MeterController : IBaseViewController<MeterModel>
    {

        [Route("{serial}")]
        [NeedLoginFilter]        
        public ActionResult Detail(string serial)
        {
            //Log.d($"meter detail serial->{serial}");
            var data = model.getMeterDetail(serial);
            if (data == null)
                return Error404Page();


            return View(new
            {
                Community=new CPS_Community().Also(c=>c.CreatById(data.CommunityId)).convertToResponse(),
                Meter=data.convertToResponse(),
                CP=data.CP.convertResponseList<object>()
            });
        }

        #region AddCP
        [HttpPost]
        [Route("AddCP")]
        public JsonResult AddCP(ChargePointReq req)
        {
            try
            {

                model.addMeterCP(req);

                return Json(new MeterResponse(true)
                {
                    info=model.getMeterCP(req)
                });
            }
            catch (ArgumentException)
            {
                return Json(ResponseError(EkiErrorCode.E001));
            }
            catch (Exception)
            {

            }
            return Json(ResponseError());
        }
        #endregion

        #region EditCP
        [HttpPost]
        [Route("EditCP")]
        public JsonResult EditCP(ChargePointReq req)
        {
            try
            {

                model.editMeterCP(req);

                return Json(new MeterResponse(true)
                {
                    info = model.getMeterCP(req)
                });
            }
            catch (ArgumentException)
            {
                return Json(ResponseError(EkiErrorCode.E001));
            }
            catch (Exception)
            {

            }
            return Json(ResponseError());
        }
        #endregion

        #region RemoveCP
        [HttpPost]
        [Route("RemoveCP")]
        public JsonResult RemoveCP(ChargePointReq req)
        {
            try
            {
                model.removeMeterCP(req);

                return Json(new MeterResponse(true)
                {
                    info = model.getMeterCP(req)
                });
            }
            catch (ArgumentException)
            {
                return Json(ResponseError(EkiErrorCode.E001));
            }
            catch (Exception)
            {

            }
            return Json(ResponseError());
        }
        #endregion

        #region CpAuth
        [HttpPost]
        [Route("CpAuth")]
        public JsonResult CpAuth(ChargePointReq req)
        {
            try
            {

                return Json(new MeterResponse(true)
                {
                    info = model.getCpAuth(req.serial)
                });
            }
            catch (ArgumentException)
            {
                return Json(ResponseError(EkiErrorCode.E001));
            }
            catch (Exception)
            {

            }
            return Json(ResponseError());
        }
        #endregion

        #region AddAuth
        [HttpPost]
        [Route("AddAuth")]
        public JsonResult AddAuth(CpAuthReq req)
        {
            try
            {
                model.addCpAuth(req);

                return Json(new MeterResponse(true)
                {
                    info = model.getCpAuth(req.cpSerial)
                });
            }
            catch (ArgumentException)
            {
                return Json(ResponseError(EkiErrorCode.E001));
            }
            catch (Exception)
            {

            }
            return Json(ResponseError());
        }
        #endregion

        #region EditAuth
        [HttpPost]
        [Route("EditAuth")]
        public JsonResult EditAuth(CpAuthReq req)
        {
            try
            {
                model.editAuth(req);

                return Json(new MeterResponse(true)
                {
                    info = model.getCpAuth(req.cpSerial)
                });
            }
            catch (ArgumentException)
            {
                return Json(ResponseError(EkiErrorCode.E001));
            }
            catch (Exception)
            {

            }
            return Json(ResponseError());
        }
        #endregion

        #region RemoveAuth
        [HttpPost]
        [Route("RemoveAuth")]
        public JsonResult RemoveAuth(CpAuthReq req)
        {
            try
            {
                model.removeAuth(req);

                return Json(new MeterResponse(true)
                {
                    info = model.getCpAuth(req.cpSerial)
                });
            }
            catch (ArgumentException)
            {
                return Json(ResponseError(EkiErrorCode.E001));
            }
            catch (Exception)
            {

            }
            return Json(ResponseError());
        }
        #endregion

        public class MeterResponse : IResponseInfo<object>
        {
            public MeterResponse(bool successful) : base(successful)
            {
            }
        }

    }
}