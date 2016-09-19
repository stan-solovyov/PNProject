using System.Threading.Tasks;
using BLL.Services.ProductService;
using NotificationApp.Services;

namespace NotificationApp
{
    public class PriceComparisonJob
    {
        private readonly IProductService _productService;
        private readonly ExternalProductService _externalProductService;
        private readonly MailService _mailService;
        private double _priceFromSite;
        public PriceComparisonJob(IProductService productService, ExternalProductService externalProductService, MailService mailService)
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

                    if (product.Price==0 && _priceFromSite != 0)
                    {
                        _mailService.ProductAvailable("drow1@mail.ru",product.Url,product.Name,_priceFromSite);
                    }

                    if (product.Price > _priceFromSite && _priceFromSite!=0)
                    {
                        _mailService.PriceFromDbHigher("drow1@mail.ru",product.Url,product.Name,product.Price,_priceFromSite);
                    }

                    if (product.Price != 0 && product.Price < _priceFromSite)
                    {
                        _mailService.PriceFromSiteHigher("drow1@mail.ru", product.Url, product.Name, product.Price, _priceFromSite);
                    }

                    if (product.Price != 0 && _priceFromSite==0)
                    {
                        _mailService.ProductOutOfStock("drow1@mail.ru", product.Url, product.Name);
                    }

                    product.Price = _priceFromSite;
                    await _productService.Update(product);
                }
            }
        }
    }
}
