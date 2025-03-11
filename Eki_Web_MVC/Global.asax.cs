using Eki_NewebPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Eki_Web_MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //server 設定套用的 ssl
            ServicePointManager.SecurityProtocol =
            SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls |
            SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            NewebPay.LoadMPG(EkiNewebPay.MPG.config());
            NewebPay.LoadInvoice(EkiNewebPay.Invoice.config());
            NewebPay.LoadCreditCard(EkiNewebPay.CreditCard.config());
        }

        
    }
}
