using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repository
{
    public interface IProductRepository<T>
    {
        IQueryable<T> GetProductsByUserId();
        Task Update(T entity);
        Task Create(T entity);
        Task Delete(T entity);
        Task<T> FindAsync(int id);
        Task SaveChangesAsync();
        void Dispose();
    }
}
