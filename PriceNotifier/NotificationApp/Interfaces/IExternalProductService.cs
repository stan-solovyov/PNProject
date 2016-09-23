using System.Threading.Tasks;

namespace NotificationApp.Interfaces
{
    public interface IExternalProductService
    {
        Task<string> GetExternalPrductPage(string address);
        double ParsePrice(string html);
    }
}
