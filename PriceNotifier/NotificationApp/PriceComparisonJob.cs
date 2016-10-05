using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Services.PriceHistoryService;
using BLL.Services.ProductService;
using Domain.Entities;
using NotificationApp.Interfaces;
using PriceNotifier.Models;
using Quartz;

namespace NotificationApp
{
    public class PriceComparisonJob : IJob
    {
        private readonly IProductService _productService;
        private readonly IExternalProductService _externalProductService;
        private readonly IMailService _mailService;
        private readonly IPriceHistoryService _priceHistoryService;
        private readonly ISignalRService _signalRService;
        public PriceComparisonJob(IProductService productService, IExternalProductService externalProductService, IMailService mailService,IPriceHistoryService priceHistoryService,ISignalRService signalRService)
        {
            _productService = productService;
            _externalProductService = externalProductService;
            _priceHistoryService = priceHistoryService;
            _mailService = mailService;
            _signalRService = signalRService;
        }

        public void Execute(IJobExecutionContext context)
        {
            Compare().Wait();
        }

        public async Task Compare()
        {
            var products = await _productService.GetTrackedItems();
            var updatedPriceList = new List<UpdatedPrice>();

            if (products != null)
            {
                foreach (var product in products)
                {
                    var html = await _externalProductService.GetExternalPrductPage(product.Url);
                    var _priceFromSite = _externalProductService.ParsePrice(html);

                    if (product.Price == _priceFromSite)
                    {
                        continue;
                    }

                    var emails = product.UserProducts.Where(c => c.ProductId == product.ProductId).Select(m => m.User.Email).ToList();
                    var userIds = product.UserProducts.Where(c => c.Product == product).Select(a => a.UserId);
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
                            _mailService.PriceFromDbHigher(email, product.Url, product.Name, product.Price, _priceFromSite);
                        }
                    }

                    if (product.Price != 0 && product.Price < _priceFromSite)
                    {
                        foreach (var email in emails)
                        {
                            _mailService.PriceFromSiteHigher(email, product.Url, product.Name, product.Price, _priceFromSite);
                        }
                    }

                    if (product.Price != 0 && _priceFromSite == 0)
                    {
                        foreach (var email in emails)
                        {
                            _mailService.ProductOutOfStock(email, product.Url, product.Name);
                        }
                    }

                    foreach (var userId in userIds)
                    {
                        var up = new UpdatedPrice
                        {
                            Price = _priceFromSite,
                            ProductId = product.ProductId,
                            UserId = userId
                        };
                        updatedPriceList.Add(up);
                    }

                    await _priceHistoryService.Create(new PriceHistory
                    {
                        ProductId = product.ProductId,
                        Date = DateTime.Now,
                        NewPrice = _priceFromSite,
                        OldPrice = product.Price
                    });
                    product.Price = _priceFromSite;
                    await _productService.Update(product);
                }

                if (updatedPriceList.Count != 0)
                {
                    await _signalRService.SendPriceUpdate(updatedPriceList);
                }
            }
        }
    }
}
