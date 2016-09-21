using System.Web.Mvc;
using System.Web.Routing;

namespace PriceNotifier
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Login", // Route name
                "login/{providerName}", // URL with parameters
                new
                {
                    controller = "Home",
                    action = "Login",
                    providerName = UrlParameter.Optional
                });

            routes.MapRoute(
                "Email", // Route name
                "email/", // URL with parameters
                new
                {
                    controller = "Home",
                    action = "Email",
                });

            routes.MapRoute(
                name: "AuthRoute",
                url: "Home/Auth/{providerName}",
                defaults: new { controller = "Home", action = "Auth", providerName = UrlParameter.Optional }
            );

            routes.MapRoute(
                    "404-PageNotFound",
                    "{*url}",
                new { controller = "Home", action = "Index"}
    );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
