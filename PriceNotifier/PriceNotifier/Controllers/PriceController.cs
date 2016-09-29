using System.Net;
using System.Web.Http;
using PriceNotifier.Hubs;
using PriceNotifier.Models;

namespace PriceNotifier.Controllers
{
    public class PriceController : SignalRBase<PriceHub>
    {
        // POST: api/Price
        public void Post(UpdatedPrice p)
        {
            if (p == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            //notify all connected clients
            Hub.Clients.Group("Clients").updatePrice(p);
        }
    }
}
