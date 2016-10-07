using Messages;
using PriceNotifier.Hubs;
using Rhino.ServiceBus;

namespace PriceNotifier.Rhino_Bus_Service
{
    public class UpdatedPricesConsumer : SignalRBase<PriceHub>, ConsumerOf<UpdatedPricesMessage>
    {
        public void Consume(UpdatedPricesMessage message)
        {
            if (message.UpdatedPricesList.Count != 0)
            {
                foreach (var up in message.UpdatedPricesList)
                {
                    Hub.Clients.Group("Clients").updatePrice(up);
                }
            }

        }
    }
}