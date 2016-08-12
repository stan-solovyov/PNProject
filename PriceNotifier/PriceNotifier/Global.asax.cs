﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using OAuth2.Client;
using RestSharp;
using System.Reflection;
using OAuth2;

namespace PriceNotifier
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SetDependencyResolver();
        }

        private void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        private void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                "Login", // Route name
                "login/{providerName}", // URL with parameters
                new
                {
                    controller = "Home",
                    action = "Login",
                    id = UrlParameter.Optional
                });


            routes.MapRoute(
                "Auth", // Route name
                "Auth", // URL with parameters
                new
                {
                    controller = "Home",
                    action = "Auth",
                    id = UrlParameter.Optional
                });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                });
        }

        private void SetDependencyResolver()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder
                .RegisterAssemblyTypes(
                    Assembly.GetExecutingAssembly(),
                    Assembly.GetAssembly(typeof(OAuth2Client)),
                    Assembly.GetAssembly(typeof(RestClient)))
                .AsImplementedInterfaces().AsSelf();

            builder.RegisterType<AuthorizationRoot>()
                .WithParameter(new NamedParameter("sectionName", "oauth2"));

            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }
    }
}
