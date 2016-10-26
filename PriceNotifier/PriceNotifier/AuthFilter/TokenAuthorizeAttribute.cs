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
        public TokenAuthorizeAttribute(params string[] UserRoles)
        {
            _userRoles = UserRoles;
        }
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

                    var user = db.Users.FirstOrDefault(c => c.Token == tokenTransferred);

                    if (user != null)
                    {
                        var owinContext = actionContext.Request.GetOwinContext();
                        owinContext.Set("userId", user.UserId);
                        var roles = user.UserRoles.Select(c => c.Role.Name).ToArray();

                        foreach (var role in roles)
                        {
                            if (_userRoles.Any(s => s.Contains(role)) || _userRoles.Length == 0)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}