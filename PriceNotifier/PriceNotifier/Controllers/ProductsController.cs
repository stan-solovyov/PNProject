using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using BLL.Services.PriceParserService;
using BLL.Services.ProductService;
using BLL.Services.UserService;
using Domain.Entities;
using PriceNotifier.AuthFilter;
using PriceNotifier.DTO;

namespace PriceNotifier.Controllers
{
    [TokenAuthorize]
    public class ProductsController : ApiController
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IOneKPriceParser _oneKPriceParser;
        private readonly IMigomPriceParser _migomPriceParser;

        public ProductsController(IProductService productService, IUserService userService,IOneKPriceParser oneKPriceParser,IMigomPriceParser migomPriceParser)
        {
            _productService = productService;
            _userService = userService;
            _oneKPriceParser = oneKPriceParser;
            _migomPriceParser = migomPriceParser;
        }

        // GET: api/Products

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var owinContext = Request.GetOwinContext();
            var userId = owinContext.Get<int>("userId");
            var products = await _productService.GetByUserId(userId);
            List<ProductDto> productDtos = new List<ProductDto>();
            foreach (var product in products)
            {
                var p = Mapper.Map<Product, ProductDto>(product);
                p.Checked = product.UserProducts.Where(c => c.ProductId == product.ProductId && c.UserId == userId).Select(b => b.Checked).Single();
                p.ImageUrl = product.ProvidersProductInfos.First(c => c.ProductId == product.ProductId).ImageUrl;
                p.MaxPrice = product.ProvidersProductInfos.Where(c => c.ProductId == product.ProductId).Max(c=>c.MaxPrice);
                p.MinPrice = product.ProvidersProductInfos.Where(c => c.ProductId == product.ProductId).Min(c=>c.MinPrice);
                p.Url = product.ProvidersProductInfos.First(c => c.ProductId == product.ProductId).Url;
                productDtos.Add(p);
            }

            return productDtos;
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
                productFound.UserProducts.Single(c => c.ProductId == productDto.Id && c.UserId == userId).Checked = productDto.Checked;
                await _productService.Update(productFound);
                productDto = Mapper.Map(productFound, productDto);
                productDto.Checked = productFound.UserProducts.Single(c => c.ProductId == productDto.Id && c.UserId == userId).Checked;
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

            var product = Mapper.Map<ProductDto, Product>(productDto);
            User user = await _userService.GetById(userId);
            var p = _productService.GetByExtIdFromDb(product.ExternalProductId);
            string html, address;

            if (p == null)
            {
                product = await _productService.Create(product);
                product.ProvidersProductInfos.Add(new ProvidersProductInfo { ImageUrl = productDto.ImageUrl, MinPrice = productDto.MinPrice, MaxPrice = productDto.MaxPrice, ProviderName = "Onliner", Url = productDto.Url });
                address = await _oneKPriceParser.GetProdLinks(productDto.Name);
                if (!string.IsNullOrEmpty(address))
                {
                    html = await _oneKPriceParser.GetExternalPrductPage(address);
                    var providersProductInfo = _oneKPriceParser.ParsePrice(html);
                    providersProductInfo.Url = address;
                    product.ProvidersProductInfos.Add(providersProductInfo);
                }

                address = await _migomPriceParser.GetProdLinks(productDto.Name);
                if (!string.IsNullOrEmpty(address))
                {
                    html = await _migomPriceParser.GetExternalPrductPage(address);
                    var providersProductInfo = _migomPriceParser.ParsePrice(html);
                    providersProductInfo.Url = address;
                    product.ProvidersProductInfos.Add(providersProductInfo);
                }

                await _productService.Update(product);
                user.UserProducts.Add(new UserProduct { Checked = true, ProductId = product.ProductId, UserId = user.UserId });
                await _userService.Update(user);
                productDto = Mapper.Map(product, productDto);
                return productDto;
            }

            address = await _oneKPriceParser.GetProdLinks(productDto.Name);
            if (!string.IsNullOrEmpty(address))
            {
                html = await _oneKPriceParser.GetExternalPrductPage(address);
                var oneKProductInfo = _oneKPriceParser.ParsePrice(html);
                var oneKProduct = p.ProvidersProductInfos.Single(c => c.Url == address);
                oneKProduct.MinPrice = oneKProductInfo.MinPrice;
                oneKProduct.MaxPrice = oneKProductInfo.MaxPrice;
            }

            var onlinerProduct = p.ProvidersProductInfos.Single(c => c.Url == productDto.Url);
            onlinerProduct.MinPrice = productDto.MinPrice;
            onlinerProduct.MaxPrice = productDto.MaxPrice;
            await _productService.Update(p);

            var productFound = _productService.GetByExtId(productDto.ExternalProductId, userId);
            if (productFound == null)
            {
                user.UserProducts.Add(new UserProduct {Checked = true, ProductId = p.ProductId, UserId = user.UserId});
                await _userService.Update(user);
                return productDto;
            }

            throw new HttpResponseException(HttpStatusCode.Conflict);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var owinContext = Request.GetOwinContext();
            var userId = owinContext.Get<int>("userId");
            User user = await _userService.GetById(userId);
            Product product = await _productService.GetById(id);

            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            await _productService.DeleteFromUserProduct(user.UserId, product.ProductId);
            if (product.UserProducts.All(c => c.ProductId != id))
            {
                await _productService.Delete(product);
            }

            return Ok();
        }
    }
}