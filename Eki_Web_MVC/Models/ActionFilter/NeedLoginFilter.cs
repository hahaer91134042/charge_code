using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Eki_Web_MVC
{
    public class NeedLoginFilter : IMvcActionFilter
    {
        
        protected override void actionExecuteing(ActionExecutingContext context)
        {
            //這邊要檢查 Admin 或者社區使用者是否有登入用 都沒登入就404
            var user = SysManager.fromSession();
            Log.d($"NeedLoginFilter user->{user.toJsonString()}");
            //這邊之後新增社區使用者

            if (user == null)
            {
                Redirect($"~/Error/{HttpStatusCode.Forbidden.toInt()}");
            }
        }
    }
}