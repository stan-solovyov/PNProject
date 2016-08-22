using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repository
{
    public interface IProductRepository
    {
        IQueryable<Product> GetProductsByUserId();
        Task Update(Product product);
        Task Create(Product product);
        Task Delete(Product product);
        Task<Product> FindAsync(int id);
        Task SaveChangesAsync();
        void Dispose();
    }
}
