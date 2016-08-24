using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.ProductService
{
    public interface IService<T>:IDisposable where T : class
    {
        IEnumerable<T> GetByUserId(int userId);
        Task Create(T entity);
        Task Update(T entity);
        Task<T> GetById(int id);
        T GetByExtId(string externalProductId ,int userId);
        T Get(int productId, int userId);
        Task Delete(T entity);
    }
}
