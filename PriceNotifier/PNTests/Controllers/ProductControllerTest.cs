using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Query;
using BLL.Services.ProductMessageService;
using BLL.Services.ProductService;
using BLL.Services.UserService;
using Domain.Entities;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PriceNotifier.Controllers;
using PriceNotifier.DTO;
using PriceNotifier.Infrostructure;
using User = Domain.Entities.User;

namespace PNTests.Controllers
{
    [TestClass]
    public class ProductControllerTest
    {
        [TestInitialize]
        public void Initialize()
        {
            AutoMapperInitializer.Initialize();
        }

        [TestMethod]
        public void GetProductsTest()
        {
            //Arrange
            var userId = 11;

            List<UserProduct> products = new List<UserProduct>
            {
                new UserProduct { Checked = true, ProductId = 33, UserId = 11},
                new UserProduct { Checked = true, ProductId = 33, UserId = 14},
                new UserProduct { Checked = true, ProductId = 33, UserId = 17}
            };

            List<ProvidersProductInfo> providersProductInfos = new List<ProvidersProductInfo>
            {
                new ProvidersProductInfo {ImageUrl = "asd",MinPrice = 12, MaxPrice = 16, ProviderName = "Onliner", ProviderId = 1, Url = "qwe",ProductId = 33}
            };

            Product product = new Product
            {
                ProductId = 33,
                ExternalProductId = "12345",
                Name = "asdasasf",
                ProvidersProductInfos = providersProductInfos,
                UserProducts = products
            };

            List<Product> allProducts = new List<Product>();
            allProducts.Add(product);
            IQueryable<Product> queryableAllProducts = allProducts.AsQueryable();

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            var mockProductMessageService = new Mock<IProductMessageService>();
            var mockElasticService = new Mock<IElasticService<Product>>();
            mockProductService.Setup(x => x.GetByUserId(userId))
                .Returns(queryableAllProducts);
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:59476/api/Products/");
            var controller = new ProductsController(mockProductService.Object, mockUserService.Object,  mockElasticService.Object, mockProductMessageService.Object);
            
            //Set up OwinContext
            controller.Request = new HttpRequestMessage();
            controller.Request.SetOwinContext(new OwinContext());
            var owinContext = controller.Request.GetOwinContext();
            owinContext.Set("userId", userId);

            //Set up OData query options
            var path = request.ODataProperties().Path;
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<ProductDto>("Products");
            ODataQueryContext context = new ODataQueryContext(builder.GetEdmModel(), typeof(ProductDto),path);
            ODataQueryOptions<ProductDto> options = new ODataQueryOptions<ProductDto>(context, request);
            
            //Act
            var result = controller.GetProducts(true,options);

            //Assert
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public async Task GetExistingProductByIdTest()
        {
            //Arrange
            List<UserProduct> products = new List<UserProduct>
            {
                new UserProduct { Checked = true, ProductId = 297, UserId = 11},
                new UserProduct { Checked = true, ProductId = 297, UserId = 14},
                new UserProduct { Checked = true, ProductId = 297, UserId = 17}
            };

            List<ProvidersProductInfo> providersProductInfos = new List<ProvidersProductInfo>
            {
                new ProvidersProductInfo {ImageUrl = "asd",MinPrice = 12, MaxPrice = 16, ProviderName = "Onliner", ProviderId = 1, Url = "qwe"}
            };

            Product product = new Product
            {
                ProductId = 297,
                ExternalProductId = "12345",
                Name = "asdasasf",
                ProvidersProductInfos = providersProductInfos,
                UserProducts = products
            };

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            var mockElasticService = new Mock<IElasticService<Product>>();
            var mockProductMessageService = new Mock<IProductMessageService>();

            mockProductService.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(product);
            var controller = new ProductsController(mockProductService.Object, mockUserService.Object, mockElasticService.Object, mockProductMessageService.Object);
            
            //Act
            ProductDto result = await controller.Get(297);

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task GetNotExistingProductByIdTest()
        {
            //Arrange
            var productId = 0;
            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            var mockProductMessageService = new Mock<IProductMessageService>();
            var mockElasticService = new Mock<IElasticService<Product>>();
            var controller = new ProductsController(mockProductService.Object, mockUserService.Object, mockElasticService.Object, mockProductMessageService.Object);
            //Act
            ProductDto result = await controller.Get(productId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteProductTest()
        {
            //Arrange
            var productId = 33;
            var userId = 11;
            List<UserProduct> products = new List<UserProduct>
            {
                new UserProduct { Checked = true, ProductId = 33, UserId = 11},
                new UserProduct { Checked = true, ProductId = 33, UserId = 14},
                new UserProduct { Checked = true, ProductId = 33, UserId = 17}
            };

            List<ProvidersProductInfo> providersProductInfos = new List<ProvidersProductInfo>
            {
                new ProvidersProductInfo {ImageUrl = "asd",MinPrice = 12, MaxPrice = 16, ProviderName = "Onliner", ProviderId = 1, Url = "qwe"}
            };

            var product = new Product
            {
                ProductId = productId,
                ExternalProductId = "12345",
                Name = "asdasasf",
                ProvidersProductInfos = providersProductInfos,
                UserProducts = products
            };

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            var mockProductMessageService = new Mock<IProductMessageService>();
            var mockElasticService = new Mock<IElasticService<Product>>();
            mockProductService.Setup(x => x.GetById(productId))
                .ReturnsAsync(new Product
                {
                    ProductId = productId,
                    ExternalProductId = "12345",
                    Name = "asdasasf",
                    ProvidersProductInfos = providersProductInfos,
                    UserProducts = products
                }).Verifiable();

            mockProductService.Setup(x => x.DeleteFromUserProduct(userId, productId)).Returns(Task.FromResult(false)).Verifiable();
            mockElasticService.Setup(x=>x.DeleteFromIndex(It.IsAny<int>())).Verifiable();
            mockUserService.Setup(x => x.GetById(userId))
               .ReturnsAsync(new User
               {
                   UserId = userId,
                   SocialNetworkName = "Twitter",
                   Username = "Stan",
                   SocialNetworkUserId = "297397558",
                   Token = "4f60b211517aa86a67bace12231d2530",
                   Email = "stanislav.soloviyev@yandex.ru",
                   UserProducts = products
               }).Verifiable();

            mockProductService.Setup(x => x.Delete(product));
            var controller = new ProductsController(mockProductService.Object, mockUserService.Object, mockElasticService.Object, mockProductMessageService.Object) { Request = new HttpRequestMessage() };
            
            //Set up OwinContext
            controller.Request.SetOwinContext(new OwinContext());
            var owinContext = controller.Request.GetOwinContext();
            owinContext.Set("userId", userId);

            //Act
            IHttpActionResult result = await controller.Delete(productId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            mockProductService.Verify();
        }

        [TestMethod]
        public async Task CreateNewProductTest()
        {
            //Arrange
            var userId = 11;
            var productId = 33;
            List<UserProduct> products = new List<UserProduct>
            {
                new UserProduct { Checked = true, ProductId = 33, UserId = 11},
                new UserProduct { Checked = true, ProductId = 33, UserId = 14},
                new UserProduct { Checked = true, ProductId = 33, UserId = 17}
            };

            List<ProvidersProductInfo> providersProductInfos = new List<ProvidersProductInfo>
            {
                new ProvidersProductInfo {ImageUrl = "asd",MinPrice = 12, MaxPrice = 16, ProviderName = "Onliner", ProviderId = 1, Url = "qwe"}
            };

            Product newProduct = new Product
            {
                ProductId = productId,
                ExternalProductId = "432",
                Name = "asdasasf",
                ProvidersProductInfos = providersProductInfos,
                UserProducts = products
            };

            ProductDto newProductDto = new ProductDto()
            {
                Id = productId,
                Checked = true,
                ExternalProductId = "432",
                Name = "asdasasf",
                MinPrice = 12,
                MaxPrice = 16,
                Url = "aasdsad",
                ImageUrl = "asdasd"
            };

            User user = new User
            {
                UserId = userId,
                SocialNetworkName = "Twitter",
                Username = "Stan",
                SocialNetworkUserId = "297397558",
                Token = "4f60b211517aa86a67bace12231d2530",
                Email = "stanislav.soloviyev@yandex.ru",
                UserProducts = products
            };

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            var mockProductMessageService = new Mock<IProductMessageService>();
            var mockElasticService = new Mock<IElasticService<Product>>();
            mockProductService.Setup(x => x.GetByExtId(newProductDto.ExternalProductId, userId)).Returns((Product)null);
            mockProductService.Setup(x => x.GetByExtIdFromDb(newProduct.ExternalProductId)).Returns(newProduct);
            mockProductService.Setup(x => x.Create(It.IsAny<Product>())).ReturnsAsync(newProduct);
            mockUserService.Setup(x => x.GetById(userId)).ReturnsAsync(user).Verifiable();
            mockElasticService.Setup(x => x.AddToIndex(It.IsAny<Product>(), It.IsAny<int>())).Verifiable();
            var controller = new ProductsController(mockProductService.Object, mockUserService.Object, mockElasticService.Object, mockProductMessageService.Object) { Request = new HttpRequestMessage() };
            
            //Set up OwinContext
            controller.Request.SetOwinContext(new OwinContext());
            var owinContext = controller.Request.GetOwinContext();
            owinContext.Set("userId", userId);

            //Act
            ProductDto result = await controller.Post(newProductDto);

            //Assert
            Assert.IsNotNull(result);
            mockProductService.Verify();
            mockElasticService.Verify();
        }

        [TestMethod]
        public async Task UpdateProductTest()
        {

            //Arrange
            var userId = 11;
            var productId = 33;

            List<UserProduct> products = new List<UserProduct>
            {
                new UserProduct { Checked = true, ProductId = 33, UserId = 11},
                new UserProduct { Checked = true, ProductId = 33, UserId = 14},
                new UserProduct { Checked = true, ProductId = 33, UserId = 17}
            };

            List<ProvidersProductInfo> providersProductInfos = new List<ProvidersProductInfo>
            {
                new ProvidersProductInfo {ImageUrl = "asd",MinPrice = 12, MaxPrice = 16, ProviderName = "Onliner", ProviderId = 1, Url = "qwe"}
            };

            var product = new Product
            {
                ProductId = productId,
                ExternalProductId = "12345",
                Name = "asdasasf",
                ProvidersProductInfos = providersProductInfos,
                UserProducts = products
            };

            var productDto = new ProductDto
            {
                Id = productId,
                Checked = true,
                ExternalProductId = "12345",
                Name = "asdasasf",
                MinPrice = 12,
                MaxPrice = 16,
                Url = "aasdsad",
                ImageUrl = "asdasd"
            };

            var mockProductService = new Mock<IProductService>();
            var mockUserServuce = new Mock<IUserService>();
            var mockProductMessageService = new Mock<IProductMessageService>();
            var mockElasticService = new Mock<IElasticService<Product>>();
            mockProductService.Setup(x => x.Get(productId, userId)).Returns(product);

            mockProductService.Setup(x => x.Update(It.IsAny<Product>())).Returns(Task.FromResult(false)).Verifiable();
            var controller = new ProductsController(mockProductService.Object, mockUserServuce.Object, mockElasticService.Object, mockProductMessageService.Object);

            //Set up OwinContext
            controller.Request = new HttpRequestMessage();
            controller.Request.SetOwinContext(new OwinContext());
            var owinContext = controller.Request.GetOwinContext();
            owinContext.Set("userId", userId);

            //Act
            ProductDto result = await controller.Put(productDto);

            //Assert
            Assert.IsNotNull(result);
            mockProductService.Verify();
        }
    }
}
