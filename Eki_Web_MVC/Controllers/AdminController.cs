using DevLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eki_Web_MVC.CPS;

namespace Eki_Web_MVC
{

    /// <summary>
    /// 後台管理員登入頁面
    /// </summary>
    [RoutePrefix("Admin")]
    public class AdminController : IBaseViewController<AdminModel>,IBaseManagerModel.IModelConfig
    {

        // GET: Login
        public ActionResult Login()
        {
            ViewBag.ip = clientIp();
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginUser user)
        {
            //Log.d($"Admin Login user->{user.toJsonString()}");
            ViewBag.ip = clientIp();
            
            if (!user.isValid())
            {
                ViewBag.ErrorMsg = " 請輸入 [帳號/密碼/驗證碼] ";
                return View();
            }


            var vCode = ValidateCode.fromSession();
            //Log.d($"Admin Login vCode->{vCode.toJsonString()}");
            if (vCode.code != user.ValidateCode)
            {
                ViewBag.ErrorMsg = " 驗證碼錯誤! ";
                return View();
            }

            var manager = SysManager.creat(user.Account, user.Password);            

            //Log.d($"SysManager->{manager.toJsonString()}");


            if (manager == null)
            {
                ViewBag.ErrorMsg = "帳號/密碼錯誤  請重新輸入";
                return View();
            }

            if (!manager.beEnable)
            {
                ViewBag.ErrorMsg = "該帳號已停權";
                return View();
            }


            manager.LoginTime = DateTime.Now;
            manager.saveSession();

            return RedirectToAction("Main", "Admin");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Admin");
        }


        [Route("")]
        [Route("Main")]
        [AdminUserFilter]
        public ActionResult Main()
        {
            var user = SysManager.fromSession();

            //Log.d($"Main user->{user.toJsonString()}");

            return View();
        }

        #region LeftMenu
        [AdminUserFilter]
        public ActionResult LeftMenu()
        {

            //Log.d($"model user->{model.user}");



            return PartialView(model.getUserMenu());
        }
        #endregion

        #region BodyTop
        [AdminUserFilter]
        public ActionResult BodyTop()
        {


            return PartialView(new List<string>());
        }
        #endregion

        #region GetUser
        [HttpPost]
        public object GetUser()
        {
            return model.user.toJsonString();
        }
        #endregion

        #region Community
        [HttpPost]
        [Route("Community")]
        public JsonResult Community()
        {
            try
            {

                return Json(new AdminResponse(true)
                {
                    info=model.getCommunity()
                });
            }
            catch (Exception)
            {

            }
            return Json(ResponseError());
        }
        #endregion

