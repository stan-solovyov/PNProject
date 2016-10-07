using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repository
{
    public interface IUserProductRepository : IRepository<UserProduct>
    {
        Task Delete(int userId, int productId);
    }
}
