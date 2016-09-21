using System.Threading.Tasks;
using Domain.EF;

namespace Domain.Repository
{
    public class UserProductRepository
    {
        private readonly UserContext _context;

        public UserProductRepository(UserContext context)
        {
            _context = context;
        }


        public async Task Delete(int userId, int productId)
        {
            var productToDelete = await _context.UserProducts.FindAsync(userId, productId);
            _context.UserProducts.Remove(productToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
