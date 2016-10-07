using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.EF;
using Domain.Entities;

namespace Domain.Repository
{
    public class ProductRepository: IRepository<Product>
    {
        private readonly UserContext _context;
        private readonly DbSet<Product> DbSet;

        public ProductRepository(UserContext context)
        {
            _context = context;
            DbSet = _context.Set<Product>();
        }

        public IQueryable<Product> Query()
        {
            return _context.Products;
        }

        public async Task<Product> Get(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Product> Create(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task Delete(Product product)
        {
            DbSet.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
