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
        Task<IHttpActionResult> PutProduct(int id, Product product);
        Task<IHttpActionResult> PostProduct(Product product);
        Task<IHttpActionResult> DeleteProduct(int id);
    }
}
