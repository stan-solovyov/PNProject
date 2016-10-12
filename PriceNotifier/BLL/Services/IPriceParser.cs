using System.Threading.Tasks;
using Domain.Entities;

namespace BLL.Services
{
    public interface IPriceParser
    {
        Task<string> GetProdLinks(string productName);
        Task<string> GetExternalPrductPage(string address);
        ProvidersProductInfo ParsePrice(string html);
    }
}
