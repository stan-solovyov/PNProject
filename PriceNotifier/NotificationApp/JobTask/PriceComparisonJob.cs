using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Services.PriceHistoryService;
using BLL.Services.ProductService;
using Domain.Entities;
using log4net;
using Messages;
using NotificationApp.Interfaces;
using Quartz;

namespace NotificationApp.JobTask
{
    public class PriceComparisonJob : IJob
    {
        private readonly IProductService _productService;
        private readonly IExternalProductService _externalProductService;
        private readonly IMailService _mailService;
        private readonly IPriceHistoryService _priceHistoryService;
        private readonly IMessageService _messageService;
        readonly ILog log = LogManager.GetLogger(typeof(Program));
        public PriceComparisonJob(IProductService productService, IExternalProductService externalProductService, IMailService mailService, IPriceHistoryService priceHistoryService, IMessageService messageService)
        {
            _productService = productService;
            _externalProductService = externalProductService;
            _priceHistoryService = priceHistoryService;
            _mailService = mailService;
            _messageService = messageService;
        }

        public void Execute(IJobExecutionContext context)
        {
            Compare().Wait();
        }

        public async Task Compare()
        {
            var products = await _productService.GetTrackedItems();
            var updatedPriceList = new List<UpdatedPrice>();
            log.Info("Loging began at " + DateTime.Now);

            if (products != null)
            {
                foreach (var product in products)
                {
                    var providersProductInfos = product.ProvidersProductInfos;
                    foreach (var providersProductInfo in providersProductInfos)
                    {
                        try
                        {
                            var priceFromDB = providersProductInfo.MinPrice;
                            var productLink = providersProductInfo.Url;
                            var priceFromSite = await _externalProductService.ParsePrice(productLink);

                            if (priceFromDB == priceFromSite)
                            {
                                continue;
                            }

                            var emails = product.UserProducts.Select(m => m.User.Email).ToList();
                            var userIds = product.UserProducts.Select(a => a.UserId).ToList();
                            if (priceFromDB == 0 && priceFromSite != 0)
                            {
                                    await _mailService.ProductAvailable(emails, productLink, product.Name, priceFromSite);
                            }

                            if (priceFromDB > priceFromSite && priceFromSite != 0)
                            {
                                    await _mailService.PriceFromDbHigher(emails, productLink, product.Name, priceFromDB,priceFromSite);
                            }

                            if (priceFromDB != 0 && priceFromDB < priceFromSite)
                            {
                                    await _mailService.PriceFromSiteHigher(emails, productLink, product.Name, priceFromDB,priceFromSite);
                            }

                            if (priceFromDB != 0 && priceFromSite == 0)
                            {
                                    await _mailService.ProductOutOfStock(emails, productLink, product.Name);
                            }

                            foreach (var userId in userIds)
                            {
                                var up = new UpdatedPrice
                                {
                                    Price = priceFromSite,
                                    ProductId = product.ProductId,
                                    UserId = userId
                                };
                                updatedPriceList.Add(up);
                            }

                            await _priceHistoryService.Create(new PriceHistory
                            {
                                ProviderId = providersProductInfo.ProviderId,
                                Date = DateTime.Now,
                                NewPrice = priceFromSite,
                                OldPrice = priceFromDB
                            });
                            providersProductInfo.MinPrice = priceFromSite;
                            await _productService.Update(product);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error message:" + ex.Message);
                        }
                    }
                }

                if (updatedPriceList.Count != 0)
                {
                    _messageService.SendPriceUpdate(new UpdatedPricesMessage
                    {
                        UpdatedPricesList = updatedPriceList
                    });
                }
            }
        }
    }
}
