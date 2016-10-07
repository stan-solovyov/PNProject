using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.EF;
using Domain.Entities;

namespace Domain.Repository
{
    public class UserProductRepository : IUserProductRepository
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

        public IQueryable<UserProduct> Query()
        {
            return _context.UserProducts;
        }

        public Task<UserProduct> Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task Update(UserProduct entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<UserProduct> Create(UserProduct entity)
        {
            _context.UserProducts.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(UserProduct entity)
        {
            _context.UserProducts.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
