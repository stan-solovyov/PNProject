using Autofac;
using BLL.Services.ProductService;
using BLL.Services.UserService;
using Domain.EF;
using Domain.Entities;
using Domain.Repository;
using NotificationApp.Interfaces;
using NotificationApp.Parsers;
using NotificationApp.Services;
using Quartz;

namespace NotificationApp
{
    public class PriceUpdate:IJob
    {
        private static IContainer Container { get; set; }
        public void Execute(IJobExecutionContext contextz)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<PriceParser>().As<IParser>();
            builder.RegisterType<PriceComparisonJob>().AsSelf().InstancePerLifetimeScope();
            builder.Register(context => new UserContext()).As<UserContext>().InstancePerLifetimeScope();
            builder.RegisterType<ExternalProductService>().As<IExternalProductService>().InstancePerLifetimeScope();
            builder.RegisterType<MailService>().As<IMailService>().InstancePerLifetimeScope();
            builder.RegisterType<UserProductRepository>().AsSelf().InstancePerLifetimeScope();
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
