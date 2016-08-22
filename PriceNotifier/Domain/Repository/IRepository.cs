using System;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IRepository<T> : IDisposable where T : class
    {
        IQueryable<T> Query();
        Task<T> GetProduct(int id);
        Task Update(T entity);
        Task Create(T entity);
        Task Delete(T entity);
    }
}
