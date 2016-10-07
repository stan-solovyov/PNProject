using Messages;
using NotificationApp.Interfaces;
using Rhino.ServiceBus;
using Rhino.ServiceBus.Hosting;

namespace NotificationApp.Services
{
    public class MessageService : IMessageService
    {
        public void SendPriceUpdate(UpdatedPricesMessage updatedPricelist)
        {
            var host = new DefaultHost();
            host.Start<ClientBootStrapper>();
            var bus = host.Bus as IServiceBus;
            bus.Send(updatedPricelist);
        }
    }
}
