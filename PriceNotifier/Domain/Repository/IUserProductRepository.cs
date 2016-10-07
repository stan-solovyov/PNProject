using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IUserProductRepository
    {
        Task Delete(int userId, int productId);
    }
}
