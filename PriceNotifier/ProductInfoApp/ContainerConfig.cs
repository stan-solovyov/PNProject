using Autofac;
using BLL.Services;
using BLL.Services.PriceParserService;
using BLL.Services.ProductService;
using BLL.Services.UserService;
using Domain.EF;
using Domain.Entities;
using Domain.Repository;
using Messages;
using Rhino.ServiceBus;
using Rhino.ServiceBus.Impl;
using System.Configuration;
using PriceNotifier.Infrostructure;

namespace ProductInfoApp
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            builder.Register(context => new UserContext()).As<UserContext>();
            builder.RegisterType<ProductRepository>().As<IRepository<Product>>();
            builder.RegisterType<ProductService>().As<IProductService>();
            builder.RegisterType<ProductInfoService>().As<IProductInfoService>();
            builder.RegisterType<UserRepository>().As<IRepository<User>>();
            builder.RegisterType<UserProductRepository>().As<IUserProductRepository>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<ProductInfoService>().As<ConsumerOf<ProductMessage>>();
            builder.RegisterType<ElasticProductService>().As<IElasticService<Product>>().WithParameter(new NamedParameter("elastiSearchServerUrl", ConfigurationManager.AppSettings["elastiSearchServerUrl"]));
            builder.RegisterType<PriceFrom1KParser>().As<IProviderProductInfoParser>();
            builder.RegisterType<PriceFromMigomParser>().As<IProviderProductInfoParser>();
            var container = builder.Build();
            new RhinoServiceBusConfiguration().UseAutofac(container).Configure();
            var bus = container.Resolve<IStartableServiceBus>();
            bus.Start();
            return container;
        }
    }
}
