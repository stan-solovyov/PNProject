using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IService<T>
    {
        Task<T> Create(T entity);
        Task Update(T entity);
        Task<T> GetById(int id);
        Task Delete(T entity);
    }
}
