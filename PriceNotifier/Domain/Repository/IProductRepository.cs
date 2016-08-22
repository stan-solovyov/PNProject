using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Domain.Entities;

namespace Domain.Repository
{
    public interface IProductRepository
    {
        IQueryable<Product> GetProducts();
        Task PutProduct(Product product);
        Task PostProduct(Product product);
        Task DeleteProduct(Product product);
        Task<Product> FindAsync(int id);
        Task SaveChangesAsync();
        void Dispose();
    }
}
