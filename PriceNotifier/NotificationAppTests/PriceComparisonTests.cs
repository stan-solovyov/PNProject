using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Services.ProductService;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NotificationApp;
using NotificationApp.Interfaces;

namespace NotificationAppTests
{
    [TestClass]
    public class PriceComparisonTests
    {
        [TestMethod]
        public async Task PricesEqual()
        {
            //Arrange
            double parsedPrice = 3214;
            List<UserProduct> userProducts = new List<UserProduct>
            {
                new UserProduct { Checked = true, ProductId = 33, UserId = 11},
                new UserProduct { Checked = true, ProductId = 34, UserId = 11},
                new UserProduct { Checked = true, ProductId = 34, UserId = 17}
            };

            List<Product> products = new List<Product>()
            {
                new Product
                {
                ProductId = 33,
                ExternalProductId = "12345",
                Name = "asdasasf",
                Price = 3214,
                Url = "aasdsad",
                ImageUrl = "asdasd",
                UserProducts = userProducts
                }
            };

            var mockProductService = new Mock<IProductService>();
            var mockExternalProductService = new Mock<IExternalProductService>();
            var mockMailService = new Mock<IMailService>();

            mockProductService.Setup(x => x.GetTrackedItems()).ReturnsAsync(products).Verifiable();
            mockExternalProductService.Setup(x => x.GetExternalPrductPage(It.IsAny<string>())).ReturnsAsync(It.IsAny<string>()).Verifiable();
            mockExternalProductService.Setup(x => x.ParsePrice(It.IsAny<string>())).Returns(parsedPrice).Verifiable();

            var priceComparisonJob = new PriceComparisonJob(mockProductService.Object, mockExternalProductService.Object, mockMailService.Object);

            //Act
            await priceComparisonJob.Compare();
            //Assert

            mockProductService.Verify();
        }

        [TestMethod]
        public async Task ProductAvailable()
        {
            //Arrange
            double parsedPrice = 3214;
            var userId = 11;
            var user = new User
            {
                UserId = userId,
                SocialNetworkName = "Twitter",
                Username = "Stan",
                SocialNetworkUserId = "297397558",
                Token = "4f60b211517aa86a67bace12231d2530",
                Email = "stanislav.soloviyev@yandex.ru"
            };
            List<UserProduct> userProducts = new List<UserProduct>
            {
                new UserProduct { Checked = true, ProductId = 33, UserId = 11,User = user }
            };

            List<Product> products = new List<Product>()
            {
                new Product
                {
                ProductId = 33,
                ExternalProductId = "12345",
                Name = "asdasasf",
                Price = 0,
                Url = "aasdsad",
                ImageUrl = "asdasd",
                UserProducts = userProducts
                }
            };

            var mockProductService = new Mock<IProductService>();
            var mockExternalProductService = new Mock<IExternalProductService>();
            var mockMailService = new Mock<IMailService>();

            mockProductService.Setup(x => x.GetTrackedItems()).ReturnsAsync(products).Verifiable();
            mockProductService.Setup(x => x.Update(It.IsAny<Product>())).Returns(Task.FromResult(false)).Verifiable();
            mockExternalProductService.Setup(x => x.GetExternalPrductPage(It.IsAny<string>())).ReturnsAsync(It.IsAny<string>()).Verifiable();
            mockExternalProductService.Setup(x => x.ParsePrice(It.IsAny<string>())).Returns(parsedPrice).Verifiable();
            mockMailService.Setup(x=>x.ProductAvailable(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>())).Verifiable();

            var priceComparisonJob = new PriceComparisonJob(mockProductService.Object, mockExternalProductService.Object, mockMailService.Object);

            //Act
            await priceComparisonJob.Compare();
            //Assert

            mockProductService.Verify();
        }

        [TestMethod]
        public async Task PriceFromDbHigher()
        {
            //Arrange
            double parsedPrice = 3214;
            var userId = 11;
            var user = new User
            {
                UserId = userId,
                SocialNetworkName = "Twitter",
                Username = "Stan",
                SocialNetworkUserId = "297397558",
                Token = "4f60b211517aa86a67bace12231d2530",
                Email = "stanislav.soloviyev@yandex.ru"
            };
            List<UserProduct> userProducts = new List<UserProduct>
            {
                new UserProduct { Checked = true, ProductId = 33, UserId = 11,User = user }
            };

            List<Product> products = new List<Product>()
            {
                new Product
                {
                ProductId = 33,
                ExternalProductId = "12345",
                Name = "asdasasf",
                Price = 3300,
                Url = "aasdsad",
                ImageUrl = "asdasd",
                UserProducts = userProducts
                }
            };

            var mockProductService = new Mock<IProductService>();
            var mockExternalProductService = new Mock<IExternalProductService>();
            var mockMailService = new Mock<IMailService>();

            mockProductService.Setup(x => x.GetTrackedItems()).ReturnsAsync(products).Verifiable();
            mockProductService.Setup(x => x.Update(It.IsAny<Product>())).Returns(Task.FromResult(false)).Verifiable();
            mockExternalProductService.Setup(x => x.GetExternalPrductPage(It.IsAny<string>())).ReturnsAsync(It.IsAny<string>()).Verifiable();
            mockExternalProductService.Setup(x => x.ParsePrice(It.IsAny<string>())).Returns(parsedPrice).Verifiable();
            mockMailService.Setup(x => x.PriceFromDbHigher(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>())).Verifiable();

            var priceComparisonJob = new PriceComparisonJob(mockProductService.Object, mockExternalProductService.Object, mockMailService.Object);

            //Act
            await priceComparisonJob.Compare();
            //Assert

            mockProductService.Verify();
        }

