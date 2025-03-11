using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Eki_Web_MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            //routes.MapRoute(
            //    name: "Error",
            //    url: "Error/{code}",
            //    defaults: new { controller = "Error", action = "Status", code = 404 }
            //    );
            //參數是3位數
            //routes.MapRoute(
            //    name: "Error",
            //    url: "Error/Status/{code}",
            //    defaults: new { controller = "Error", action = "Status", code = 404 }
            //    );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "OCS", action = "Index", id = UrlParameter.Optional }
            );

            //社區後台 等需要再來 目前不用
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Community", action = "Login", id = UrlParameter.Optional }
            //);


        }
    }
}
