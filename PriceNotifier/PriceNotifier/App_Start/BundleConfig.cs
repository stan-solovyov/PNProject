using System.Web;
using System.Web.Optimization;

namespace PriceNotifier
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/MVCsite.css"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                      "~/Scripts/angular.min.js",
                      "~/Scripts/angular-route.min.js",
                      "~/Scripts/jquery-3.1.0.min.js",
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/loading-bar.min.js",
                      "~/Scripts/angular-animate.min.js",
                      "~/Scripts/toaster.min.js",
                      "~/Scripts/underscore.min.js",
                      "~/Scripts/app/app.js",
                      "~/Scripts/app/Services/tokenService.js",
                      "~/Scripts/app/Services/tokenInterceptor.js",
                      "~/Scripts/app/Services/productService.js",
                      "~/Scripts/app/Services/externalProductService.js",
                      "~/Scripts/app/Services/userService.js",
                      "~/Scripts/app/Services/pagerService.js",
                      "~/Scripts/app/Controllers/MainCtrl.js",
                      "~/Scripts/app/Controllers/UserCtrl.js",
                      "~/Scripts/app/Controllers/ProductCtrl.js"));

            bundles.Add(new StyleBundle("~/Content/app").Include(
                     "~/Content/bootstrap.min.css",
                     "~/Content/Site.css",
                     "~/Content/loading-bar.min.css",
                     "~/Content/toaster.min.css"
                     ));
        }
    }
}
