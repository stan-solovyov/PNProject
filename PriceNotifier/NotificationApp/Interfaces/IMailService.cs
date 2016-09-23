namespace NotificationApp.Interfaces
{
    public interface IMailService
    {
        void ProductAvailable(string email, string productUrl, string productName, double priceFromSite);
        void PriceFromDbHigher(string email, string productUrl, string productName, double priceFromDb, double priceFromSite);

        void PriceFromSiteHigher(string email, string productUrl, string productName, double priceFromDb,double priceFromSite);

        void ProductOutOfStock(string email, string productUrl, string productName);
    }
}
