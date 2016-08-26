using System;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Query();
        Task<T> Get(int i);
        Task Update(T entity);
        Task Create(T entity);
        Task Delete(T entity);
    }
}
