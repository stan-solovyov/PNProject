using System.Net.Http;
using System.Web.Http;

namespace PriceNotifier.Infrostructure
{
    public class BaseApiController:ApiController
    {
        protected static int GetCurrentUserId(HttpRequestMessage request)
        {
            var owinContext = request.GetOwinContext();
            var userId = owinContext.Get<int>("userId");

            return userId;
        }
    }
}