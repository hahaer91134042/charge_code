using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevLibs;

namespace Eki_Web_MVC
{
    [RoutePrefix("Sys")]
    public class SysController : IBaseViewController<SysModel>, IBaseManagerModel.IModelConfig
    {
        #region Group
        [HttpPost]
        public JsonResult Group()
        {
            return Json(new SysResponse(true)
            {
                info = (from g in EkiSQL.ekicps.table<SysGroup>()
                        where g.beEnable
                        orderby g.Lv ascending
                        select g.convertToResponse())
            });
        }
        #endregion

        #region AdminUser        
        [HttpPost]
        public JsonResult AdminUser()
        {
            var result = (from m in EkiSQL.ekicps.table<SysManager>()
                          select m.convertToResponse());

            return Json(new SysResponse(true)
            {
                info = result
            });
        }
        #endregion

        #region AddUser
        [HttpPost]
        public JsonResult AddUser(UserReq user)
        {
            try
            {
                if (!user.cleanXss())
                    throw new ArgumentException();

                //Log.d($"AddUser->{user.toJsonString()}");
                var success = model.AddUser(user);

                if (success)
                {
                    return Json(new SysResponse(true)
                    {
                        info = (from m in EkiSQL.ekicps.table<SysManager>()
                                where m.beEnable
                                select m.convertToResponse())
                    });
                }
                else
                {
                    return Json(ResponseError(EkiErrorCode.E005));
                }

            }
            catch (ArgumentException)
            {
                return Json(ResponseError(EkiErrorCode.E001));
            }
            catch (Exception e)
            {

            }
            return Json(ResponseError());
        }
        #endregion

        #region EditUser
        [HttpPost]
        public JsonResult EditUser(UserReq user)
        {
            try
            {
                Log.d($"edit user->{user.toJsonString()}");


                return Json(model.editUser(user).Let(success =>
                {
                    if (success)
                        return new SysResponse(true)
                        {
                            info = (from m in EkiSQL.ekicps.table<SysManager>()
                                    select m.convertToResponse())
                        };

                    return ResponseError();
                }));
            }
            catch (Exception e)
            {
                Log.e("edit user error", e);
            }
            return Json(ResponseError());
        }
        #endregion

        #region Menu
        [HttpPost]
        public JsonResult Menu()
        {
            var result = (from m in EkiSQL.ekicps.table<SysMenu>()

                          select m.convertToResponse());

            return Json(new SysResponse(true)
            {
                info = result
            });
        }
        #endregion

        #region AddMenu
        [HttpPost]
        public JsonResult AddMenu(MenuReq req)
        {
            try
            {
                if (!req.cleanXss())
                    throw new ArgumentException();

                model.addNewMenu(req);

                return Json(new SysResponse(true)
                {
                    info = (from m in EkiSQL.ekicps.table<SysMenu>()

                            select m.convertToResponse())
                });
            }
            catch (ArgumentException)
            {
                return Json(ResponseError(EkiErrorCode.E001));
            }
            catch(Exception e)
            {

            }
            return Json(ResponseError());
        }
        #endregion

        #region DelMenu
        [HttpPost]
        public JsonResult DelMenu(MenuReq req)
        {
            try
            {
                if (req.id < 1)
                    throw new ArgumentException();

                model.delMenu(req);

                return Json(new SysResponse(true)
                {
                    info = (from m in EkiSQL.ekicps.table<SysMenu>()

                            select m.convertToResponse())
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

        #region EditMenu
        [HttpPost]
        public JsonResult EditMenu(MenuReq req)
        {
            try
            {
                if (!req.cleanXss())
                    throw new ArgumentException();

                model.editMenu(req);

                return Json(new SysResponse(true)
                {
                    info = (from m in EkiSQL.ekicps.table<SysMenu>()

                            select m.convertToResponse())
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

        public class SysResponse : IResponseInfo<object>
        {
            public SysResponse(bool successful) : base(successful)
            {
            }
        }

        public object[] modelConstructor()
            => new object[] { SysManager.fromSession() };
    }
}