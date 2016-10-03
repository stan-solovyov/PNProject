using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using PriceNotifier.Hubs;
using PriceNotifier.Models;

namespace PriceNotifier.Controllers
{
    public class PriceController : SignalRBase<PriceHub>
    {
        // POST: api/Price
        public void Post(List<UpdatedPrice> updatedPrices)
        {
            if (updatedPrices.Count == 0)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            //notify all connected clients
            foreach (var up in updatedPrices)
            {
                Hub.Clients.Group("Clients").updatePrice(up);
            }
        }
    }
}