        [TestMethod]
        public async Task PriceFromSiteHigher()
        {
            //Arrange
            double parsedPrice = 3500;
            var userId = 11;
            var user = new User
            {
                UserId = userId,
                SocialNetworkName = "Twitter",
                Username = "Stan",
                SocialNetworkUserId = "297397558",
                Token = "4f60b211517aa86a67bace12231d2530",
                Email = "stanislav.soloviyev@yandex.ru"
            };
            List<UserProduct> userProducts = new List<UserProduct>
            {
                new UserProduct { Checked = true, ProductId = 33, UserId = 11,User = user }
            };

            List<Product> products = new List<Product>()
            {
                new Product
                {
                ProductId = 33,
                ExternalProductId = "12345",
                Name = "asdasasf",
                Price = 3300,
                Url = "aasdsad",
                ImageUrl = "asdasd",
                UserProducts = userProducts
                }
            };

            var mockProductService = new Mock<IProductService>();
            var mockExternalProductService = new Mock<IExternalProductService>();
            var mockMailService = new Mock<IMailService>();

            mockProductService.Setup(x => x.GetTrackedItems()).ReturnsAsync(products).Verifiable();
            mockProductService.Setup(x => x.Update(It.IsAny<Product>())).Returns(Task.FromResult(false)).Verifiable();
            mockExternalProductService.Setup(x => x.GetExternalPrductPage(It.IsAny<string>())).ReturnsAsync(It.IsAny<string>()).Verifiable();
            mockExternalProductService.Setup(x => x.ParsePrice(It.IsAny<string>())).Returns(parsedPrice).Verifiable();
            mockMailService.Setup(x => x.PriceFromSiteHigher(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>())).Verifiable();

            var priceComparisonJob = new PriceComparisonJob(mockProductService.Object, mockExternalProductService.Object, mockMailService.Object);

            //Act
            await priceComparisonJob.Compare();
            //Assert

            mockProductService.Verify();
        }

        [TestMethod]
        public async Task ProductOutOfStock()
        {
            //Arrange
            double parsedPrice = 0;
            var userId = 11;
            var user = new User
            {
                UserId = userId,
                SocialNetworkName = "Twitter",
                Username = "Stan",
                SocialNetworkUserId = "297397558",
                Token = "4f60b211517aa86a67bace12231d2530",
                Email = "stanislav.soloviyev@yandex.ru"
            };
            List<UserProduct> userProducts = new List<UserProduct>
            {
                new UserProduct { Checked = true, ProductId = 33, UserId = 11,User = user }
            };

            List<Product> products = new List<Product>()
            {
                new Product
                {
                ProductId = 33,
                ExternalProductId = "12345",
                Name = "asdasasf",
                Price = 3300,
                Url = "aasdsad",
                ImageUrl = "asdasd",
                UserProducts = userProducts
                }
            };

            var mockProductService = new Mock<IProductService>();
            var mockExternalProductService = new Mock<IExternalProductService>();
            var mockMailService = new Mock<IMailService>();

            mockProductService.Setup(x => x.GetTrackedItems()).ReturnsAsync(products).Verifiable();
            mockProductService.Setup(x => x.Update(It.IsAny<Product>())).Returns(Task.FromResult(false)).Verifiable();
            mockExternalProductService.Setup(x => x.GetExternalPrductPage(It.IsAny<string>())).ReturnsAsync(It.IsAny<string>()).Verifiable();
            mockExternalProductService.Setup(x => x.ParsePrice(It.IsAny<string>())).Returns(parsedPrice).Verifiable();
            mockMailService.Setup(x => x.ProductOutOfStock(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            var priceComparisonJob = new PriceComparisonJob(mockProductService.Object, mockExternalProductService.Object, mockMailService.Object);

            //Act
            await priceComparisonJob.Compare();
            //Assert

            mockProductService.Verify();
        }
    }
}
