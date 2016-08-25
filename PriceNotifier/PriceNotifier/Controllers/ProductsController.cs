using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using BLL.ProductService;
using Domain.EF;
using Domain.Entities;
using Domain.Repository;
using PriceNotifier.AuthFilter;
using PriceNotifier.DTO;

namespace PriceNotifier.Controllers
{
    [MyAuthorize]
    public class ProductsController : ApiController
    {
        private readonly IService<Product> _productService;

        public ProductsController()
        {
            _productService = new ProductService(new ProductRepository(new UserContext()));
        }

        public ProductsController(IService<Product> productService)
        {
            _productService = productService;
        }

        // GET: api/Products

        public IEnumerable<ProductDto> GetProducts()
        {
            var owinContext = Request.GetOwinContext();
            var userId = owinContext.Get<int>("userId");

            var userProducts = Mapper.Map<IEnumerable<ProductDto>>(_productService.GetByUserId(userId));

            return userProducts;
        }

        // GET: api/Products/5
        [ResponseType(typeof(ProductDto))]
        public async Task<ProductDto> Get(int id)
        {
            Product product = await _productService.GetById(id);
            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            var productDto = Mapper.Map<Product, ProductDto>(product);
            return productDto;
        }

        // PUT: api/Products/
        [ResponseType(typeof(ProductDto))]
        public async Task<ProductDto> Put(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var owinContext = Request.GetOwinContext();
            var userId = owinContext.Get<int>("userId");

            var productFound = _productService.Get(productDto.Id, userId);
            if (productFound != null)
            {
                productFound = Mapper.Map(productDto, productFound);
                await _productService.Update(productFound);
                productDto = Mapper.Map(productFound,productDto);
                return productDto;
            }
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        // POST: api/Products
        [ResponseType(typeof(ProductDto))]
        public async Task<ProductDto> Post(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var product = Mapper.Map<ProductDto, Product>(productDto);
            var owinContext = Request.GetOwinContext();
            var userId = owinContext.Get<int>("userId");
            product.UserId = userId;

            var productFound = _productService.GetByExtId(productDto.ExternalProductId, product.UserId);

            if (productFound == null)
            {
                await _productService.Create(product);
                return productDto;
            }
            throw new HttpResponseException(HttpStatusCode.Conflict);
        }

        [HttpDelete]
        // DELETE: api/Products/5
        [ResponseType(typeof(ProductDto))]
        public async Task<ProductDto> Delete(int id)
        {
            Product product = await _productService.GetById(id);
            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            var productDto = Mapper.Map<Product, ProductDto>(product);
            await _productService.Delete(product);
            return productDto;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _productService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}