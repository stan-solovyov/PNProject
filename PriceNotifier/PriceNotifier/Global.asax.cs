using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using OAuth2.Client;
using RestSharp;
using System.Reflection;
using Domain.Entities;
using OAuth2;
using PriceNotifier.Infrostructure;


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
            builder.RegisterType<ElasticProductService>().As<IElasticService<Product>>().WithParameter(new NamedParameter("elastiSearchServerUrl", ConfigurationManager.AppSettings["elastiSearchServerUrl"]));
            builder.RegisterType<ElasticUserService>().As<IElasticService<User>>().WithParameter(new NamedParameter("elastiSearchServerUrl", ConfigurationManager.AppSettings["elastiSearchServerUrl"]));

            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }
    }
}
