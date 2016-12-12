using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Domain.EF;
using System.Web.Http.Controllers;

namespace PriceNotifier.AuthFilter
{
    public class TokenAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] _userRoles;
        private string requestTokenHeader = "X-Auth";
        public TokenAuthorizeAttribute(params string[] UserRoles)
        {
            _userRoles = UserRoles;
        }
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var req = actionContext.Request.Headers;

            if (req.Contains(requestTokenHeader))
            {
                var tokenTransferred = req.GetValues(requestTokenHeader).First();
                //check for null
                if (!string.IsNullOrEmpty(tokenTransferred))
                {
                    UserContext db = new UserContext();

                    var user = db.Users.FirstOrDefault(c => c.Token == tokenTransferred);

                    if (user != null)
                    {
                        var owinContext = actionContext.Request.GetOwinContext();
                        owinContext.Set("userId", user.Id);
                        if (_userRoles != null)
                        {
                            var roles = user.UserRoles.Select(c => c.Role.Name).ToArray();
                            foreach (var role in roles)
                            {
                                if (_userRoles.Any(s => s.Contains(role)))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}