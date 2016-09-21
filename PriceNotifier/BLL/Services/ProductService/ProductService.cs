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
        private readonly UserProductRepository _userProductRepository;

        public ProductService(IRepository<Product> productRepository, UserProductRepository userProductRepository)
        {
            _productRepository = productRepository;
            _userProductRepository = userProductRepository;
        }

        public async Task<IEnumerable<Product>> GetByUserId(int userId)
        {
            return await _productRepository.Query().Where(c => c.UserProducts.Any(b => b.UserId == userId)).ToListAsync();
        }

        public async Task<Product> Create(Product product)
        {
            await _productRepository.Create(product);
            return product;
        }

        public async Task Update(Product product)
        {
            await _productRepository.Update(product);
        }

        public async Task<Product> GetById(int productId)
        {
            Product product = await _productRepository.Get(productId);
            return product;
        }

        public async Task Delete(Product product)
        {
            await _productRepository.Delete(product);
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
            return await _productRepository.Query().Where(c=>c.UserProducts.Any(d=>d.Checked)).ToListAsync();
        }

        public Product GetByExtIdFromDb(string externalProductId)
        {
            return _productRepository.Query().FirstOrDefault(c => c.ExternalProductId == externalProductId);
        }

        public async Task DeleteFromUserProduct(int userId, int productId)
        {
            await _userProductRepository.Delete(userId,productId);
        }
    }
}
