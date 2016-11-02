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
    [DisallowConcurrentExecution]
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
            var retryCount = 3;
            log.Info("Loging began at " + DateTime.Now);

            if (products != null)
            {
                foreach (var product in products)
                {
                    var providersProductInfos = product.ProvidersProductInfos;
                    foreach (var providersProductInfo in providersProductInfos)
                    {
                        for (int currentRetry = 0; currentRetry < retryCount; currentRetry++)
                        {
                            try
                            {
                                var priceFromDB = providersProductInfo.MinPrice;
                                var productLink = providersProductInfo.Url;
                                var priceFromSite = await _externalProductService.ParsePrice(productLink);
                                var emails = product.UserProducts.Select(m => m.User.Email).ToList();
                                var userIds = product.UserProducts.Select(a => a.UserId).ToList();

                                if (priceFromDB.HasValue && priceFromSite.HasValue)
                                {
                                    if (priceFromDB.Value == priceFromSite.Value)
                                    {
                                        break;
                                    }

                                    if (priceFromDB.Value > priceFromSite.Value)
                                    {
                                        await _mailService.PriceFromDbHigher(emails, productLink, product.Name,
                                                priceFromDB.Value, priceFromSite.Value);
                                    }

                                    if (priceFromDB.Value < priceFromSite.Value)
                                    {
                                        await
                                            _mailService.PriceFromSiteHigher(emails, productLink, product.Name,
                                                priceFromDB.Value, priceFromSite.Value);
                                    }
                                }

                                if (priceFromDB == null && priceFromSite == null)
                                {
                                    break;
                                }

                                if (priceFromDB == null && priceFromSite != null)
                                {
                                    await _mailService.ProductAvailable(emails, productLink, product.Name,
                                            priceFromSite.Value);
                                }

                                if (priceFromDB != null && priceFromSite == null)
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
                            await Task.Delay(3000);
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
