using System.Web;
using System.Web.Mvc;

namespace PriceNotifier.Infrostructure
{
    public class BaseMVCController:Controller
    {
        protected static int GetCurrentUserId(HttpRequestBase request)
        {
            var owinContext = request.GetOwinContext();
            var userId = owinContext.Get<int>("userId");
        
            return userId;
        }
    }
}