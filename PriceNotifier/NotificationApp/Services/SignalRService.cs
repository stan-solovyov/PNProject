using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NotificationApp.Interfaces;
using PriceNotifier.Models;

namespace NotificationApp.Services
{
    public class SignalRService : ISignalRService
    {
        public async Task SendPriceUpdate(List<UpdatedPrice> updatedPricelist)
        {
            var priceUri = "api/price";
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://localhost:59476/");
                await httpClient.PostAsJsonAsync(priceUri, updatedPricelist);
            }
        }
    }
}
