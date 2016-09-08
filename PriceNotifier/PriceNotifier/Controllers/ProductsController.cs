using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using BLL.Services.ProductService;
using Domain.Entities;
using PriceNotifier.AuthFilter;
using PriceNotifier.DTO;

namespace PriceNotifier.Controllers
{
    [MyAuthorize]
    public class ProductsController : ApiController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Products

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var owinContext = Request.GetOwinContext();
            var userId = owinContext.Get<int>("userId");
            var users = await _productService.GetByUserId(userId);
            return Mapper.Map<IEnumerable<ProductDto>>(users);
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

            return Mapper.Map<Product, ProductDto>(product);
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
                productDto = Mapper.Map(productFound, productDto);
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

            var owinContext = Request.GetOwinContext();
            var userId = owinContext.Get<int>("userId");

            var productFound = _productService.GetByExtId(productDto.ExternalProductId, userId);
            if (productFound == null)
            {
                var product = Mapper.Map<ProductDto, Product>(productDto);
                product.UserId = userId;
                var productFromDb = await _productService.Create(product);
                productDto = Mapper.Map(productFromDb, productDto);
                return productDto;
            }

            throw new HttpResponseException(HttpStatusCode.Conflict);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Delete(int id)
        {
            Product product = await _productService.GetById(id);
            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            await _productService.Delete(product);
            return Ok();
        }
    }
}