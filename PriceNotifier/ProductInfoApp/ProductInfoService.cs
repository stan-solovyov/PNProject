using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Services;
using BLL.Services.ProductService;
using Domain.Entities;
using Rhino.ServiceBus;
using Messages;

namespace ProductInfoApp
{
    class ProductInfoService : IProductInfoService, ConsumerOf<ProductMessage>
    {
        private readonly IEnumerable<IProviderProductInfoParser> _ppiParsers;
        private readonly IProductService _productService;

        public ProductInfoService(IEnumerable<IProviderProductInfoParser> ppiParsers, IProductService productService)
        {
            _ppiParsers = ppiParsers;
            _productService = productService;
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
