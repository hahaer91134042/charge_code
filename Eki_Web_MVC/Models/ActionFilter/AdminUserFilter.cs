using DevLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Eki_Web_MVC
{
    public class AdminUserFilter : IMvcActionFilter
    {
        //private bool checkToMain=false;
        private ManagerLv checkManagerLv;
        public AdminUserFilter(ManagerLv lv=ManagerLv.SysDev) 
        {
            checkManagerLv = lv;
        }
        //public AdminUserFilter(bool c,ManagerLv lv=ManagerLv.SysDev):this(lv)
        //{
        //    checkToMain = c;
        //}
        protected override void actionExecuteing(ActionExecutingContext context)
        {
            var user = SysManager.fromSession();
            //Log.d($"AdminUserFilter user->{user.toJsonString()}");

            if (user == null)
            {
                Redirect("~/Admin/Login");
                return;
            }

            if (!user.beEnable)
            {
                Redirect($"~/Error/{HttpStatusCode.Unauthorized.toInt()}");
                return;
            }

            if (user.Lv.toInt() < checkManagerLv.toInt())
            {
                Redirect($"~/Error/{HttpStatusCode.Unauthorized.toInt()}");
                return;
            }

            //if (checkToMain)
            //{
            //    if (user != null)
            //    {
            //        Redirect("~/Admin/Main");
            //    }
            //}
            //else
            //{
                
            //}

        }
    }
}