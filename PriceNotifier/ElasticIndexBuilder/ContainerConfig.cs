using System.Configuration;
using Autofac;
using Domain.EF;
using Domain.Entities;
using PriceNotifier.Infrostructure;

namespace ElasticIndexBuilder
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ElasticIndex>().As<IElasticIndex>();
            builder.RegisterType<ElasticProductService>().As<IElasticProductService<Product>>().WithParameter(new NamedParameter("elastiSearchServerUrl", ConfigurationManager.AppSettings["elastiSearchServerUrl"]));
            builder.RegisterType<ElasticUserService>().As<IElasticService<User>>().WithParameter(new NamedParameter("elastiSearchServerUrl", ConfigurationManager.AppSettings["elastiSearchServerUrl"]));
            builder.Register(c => new UserContext());
            return builder.Build();
        }
    }
}
