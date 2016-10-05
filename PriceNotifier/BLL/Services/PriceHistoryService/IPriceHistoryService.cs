using System.Linq;
using Domain.Entities;

namespace BLL.Services.PriceHistoryService
{
    public interface IPriceHistoryService:IService<PriceHistory>
    {
        IQueryable<PriceHistory> GetByProductId(int productId);
    }
}
