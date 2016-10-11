using Autofac;
using BLL.Services.PriceHistoryService;
using BLL.Services.ProductService;
using BLL.Services.UserService;
using Domain.EF;
using Domain.Entities;
using Domain.Repository;
using NotificationApp.Interfaces;
using NotificationApp.JobSetup;
using NotificationApp.JobTask;
using NotificationApp.Parsers;
using NotificationApp.Services;

namespace NotificationApp
{
    class Program
    {
        static void Main()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<PriceFromOnlinerParser>().As<IPriceParser>();
            builder.RegisterType<PriceComparisonJob>().AsSelf().InstancePerLifetimeScope();
            builder.Register(context => new UserContext()).As<UserContext>().InstancePerLifetimeScope();
            builder.RegisterType<ExternalProductService>().As<IExternalProductService>().InstancePerLifetimeScope();
            builder.RegisterType<MailService>().As<IMailService>().InstancePerLifetimeScope();
            builder.RegisterType<UserProductRepository>().As<IUserProductRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ProductRepository>().As<IRepository<Product>>().InstancePerLifetimeScope();
            builder.RegisterType<ProductService>().As<IProductService>().InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<IRepository<User>>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<PriceHistoryRepository>().As<IRepository<PriceHistory>>().InstancePerLifetimeScope();
            builder.RegisterType<PriceHistoryService>().As<IPriceHistoryService>().InstancePerLifetimeScope();
            builder.RegisterType<MessageService>().As<IMessageService>().InstancePerLifetimeScope();

            var container = builder.Build();
            JobScheduler.Start(container);
        }
    }
}
