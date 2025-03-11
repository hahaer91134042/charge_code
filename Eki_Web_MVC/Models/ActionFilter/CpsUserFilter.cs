using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Eki_Web_MVC.CPS;

namespace Eki_Web_MVC
{
    public class CpsUserFilter : IMvcActionFilter
    {
        protected override void actionExecuteing(ActionExecutingContext context)
        {
            var user = CPS_Community.fromSession();

            if (user == null)
            {
                Redirect("~/");
                return;
            }
            if (!user.beEnable)
            {
                Redirect($"~/Error/{HttpStatusCode.Unauthorized.toInt()}");
                return;
            }

        }
    }
}