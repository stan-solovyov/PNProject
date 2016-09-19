using System;
using Autofac;
using BLL.Services;
using BLL.Services.ProductService;
using BLL.Services.UserService;
using Domain.EF;
using Domain.Entities;
using Domain.Repository;
using NotificationApp.Parsers;
using NotificationApp.Services;

namespace NotificationApp
{
    class Program
    {
        private static IContainer Container { get; set; }
        static void Main()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<PriceParser>().As<IParser>();
            builder.RegisterType<PriceComparisonJob>().AsSelf().InstancePerLifetimeScope();
            builder.Register(context => new UserContext()).As<UserContext>().InstancePerLifetimeScope();
            builder.RegisterType<ExternalProductService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<MailService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ProductRepository>().As<IRepository<Product>>().InstancePerLifetimeScope();
            builder.RegisterType<ProductService>().As<IProductService>().InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<IRepository<User>>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            Container = builder.Build();

            using (var scope = Container.BeginLifetimeScope())
            {
                var app = scope.Resolve<PriceComparisonJob>();
                 app.Compare().Wait();
            }
        }
    }
}
