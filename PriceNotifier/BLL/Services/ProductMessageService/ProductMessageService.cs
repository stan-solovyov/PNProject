using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Messages;
using Rhino.ServiceBus;
using Rhino.ServiceBus.Hosting;

namespace BLL.Services.ProductMessageService
{
    public class ProductMessageService: IProductMessageService
    {
        public void SendProduct(ProductMessage message)
        {
            var host = new DefaultHost();
            host.Start<PriceNotifierBootstrapper>();
            var bus = host.Bus as IServiceBus;
            bus.Send(message);
        }
    }
}
