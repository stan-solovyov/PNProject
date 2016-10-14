using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationApp.Interfaces
{
    public interface IMailService
    {
        Task ProductAvailable(List<string> emails, string productUrl, string productName, double priceFromSite);
        Task PriceFromDbHigher(List<string> emails, string productUrl, string productName, double priceFromDb, double priceFromSite);
        Task PriceFromSiteHigher(List<string> emails, string productUrl, string productName, double priceFromDb,double priceFromSite);
        Task ProductOutOfStock(List<string> emails, string productUrl, string productName);
    }
}
