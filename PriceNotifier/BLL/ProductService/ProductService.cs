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


        public IEnumerable<Product> GetByUserId(int id)
        {
            return _productRepository.Query().Where(c => c.UserId == id);
        }

        public async Task Create(Product product)
        {
            await _productRepository.Create(product);
        }

        public async Task Update(Product product)
        {
            await _productRepository.Update(product);
        }

        public async Task<Product> GetById(int id)
        {
            Product product = await _productRepository.GetProduct(id);
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

        public  Product FindSpecificProduct(Product product, int id)
        {
            return _productRepository.Query().Where(c => c.UserId == id).FirstOrDefault(c => c.ProductId == product.ProductId);
        }
    }
}
