using System.Linq;
using System.Threading.Tasks;
using BLL.Services.ProductService;
using NotificationApp.Interfaces;
using Quartz;

namespace NotificationApp
{
    public class PriceComparisonJob:IJob
    {
        private readonly IProductService _productService;
        private readonly IExternalProductService _externalProductService;
        private readonly IMailService _mailService;
        private double _priceFromSite;
        public PriceComparisonJob(IProductService productService, IExternalProductService externalProductService, IMailService mailService)
        {
            _productService = productService;
            _externalProductService = externalProductService;
            _mailService = mailService;
        }

        public async Task Compare()
        {
            var products = await _productService.GetTrackedItems();

            if (products != null)
            {
                foreach (var product in products)
                {
                    var html = await _externalProductService.GetExternalPrductPage(product.Url);
                    _priceFromSite = _externalProductService.ParsePrice(html);

                    if (product.Price == _priceFromSite)
                    {
                        continue;
                    }

                    var emails = product.UserProducts.Where(c => c.ProductId == product.ProductId).Select(m => m.User.Email).ToList();
                    if (product.Price == 0 && _priceFromSite != 0)
                    {
                        foreach (var email in emails)
                        {
                            _mailService.ProductAvailable(email, product.Url, product.Name, _priceFromSite);
                        }
                    }

                    if (product.Price > _priceFromSite && _priceFromSite != 0)
                    {
                        foreach (var email in emails)
                        {
                            _mailService.PriceFromDbHigher(email, product.Url, product.Name, product.Price,_priceFromSite);
                        }
                    }

                    if (product.Price != 0 && product.Price < _priceFromSite)
                    {
                        foreach (var email in emails)
                        {
                            _mailService.PriceFromSiteHigher(email, product.Url, product.Name, product.Price,_priceFromSite);
                        }
                    }

                    if (product.Price != 0 && _priceFromSite == 0)
                    {
                        foreach (var email in emails)
                        {
                            _mailService.ProductOutOfStock(email, product.Url, product.Name);
                        }
                    }

                    product.Price = _priceFromSite;
                    await _productService.Update(product);
                }
            }
        }

        public  void Execute(IJobExecutionContext context)
        {
            Compare().Wait();
        }
    }
}
