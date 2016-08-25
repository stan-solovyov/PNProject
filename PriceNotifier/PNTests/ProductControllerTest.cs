using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using BLL.ProductService;
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
        public void GetProductsTest()
        {
            //Arrange
            var userId = 1;
            Product product = new Product
            {
                Id = 297,
                Checked = true,
                ExternalProductId = "12345",
                Name = "asdasasf",
                Price = "3214",
                Url = "aasdsad",
                ImageUrl = "asdasd",
                UserId = userId
            };
            var mockService = new Mock<IService<Product>>();
            mockService.Setup(x => x.GetByUserId(userId))
                .Returns(new List<Product>{ product });
            var controller = new ProductsController(mockService.Object);

            //Set up OwinContext
            controller.Request = new HttpRequestMessage();
            controller.Request.SetOwinContext(new OwinContext());
            var owinContext = controller.Request.GetOwinContext();
            owinContext.Set("userId", userId);
            
            //Act
            IEnumerable<ProductDto> result =  controller.GetProducts();
            
            //Assert
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public async Task GetExistingProductByIdTest()
        {
            //Arrange
            var mockService = new Mock<IService<Product>>();
            Product product = new Product
            {
                Id = 297,
                Checked = true,
                ExternalProductId = "12345",
                Name = "asdasasf",
                Price = "3214",
                Url = "aasdsad",
                ImageUrl = "asdasd",
                UserId = 1
            };
            mockService.Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(product);
            var controller = new ProductsController(mockService.Object);
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
            var productId = 1;
            var mockService = new Mock<IService<Product>>();
            var controller = new ProductsController(mockService.Object);
            //Act
            ProductDto result = await controller.Get(productId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteProductTest()
        {
            //Arrange
            var productId = 297;
            var product = new Product
            {
                Id = productId,
                Checked = true,
                ExternalProductId = "12345",
                Name = "asdasasf",
                Price = "3214",
                Url = "aasdsad",
                ImageUrl = "asdasd",
                UserId = 1
            };
            var mockService = new Mock<IService<Product>>();
            mockService.Setup(x => x.GetById(productId))
                .ReturnsAsync(new Product
                {
                    Id = productId,
                    Checked = true,
                    ExternalProductId = "12345",
                    Name = "asdasasf",
                    Price = "3214",
                    Url = "aasdsad",
                    ImageUrl = "asdasd",
                    UserId = 1
                }).Verifiable();

            mockService.Setup(x => x.Delete(product));
            var controller = new ProductsController(mockService.Object);
            //Act
            IHttpActionResult result = await controller.Delete(productId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            mockService.Verify();
        }

        [TestMethod]
        public async Task CreateNewProductTest()
        {

            //Arrange
            var userId = 1;
            Product product = new Product
            {
                Id = 297,
                Checked = true,
                ExternalProductId = "12345",
                Name = "asdasasf",
                Price = "3214",
                Url = "aasdsad",
                ImageUrl = "asdasd",
                UserId = userId
            };

            Product newProduct = new Product
            {
                Id = 298,
                Checked = true,
                ExternalProductId = "432",
                Name = "asdasasf",
                Price = "3214",
                Url = "aasdsad",
                ImageUrl = "asdasd",
                UserId = userId
            };

            ProductDto newProductDto = new ProductDto()
            {
                Id = 298,
                Checked = true,
                ExternalProductId = "432",
                Name = "asdasasf",
                Price = "3214",
                Url = "aasdsad",
                ImageUrl = "asdasd"
            };

            var mockService = new Mock<IService<Product>>();
            mockService.Setup(x => x.GetByExtId(product.ExternalProductId, userId))
                .Returns(product);

            mockService.Setup(x => x.Create(newProduct)).Callback<Product>(c => Assert.AreEqual(c.UserId, userId));
            var controller = new ProductsController(mockService.Object);

            //Set up OwinContext
            controller.Request = new HttpRequestMessage();
            controller.Request.SetOwinContext(new OwinContext());
            var owinContext = controller.Request.GetOwinContext();
            owinContext.Set("userId", userId);

            //Act
            ProductDto result = await controller.Post(newProductDto);
            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task UpdateProductTest()
        {

            //Arrange
            var userId = 1;
            var productId = 297;
            var product = new Product
            {
                Id = productId,
                Checked = true,
                ExternalProductId = "12345",
                Name = "asdasasf",
                Price = "3214",
                Url = "aasdsad",
                ImageUrl = "asdasd",
                UserId = userId
            };

            var productNew = new Product
            {
                Id = productId,
                Checked = false,
                ExternalProductId = "12345",
                Name = "asdasasf",
                Price = "3214",
                Url = "aasdsad",
                ImageUrl = "asdasd",
                UserId = userId
            };

            var productDto = new ProductDto
            {
                Id = productId,
                Checked = true,
                ExternalProductId = "12345",
                Name = "asdasasf",
                Price = "3214",
                Url = "aasdsad",
                ImageUrl = "asdasd"
            };

            var mockService = new Mock<IService<Product>>();
            mockService.Setup(x => x.Get(productId, userId))
                .Returns(product);

            mockService.Setup(x => x.Update(productNew)).Callback<Product>(c => Assert.AreEqual(c.UserId, userId));
            var controller = new ProductsController(mockService.Object);

            //Set up OwinContext
            controller.Request = new HttpRequestMessage();
            controller.Request.SetOwinContext(new OwinContext());
            var owinContext = controller.Request.GetOwinContext();
            owinContext.Set("userId", userId);

            //Act
            ProductDto result = await controller.Put(productDto);
            //Assert
            Assert.IsNotNull(result);
        }
    }
}
