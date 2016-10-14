using System.Threading.Tasks;

namespace NotificationApp.Interfaces
{
    public interface IExternalProductService
    {
        Task<double> ParsePrice(string address);
    }
}
