using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace BLL.Services.ProductService
{
    public interface IProductService:IService<Product>
    {
        Task<IEnumerable<Product>> GetByUserId(int userId);
        Product GetByExtId(string externalProductId ,int userId);
        Product Get(int productId, int userId);
    }
}
