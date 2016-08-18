using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Domain.EF;
using System.Web.Http.Controllers;
using Domain.Entities;

namespace PriceNotifier.AuthFilter
{
    public class MyAuthorizeAttribute: AuthorizeAttribute
    {
       
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var req = actionContext.Request.Headers;
           
            if (req.Contains("X-Auth"))
            {
                var tokenTransferred = req.GetValues("X-Auth").First();
                //check for null
                if (!string.IsNullOrEmpty(tokenTransferred))
                {
                     UserContext db = new UserContext();

                    var userFound = db.Users.Any(c => c.Token == tokenTransferred);

                    var user = db.Users.FirstOrDefault(c => c.Token == tokenTransferred);

                    if (user != null)
                    {
                        var owinContext = actionContext.Request.GetOwinContext();
                        owinContext.Set("userId", user.Id);
                    }

                    return userFound;
                }
            }

            return false;
        }
    }
}