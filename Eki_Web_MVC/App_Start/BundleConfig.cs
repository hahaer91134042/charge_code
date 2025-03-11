using System.Web;
using System.Web.Optimization;

namespace Eki_Web_MVC
{
    public class BundleConfig
    {
        // 如需統合的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/jquery").Include(
                        "~/Scripts/jquery-3.6.0.js",
                        "~/Scripts/jquery-ui.min.js",
                        "~/Scripts/jquery.easing.js"
                        ));

            bundles.Add(new ScriptBundle("~/js/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));


            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好可進行生產時，請使用 https://modernizr.com 的建置工具，只挑選您需要的測試。
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));


            //bootstrap4 相依的popper必須要用umd的才能 其他的會抓不到
            //這邊Scriptbundle->Bundle可能是Webgrace有點問題
            bundles.Add(new Bundle("~/js/bootstrap").Include(
                       //"~/Scripts/popper-utils.js",
                       //"~/Scripts/popper.js",

                       "~/lib/popper.js/umd/popper-utils.js",
                       "~/lib/popper.js/umd/popper.js",
                       "~/Scripts/bootstrap.js",
                       "~/Scripts/bootstrap.bundle.js"
                    ));

            bundles.Add(new ScriptBundle("~/js/plugin").Include(
                          "~/Scripts/moment.js",
                          "~/Scripts/lodash.core.js",
                          "~/Scripts/lodash.js",
                          "~/Scripts/axios.js",
                          "~/Scripts/plugin/base64.js",
                          "~/lib/js-xss/xss.js",
                          "~/lib/scriptjs/dist/script.js",                          
                          "~/Scripts/toastr.js",
                          "~/Scripts/plugin/nprogress.js",
                          "~/Scripts/plugin/nprogressbar.js",
                          "~/Scripts/plugin/EkiModel.js",
                          "~/Scripts/plugin/EkiFunc.js",
                          "~/Scripts/plugin/EkiExt.js",
                          "~/Scripts/plugin/Firestore_Cps.js",
                          "~/Scripts/plugin/EkiOCPP.js",
                          "~/Scripts/jquery-ui-timepicker-addon.min.js",
                          "~/Scripts/jquery-ui-datepicker-zh-TW.js",
                          "~/Scripts/jquery-ui-timepicker-zh-TW.js",
                          "~/Scripts/jquery.colorbox.js"
                    ));


            bundles.Add(new ScriptBundle("~/js/vue").Include(
                    "~/Scripts/vue.js"
                ));


            bundles.Add(new StyleBundle("~/css/jquery").Include(
                        "~/Content/jquery/jquery-ui.min.css",        //這要注意css資料夾內的image
                        "~/Content/jquery/jquery-ui.structure.min.css",
                        "~/Content/jquery/jquery-ui.theme.min.css"
                ));

            bundles.Add(new StyleBundle("~/css/plugin").Include(
                       "~/Content/jquery/jquery-ui.min.css",        //這要注意css資料夾內的image
                       "~/Content/jquery/jquery-ui.structure.min.css",
                       "~/Content/jquery/jquery-ui.theme.min.css",
                       "~/Content/toastr.css",
                       "~/Content/jquery-ui-timepicker-addon.min.css",
                       "~/Content/nprogress.css",
                       "~/Content/colorbox/colorbox.css"
                ));

            bundles.Add(new StyleBundle("~/css/custom").Include(
                           "~/Content/custom/Main.css",
                           "~/Content/custom/cssStyle.css",
                           "~/Content/custom/main_block.css",
                           "~/Content/custom/Site.css",
                           "~/Content/custom/EkiCss.css",
                           "~/Content/custom/LeftMenu.css"
                     ));

            bundles.Add(new StyleBundle("~/css/bootstrap").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-grid.css",
                      "~/Content/bootstrap-reboot.css"

                    ));
        }
    }
}
