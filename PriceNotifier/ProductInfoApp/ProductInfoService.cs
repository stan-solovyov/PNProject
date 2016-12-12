using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Services;
using BLL.Services.ProductService;
using Domain.Entities;
using Rhino.ServiceBus;
using Messages;
using PriceNotifier.Infrostructure;

namespace ProductInfoApp
{
    class ProductInfoService : IProductInfoService, ConsumerOf<ProductMessage>
    {
        private readonly IEnumerable<IProviderProductInfoParser> _ppiParsers;
        private readonly IProductService _productService;
        private readonly IElasticService<Product> _elasticProductService;

        public ProductInfoService(IEnumerable<IProviderProductInfoParser> ppiParsers, IProductService productService, IElasticService<Product> elasticProductService)
        {
            _ppiParsers = ppiParsers;
            _productService = productService;
            _elasticProductService = elasticProductService;
        }
        public void AddProductInfo(int productId)
        {
            var tasks = new List<Task<ProvidersProductInfo>>();
            var productfound = _productService.GetById(productId).Result;
            foreach (var parser in _ppiParsers)
            {
                var providersProductInfo = parser.GetProvidersProductInfo(productfound.Name);
                tasks.Add(providersProductInfo);
            }

            if (tasks.Count != 0)
            {
                var providerProductInfoes = Task.WhenAll(tasks).Result;

                foreach (var providersProductInfo in providerProductInfoes)
                {
                    if (providersProductInfo != null)
                    {
                        productfound.ProvidersProductInfos.Add(providersProductInfo);
                    }
                }
                _productService.Update(productfound).Wait();
            }
            _elasticProductService.AddToIndex(productfound, productfound.Id);
        }

        public void Consume(ProductMessage message)
        {
            if (message != null)
            {
                AddProductInfo(message.ProductId);
            }
        }
    }
}
