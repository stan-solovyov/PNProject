﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.OData;
using System.Web.OData.Extensions;
using System.Web.OData.Query;
using AutoMapper;
using BLL.Services;
using BLL.Services.ProductService;
using BLL.Services.UserService;
using Domain.Entities;
using PriceNotifier.AuthFilter;
using PriceNotifier.DTO;
using PriceNotifier.Infrostructure;

namespace PriceNotifier.Controllers
{
    [TokenAuthorize("Admin", "User")]
    public class ProductsController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IEnumerable<IProviderProductInfoParser> _ppiParsers;

        public ProductsController(IProductService productService, IUserService userService, IEnumerable<IProviderProductInfoParser> ppiParsers)
        {
            _productService = productService;
            _userService = userService;
            _ppiParsers = ppiParsers;
        }

        // GET: api/Products

        public PageResult<ProductDto> GetProducts(ODataQueryOptions<ProductDto> options)
        {
            var userId = GetCurrentUserId(Request);
            var allProducts = _productService.GetByUserId(userId);
            List<ProductDto> productDtos = new List<ProductDto>();
            foreach (Product product in allProducts)
            {
                var p = Mapper.Map<Product, ProductDto>(product);
                p.Checked = product.UserProducts.Where(c => c.UserId == userId).Select(b => b.Checked).Single();
                p.Url = product.ProvidersProductInfos.First(c => c.MinPrice == p.MinPrice).Url;
                productDtos.Add(p);
            }

            var results = options.ApplyTo(productDtos.AsQueryable());

            return new PageResult<ProductDto>(
               results as IEnumerable<ProductDto>,
               Request.ODataProperties().NextLink,
               Request.ODataProperties().TotalCount);
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

            var userId = GetCurrentUserId(Request);

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

            var userId = GetCurrentUserId(Request);

            var user = await _userService.GetById(userId);
            var p = _productService.GetByExtIdFromDb(productDto.ExternalProductId);

            if (p == null)
            {
                var product = Mapper.Map<ProductDto, Product>(productDto);
                product.ProvidersProductInfos.Add(new ProvidersProductInfo { ImageUrl = productDto.ImageUrl, MinPrice = productDto.MinPrice, MaxPrice = productDto.MaxPrice, ProviderName = "Onliner", Url = productDto.Url });
                var tasks = new List<Task<ProvidersProductInfo>>();

                foreach (var parser in _ppiParsers)
                {
                    var providersProductInfo = parser.GetProvidersProductInfo(productDto.Name);
                    tasks.Add(providersProductInfo);
                }

                if (tasks.Count != 0)
                {
                    var providerProductInfoes = await Task.WhenAll(tasks);

                    foreach (var providersProductInfo in providerProductInfoes)
                    {
                        if (providersProductInfo != null)
                        {
                            product.ProvidersProductInfos.Add(providersProductInfo);
                        }
                    }
                }

                product = await _productService.Create(product);
                user.UserProducts.Add(new UserProduct { Checked = true, ProductId = product.ProductId, UserId = user.UserId });
                await _userService.Update(user);
                productDto = Mapper.Map(product, productDto);
                return productDto;
            }

            var productFound = _productService.GetByExtId(productDto.ExternalProductId, userId);
            if (productFound == null)
            {
                user.UserProducts.Add(new UserProduct { Checked = true, ProductId = p.ProductId, UserId = user.UserId });
                await _userService.Update(user);
                return productDto;
            }

            throw new HttpResponseException(HttpStatusCode.Conflict);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var userId = GetCurrentUserId(Request);
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