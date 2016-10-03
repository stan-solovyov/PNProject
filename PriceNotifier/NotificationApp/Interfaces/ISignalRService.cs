using System.Collections.Generic;
using System.Threading.Tasks;
using PriceNotifier.Models;

namespace NotificationApp.Interfaces
{
    public interface ISignalRService
    {
        Task SendPriceUpdate(List<UpdatedPrice> updatedPricelist);
    }
}
