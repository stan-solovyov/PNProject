using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Owin;
using PriceNotifier.DTO;
using WebGrease.Configuration;

[assembly: OwinStartup(typeof(PriceNotifier.Startup))]

namespace PriceNotifier
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);
            AutoMapperInitializer.Initialize();
        }
    }
}
