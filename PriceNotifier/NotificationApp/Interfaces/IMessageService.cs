using Messages;

namespace NotificationApp.Interfaces
{
    public interface IMessageService
    {
        void SendPriceUpdate(UpdatedPricesMessage updatedPricelistMessage);
    }
}
