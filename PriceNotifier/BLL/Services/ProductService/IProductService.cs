using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace BLL.Services.ProductService
{
    public interface IProductService : IService<Product>
    {
        IQueryable<Product> GetByUserId(int userId);
        IQueryable<Product> GetAllProducts();
        Product GetByExtId(string externalProductId, int userId);
        Product GetByExtIdFromDb(string externalProductId);
        Product Get(int productId, int userId);
        Task DeleteFromUserProduct(int userId, int productId);
        Task<IEnumerable<Product>> GetTrackedItems();
    }
}
