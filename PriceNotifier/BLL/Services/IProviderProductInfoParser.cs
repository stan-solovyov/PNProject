using System.Threading.Tasks;
using Domain.Entities;

namespace BLL.Services
{
    public interface IProviderProductInfoParser
    {
        Task<ProvidersProductInfo> GetProvidersProductInfo(string productName);
    }
}
