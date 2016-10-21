using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using BLL.Services;
using BLL.Services.ArticleService;
using BLL.Services.PriceHistoryService;
using BLL.Services.PriceParserService;
using BLL.Services.ProductService;
using BLL.Services.UserService;
using Domain.EF;
using Domain.Entities;
using Domain.Repository;
using Messages;
using Microsoft.Owin;
using Owin;
using PriceNotifier.DTO;
using PriceNotifier.RSBMessageConsumer;
using Rhino.ServiceBus;
using Rhino.ServiceBus.Impl;

[assembly: OwinStartup(typeof(PriceNotifier.Startup))]

namespace PriceNotifier
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.Register(context => new UserContext()).As<UserContext>().InstancePerRequest();
            builder.RegisterType<ProductRepository>().As<IRepository<Product>>().InstancePerRequest();
            builder.RegisterType<ProductService>().As<IProductService>().InstancePerRequest();
            builder.RegisterType<UserRepository>().As<IRepository<User>>().InstancePerRequest();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest();
            builder.RegisterType<PriceHistoryRepository>().As<IRepository<PriceHistory>>().InstancePerRequest();
            builder.RegisterType<PriceHistoryService>().As<IPriceHistoryService>().InstancePerRequest();
            builder.RegisterType<ArticleRepository>().As<IRepository<Article>>().InstancePerRequest();
            builder.RegisterType<ArticleService>().As<IArticleService>().InstancePerRequest();
            builder.RegisterType<UpdatedPricesConsumer>().As<ConsumerOf<UpdatedPricesMessage>>();
            builder.RegisterType<UserProductRepository>().As<IUserProductRepository>().InstancePerRequest();
            builder.RegisterType<PriceFrom1KParser>().As<IProviderProductInfoParser>().InstancePerRequest();
            builder.RegisterType<PriceFromMigomParser>().As<IProviderProductInfoParser>().InstancePerRequest();
            var container = builder.Build();

            //injecting RSB with Autofac
            new RhinoServiceBusConfiguration().UseAutofac(container).Configure();
            var bus = container.Resolve<IStartableServiceBus>();
            bus.Start();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            app.MapSignalR();
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
            AutoMapperInitializer.Initialize();
        }
    }
}
