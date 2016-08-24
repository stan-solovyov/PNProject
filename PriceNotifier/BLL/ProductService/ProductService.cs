using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.EF;
using Domain.Entities;
using Domain.Repository;

namespace BLL.ProductService
{
    public class ProductService:IService<Product>
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService()
        {
            _productRepository = new ProductRepository(new UserContext());
        }

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public IEnumerable<Product> GetByUserId(int userId)
        {
            return _productRepository.Query().Where(c => c.UserId == userId);
        }

        public async Task Create(Product product)
        {
            await _productRepository.Create(product);
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

        public void Dispose()
        {
            _productRepository.Dispose();
        }

        public  Product GetByExtId(string externalProductId, int userId)
        {
            return _productRepository.Query().Where(c => c.UserId == userId).FirstOrDefault(c => c.ExternalProductId == externalProductId);
        }

        public Product Get(int productId, int userId)
        {
            return _productRepository.Query().Where(c => c.UserId == userId).FirstOrDefault(c => c.Id == productId);
        }
    }
}
