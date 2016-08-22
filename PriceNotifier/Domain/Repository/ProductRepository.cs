using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.EF;

namespace Domain.Repository
{
    public class ProductRepository<Product> : IProductRepository<Product> where Product : class
    {
        private readonly UserContext _context;
        protected DbSet<Product> DbSet;

        public ProductRepository(UserContext context)
        {
            _context = context;
            DbSet = _context.Set<Product>();
        }

        public IQueryable<Product> GetProductsByUserId()
        {
            return DbSet;
        }

        public async Task Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Create(Product product)
        {
            DbSet.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Product product)
        {
            DbSet.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> FindAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
