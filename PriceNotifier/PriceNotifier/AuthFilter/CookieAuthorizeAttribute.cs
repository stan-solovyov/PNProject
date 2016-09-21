using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.EF;

namespace PriceNotifier.AuthFilter
{
    public class CookieAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var req = httpContext.Request.Cookies;

                var tokenTransferred = req["Token"].Value;
                //check for null
                if (!string.IsNullOrEmpty(tokenTransferred))
                {
                    UserContext db = new UserContext();

                    var userFound = db.Users.Any(c => c.Token == tokenTransferred);

                    var user = db.Users.FirstOrDefault(c => c.Token == tokenTransferred);

                    if (user != null)
                    {
                        var owinContext = httpContext.Request.GetOwinContext();
                        owinContext.Set("userId", user.UserId);
                    }

                    return userFound;
                }

            return false;
        }
    }
}