using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repository;

namespace BLL.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IUserProductRepository _userProductRepository;

        public ProductService(IRepository<Product> productRepository, IUserProductRepository userProductRepository)
        {
            _productRepository = productRepository;
            _userProductRepository = userProductRepository;
        }

        public  IQueryable<Product> GetByUserId(int userId)
        {
            return  _productRepository.Query().Where(c => c.UserProducts.Any(b => b.UserId == userId)).Include(c => c.ProvidersProductInfos);
        }

        public async Task<Product> Create(Product product)
        {
            await _productRepository.Create(product);
            return product;
        }

        public Task Update(Product product)
        {
            return _productRepository.Update(product);
        }

        public async Task<Product> GetById(int productId)
        {
            Product product = await _productRepository.Get(productId);
            return product;
        }

        public Task Delete(Product product)
        {
            return _productRepository.Delete(product);
        }

        public Product GetByExtId(string externalProductId, int userId)
        {
            return _productRepository.Query().Where(c => c.UserProducts.Any(b => b.UserId == userId)).FirstOrDefault(c => c.ExternalProductId == externalProductId);
        }

        public Product Get(int productId, int userId)
        {
            return _productRepository.Query().Where(c => c.UserProducts.Any(b => b.UserId == userId)).FirstOrDefault(c => c.ProductId == productId);
        }

        public async Task<IEnumerable<Product>> GetTrackedItems()
        {
            return await _productRepository.Query().Where(c => c.UserProducts.Any(d => d.Checked)).Include(c => c.UserProducts.Select(a => a.User)).ToListAsync();
        }

        public Product GetByExtIdFromDb(string externalProductId)
        {
            return _productRepository.Query().FirstOrDefault(c => c.ExternalProductId == externalProductId);
        }

        public Task DeleteFromUserProduct(int userId, int productId)
        {
            return _userProductRepository.Delete(userId, productId);
        }
    }
}
