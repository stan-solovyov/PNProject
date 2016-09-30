using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace BLL.Services.PriceHistoryService
{
    public interface IPriceHistoryService:IService<PriceHistory>
    {
        Task<IEnumerable<PriceHistory>> GetByProductId(int productId);
    }
}
