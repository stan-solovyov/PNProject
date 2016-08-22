using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Domain.EF;
using Domain.Entities;

namespace Domain.Repository
{
    public class ProductRepository:IProductRepository
    {
        private readonly UserContext _context;
        protected DbSet<Product> DbSet;
        public ProductRepository(UserContext context)
        {
            _context = context;
            DbSet = _context.Set<Product>();
        }


        public IQueryable<Product> GetProducts()
        {
            return DbSet;
        }

        public async Task PutProduct(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task PostProduct(Product product)
        {
            DbSet.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(Product product)
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
