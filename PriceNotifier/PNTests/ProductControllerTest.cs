using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using BLL.Services.ProductService;
using BLL.Services.UserService;
using Domain.Entities;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PriceNotifier.Controllers;
using PriceNotifier.DTO;

namespace PNTests
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
        public async Task GetProductsTest()
        {
            //Arrange
            var userId = 11;
            List<UserProduct> products = new List<UserProduct>
            {
                new UserProduct { Checked = true, ProductId = 33, UserId = 11},
                new UserProduct { Checked = true, ProductId = 34, UserId = 11},
                new UserProduct { Checked = true, ProductId = 34, UserId = 17}
            };

            Product product = new Product
            {
                ProductId = 33,
                ExternalProductId = "12345",
                Name = "asdasasf",
                Price = 3214,
                Url = "aasdsad",
                ImageUrl = "asdasd",
                UserProducts = products
            };
            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            mockProductService.Setup(x => x.GetByUserId(userId))
                .ReturnsAsync(new List<Product> { product });
            var controller = new ProductsController(mockProductService.Object, mockUserService.Object);

            //Set up OwinContext
            controller.Request = new HttpRequestMessage();
            controller.Request.SetOwinContext(new OwinContext());
            var owinContext = controller.Request.GetOwinContext();
            owinContext.Set("userId", userId);

            //Act
            IEnumerable<ProductDto> result = await controller.GetProducts();

            //Assert
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public async Task GetExistingProductByIdTest()
        {
            //Arrange
            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            Product product = new Product
            {
                ProductId = 297,
                ExternalProductId = "12345",
                Name = "asdasasf",
                Price = 3214,
                Url = "aasdsad",
                ImageUrl = "asdasd",
            };
            mockProductService.Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(product);
            var controller = new ProductsController(mockProductService.Object, mockUserService.Object);
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
            var controller = new ProductsController(mockProductService.Object, mockUserService.Object);
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
                new UserProduct { Checked = true, ProductId = 34, UserId = 11},
                new UserProduct { Checked = true, ProductId = 34, UserId = 17}
            };

            var product = new Product
            {
                ProductId = productId,
                ExternalProductId = "12345",
                Name = "asdasasf",
                Price = 3214,
                Url = "aasdsad",
                ImageUrl = "asdasd",
                UserProducts = products
            };
            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            mockProductService.Setup(x => x.GetById(productId))
                .ReturnsAsync(new Product
                {
                    ProductId = productId,
                    ExternalProductId = "12345",
                    Name = "asdasasf",
                    Price = 3214,
                    Url = "aasdsad",
                    ImageUrl = "asdasd",
                    UserProducts = products
                }).Verifiable();

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
            var controller = new ProductsController(mockProductService.Object, mockUserService.Object) { Request = new HttpRequestMessage() };
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
                new UserProduct { Checked = true, ProductId = 34, UserId = 11},
                new UserProduct { Checked = true, ProductId = 34, UserId = 17}
            };

            Product newProduct = new Product
            {
                ProductId = productId,
                ExternalProductId = "432",
                Name = "asdasasf",
                Price = 3214,
                Url = "aasdsad",
                ImageUrl = "asdasd",
                UserProducts = products
            };

            ProductDto newProductDto = new ProductDto()
            {
                Id = productId,
                Checked = true,
                ExternalProductId = "432",
                Name = "asdasasf",
                Price = 3214,
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
            mockProductService.Setup(x => x.GetByExtId(newProductDto.ExternalProductId, userId)).Returns((Product)null);
            mockProductService.Setup(x => x.GetByExtIdFromDb(newProduct.ExternalProductId)).Returns(newProduct);
            mockProductService.Setup(x => x.Create(It.IsAny<Product>())).ReturnsAsync(newProduct);
            mockUserService.Setup(x => x.GetById(userId)).ReturnsAsync(user).Verifiable();
            var controller = new ProductsController(mockProductService.Object, mockUserService.Object) { Request = new HttpRequestMessage() };
            //Set up OwinContext
            controller.Request.SetOwinContext(new OwinContext());
            var owinContext = controller.Request.GetOwinContext();
            owinContext.Set("userId", userId);

            //Act
            ProductDto result = await controller.Post(newProductDto);
            //Assert
            Assert.IsNotNull(result);
            mockProductService.Verify();
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
                new UserProduct { Checked = true, ProductId = 34, UserId = 11},
                new UserProduct { Checked = true, ProductId = 34, UserId = 17}
            };

            var product = new Product
            {
                ProductId = productId,
                ExternalProductId = "12345",
                Name = "asdasasf",
                Price = 3214,
                Url = "aasdsad",
                ImageUrl = "asdasd",
                UserProducts = products
            };

            var productDto = new ProductDto
            {
                Id = productId,
                Checked = true,
                ExternalProductId = "12345",
                Name = "asdasasf",
                Price = 3214,
                Url = "aasdsad",
                ImageUrl = "asdasd"
            };

            var mockProductService = new Mock<IProductService>();
            var mockUserServuce = new Mock<IUserService>();
            mockProductService.Setup(x => x.Get(productId, userId))
                .Returns(product);

            mockProductService.Setup(x => x.Update(It.IsAny<Product>())).Returns(Task.FromResult(false)).Verifiable();
            var controller = new ProductsController(mockProductService.Object, mockUserServuce.Object);

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
