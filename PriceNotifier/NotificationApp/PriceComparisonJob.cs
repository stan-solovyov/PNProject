using System.Globalization;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using BLL.Services.ProductService;

namespace NotificationApp
{
    public class PriceComparisonJob
    {
        private readonly IParser _parseService;
        private readonly IProductService _productService;
        private double _priceFromSite, _priceFromDb;
        public PriceComparisonJob(IParser service, IProductService productService)
        {
            _parseService = service;
            _productService = productService;

        }

        public async Task Compare()
        {
            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com")
            {
                Port =  587,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("stanislav.soloview@gmail.com", "1691811qwe"),
                EnableSsl = true,
                Timeout = 20000
            };

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("price_notifier@gmail.com");
            mail.To.Add("drow1@mail.ru");
            mail.IsBodyHtml = true;

            var products = await _productService.GetTrackedItems();
     
            if (products != null)
            {
                foreach (var product in products)
                {
                    using (var client = new HttpClient())
                    {
                        var html = await client.GetStringAsync(product.Url);
                        _priceFromSite = _parseService.Parse(html);
                    }

                    _priceFromDb = 0;
                    if (product.Price != null)
                    {
                        _priceFromDb = double.Parse(product.Price.Replace(".", ","));
                    }
                    var newPrice = _priceFromSite.ToString(CultureInfo.CurrentCulture);

                    if (string.IsNullOrEmpty(product.Price) && _priceFromSite != 0)
                    {
                        product.Price = _priceFromSite.ToString(CultureInfo.CurrentCulture);
                        await _productService.Update(product);

                        mail.Subject = "Product is in stock";
                        mail.Body = $"The item <a href = '{product.Url}'> {product.Name} </a>  is available for purchase for {_priceFromSite} BYR.";
                        smtpServer.Send(mail);
                    }

                    if (_priceFromDb > _priceFromSite)
                    {
                        product.Price = newPrice;
                        await _productService.Update(product);

                        mail.Subject = "Price drop";
                        mail.Body = $"The price for <a href = '{product.Url}'> {product.Name} </a>  has decreased on {(_priceFromDb - _priceFromSite)} BYR.";
                        smtpServer.Send(mail);
                    }

                    if (_priceFromDb < _priceFromSite)
                    {
                        product.Price = newPrice;
                        await _productService.Update(product);

                        mail.Subject = "Price increase";
                        mail.Body = $"The price for <a href = '{product.Url}'> {product.Name} </a>  has increased on {(_priceFromSite - _priceFromDb)} BYR.";
                        smtpServer.Send(mail);
                    }

                    if (_priceFromDb!= 0 && _priceFromSite==0)
                    {
                        product.Price = newPrice;
                        await _productService.Update(product);

                        mail.Subject = "Product is out of sale";
                        mail.Body = $"Unfortunately the <a href = '{product.Url}'> {product.Name} </a>  is out of sale.";
                        smtpServer.Send(mail);
                    }
                }
            }
        }
    }
}
