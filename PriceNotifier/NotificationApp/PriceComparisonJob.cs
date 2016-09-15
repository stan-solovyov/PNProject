using System;
using System.Threading.Tasks;
using BLL.Services.ProductService;

namespace NotificationApp
{
    public class PriceComparisonJob
    {
        private readonly IParser _parseService;
        private readonly IProductService _productService;

        public PriceComparisonJob(IParser service, IProductService productService)
        {
            _parseService = service;
            _productService = productService;

        }

        public async Task Compare()
        {
            var products = await _productService.GetByUserId(2);
     
            if (products != null)
            {
                foreach (var product in products)
                {
                    Console.WriteLine($"{product.Name}");
                }
            }
            
            Console.ReadLine();
        }

    }
}
