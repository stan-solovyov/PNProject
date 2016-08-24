using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using BLL.ProductService;
using Domain.Entities;
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
        public async Task IfWeGetRightProductById()
        {
            //Arrange
            var mockService = new Mock<IService<Product>>();
            mockService.Setup(x => x.GetById(297))
                .ReturnsAsync(new Product
                {
                    Id = 297,
                    Checked = true,
                    ExternalProductId = "12345",
                    Name = "asdasasf",
                    Price = "3214",
                    Url = "aasdsad",
                    ImageUrl = "asdasd",
                    UserId = 1
                });
            var controller = new ProductsController(mockService.Object);
            //Act
            ProductDto result = await controller.Get(297);
            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task NoSuchProduct()
        {
            //Arrange
            var mockService = new Mock<IService<Product>>();
            var controller = new ProductsController(mockService.Object);
            //Act
            ProductDto result = await controller.Get(300);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task IfWeDeleteTheProduct()
        {
            //Arrange
            var mockService = new Mock<IService<Product>>();
           
            //Act
            
            //Assert
           
        }
    }
}
