using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.ProductService
{
    public interface IService<T>:IDisposable where T : class
    {
        IEnumerable<T> GetByUserId(int userid);
        Task Create(T entity);
        Task Update(T entity);
        Task<T> GetById(int id);
        T FindSpecificProduct(T entity,int id);
        Task Delete(T entity);
    }
}