        #region AddCommunity
        [HttpPost]
        [Route("AddCommunity")]
        public JsonResult AddCommunity(CommunityReq req)
        {
            try
            {
                //Log.d($"add community->{req.toJsonString()}");

                model.addCommunity(req);

                return Json(new AdminResponse(true)
                {
                    info = model.getCommunity()
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

        #region EditCommunity
        [HttpPost]
        [Route("EditCommunity")]
        public JsonResult EditCommunity(CommunityReq req)
        {
            try
            {
                Log.d($"Edit Community->{req.toJsonString()}");
                //這邊暫時不修改Meter
                model.editCommunity(req);

                return Json(new AdminResponse(true)
                {
                    info = model.getCommunity()
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

        #region AddMeter
        [HttpPost]
        [Route("AddMeter")]
        public JsonResult AddMeter(CommunityReq req)
        {
            try
            {
                model.addCommunityMeter(req);

                return Json(new AdminResponse(true)
                {
                    info = model.getCommunity()
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

        #region EditMeter
        [HttpPost]
        [Route("EditMeter")]
        public JsonResult EditMeter(CommunityReq req)
        {
            try
            {
                model.editMeter(req);
                return Json(new AdminResponse(true)
                {
                    info = model.getCommunity()
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

        #region RemoveMeter
        [HttpPost]
        [Route("RemoveMeter")]
        public JsonResult RemoveMeter(CommunityReq req)
        {
            try
            {
                //Log.d($"RemoveMeter req->{req.toJsonString()}");
                model.removeMeter(req);

                return Json(new AdminResponse(true)
                {
                    info = model.getCommunity()
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

        #region GetLocation
        [HttpPost]
        [Route("Location")]
        public JsonResult Location(LocationReq req)
        {
            try
            {
                return Json(new AdminResponse(true)
                {
                    info = model.getLocation(req)
                });
            }
            catch (Exception)
            {

            }
            return Json(ResponseError());
        }
        #endregion

        #region AddLocation
        [HttpPost]
        [Route("AddLocation")]
        public JsonResult AddLocation(LocationReq req)
        {
            try
            {
                model.addLocation(req);

                return Json(new AdminResponse(true)
                {
                    info = model.getLocation(req)
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

        #region EditLocation
        [HttpPost]
        [Route("EditLocation")]
        public JsonResult EditLocation(LocationReq req)
        {
            try
            {
                model.editLocation(req);
                return Json(new AdminResponse(true)
                {
                    info = model.getLocation(req)
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

        #region SetChargeConfig
        [HttpPost]
        [Route("SetChargeConfig")]
        public JsonResult SetChargeConfig(ChargeConfigReq req)
        {
            try
            {
                if (req.locationId < 1)
                    throw new ArgumentNullException();

                //沒有資料 建立新的
                if (req.id < 1)
                {
                    model.addChargeConfig(req);
                }
                else//已存在資料  更新
                {
                    model.updateChargeConfig(req);
                }

                var loc = new CPS_Location().Also(l => l.CreatById(req.locationId));

                return Json(new AdminResponse(true)
                {
                    info=model.getLocation(loc.CommunityId)
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

        #region RemoveLocation
        [HttpPost]
        [Route("RemoveLocation")]
        public JsonResult RemoveLocation(LocationReq req)
        {
            try
            {
                model.removeLocation(req);
                return Json(new AdminResponse(true)
                {
                    info = model.getLocation(req)
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

        #region House
        [HttpPost]
        [Route("House")]
        public JsonResult House(HouseReq req)
        {
            try
            {
                return Json(new AdminResponse(true)
                {
                    info = model.getCommunityHouse(req)
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

        #region AddHouse
        [HttpPost]
        [Route("AddHouse")]
        public JsonResult AddHouse(HouseReq req)
        {
            try
            {
                model.addCommunityHouse(req);
                return Json(new AdminResponse(true)
                {
                    info = model.getCommunityHouse(req)
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

        #region EditHouse
        [HttpPost]
        [Route("EditHouse")]
        public JsonResult EditHouse(HouseReq req)
        {
            try
            {
                model.editCommunityHouse(req);

                return Json(new AdminResponse(true)
                {
                    info = model.getCommunityHouse(req)
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

        #region EditHouse
        [HttpPost]
        [Route("RemoveHouse")]
        public JsonResult RemoveHouse(HouseReq req)
        {
            try
            {
                model.removeCommunityHouse(req);

                return Json(new AdminResponse(true)
                {
                    info = model.getCommunityHouse(req)
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

        #region CommunityCP
        [HttpPost]
        [Route("CommunityCp")]
        public JsonResult CommunityCp(ChargePointReq req)
        {
            try
            {
                //Log.d($"Community Cp req->{req.toJsonString()}");
                if (req.cID < 1)
                    throw new ArgumentNullException();

                return Json(new AdminResponse(true)
                {
                    info= model.getCommunityCp(req.cID)
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

        #region AddCp
        [HttpPost]
        [Route("AddCp")]
        public JsonResult AddCp(ChargePointReq req)
        {
            try
            {
                Log.d($"Add Cp req->{req.toJsonString()}");
                if (req.cID < 1 || req.serial.isNullOrEmpty())
                    throw new ArgumentException();

                if (!model.addCommunityCp(req.convertToDbModel()))
                    return Json(ResponseError(EkiErrorCode.E001));

                return Json(new AdminResponse(true)
                {
                    info=model.getCommunityCp(req.cID)
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

        #region EditCp
        [HttpPost]
        [Route("EditCp")]
        public JsonResult EditCp(ChargePointReq req)
        {
            try
            {

                var cp = model.editCp(req);

                if (cp == null)
                    throw new ArgumentNullException();

                return Json(new AdminResponse(true)
                {
                    info = model.getCommunityCp(cp.CommunityId)
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

        #region RemoveCp
        [HttpPost]
        [Route("RemoveCp")]
        public JsonResult RemoveCp(ChargePointReq req)
        {
            try
            {
                var cp = new CPS_ChargePoint().Also(c => c.CreatById(req.id));

                if (cp.Id < 1)
                    throw new ArgumentNullException();

                model.removeCp(cp);

                return Json(new AdminResponse(true)
                {
                    info = model.getCommunityCp(cp.CommunityId)
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


        public class AdminResponse : IResponseInfo<object>
        {
            public AdminResponse(bool successful) : base(successful)
            {
            }
        }
        public object[] modelConstructor()
            => new object[] { SysManager.fromSession() };
    }
}