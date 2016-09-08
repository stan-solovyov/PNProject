using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using BLL.Services;
using BLL.Services.ProductService;
using BLL.Services.UserService;
using Domain.EF;
using Domain.Entities;
using Domain.Repository;
using Microsoft.Owin;
using Owin;
using PriceNotifier.DTO;

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

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
            AutoMapperInitializer.Initialize();
        }
    }
}
