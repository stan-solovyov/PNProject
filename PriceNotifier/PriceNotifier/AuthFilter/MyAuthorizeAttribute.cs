using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using OAuth2.Client;
using Domain.EF;
using System.Web.Http.Controllers;
using Domain.Entities;

namespace PriceNotifier.AuthFilter
{
    public class MyAuthorizeAttribute: AuthorizeAttribute
    {
        public MyAuthorizeAttribute() { }

        private UserContext db = new UserContext();

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var req = actionContext.Request.Headers;

            if (req.Contains("X-Auth"))
            {
                var tokenTransferred = req.GetValues("X-Auth").First();
                //check for null
                if (!string.IsNullOrEmpty(tokenTransferred))
                {
                    var userFound = db.Users.Any(c => c.Token == tokenTransferred);

                    return userFound;
                }
            }

            return false;
        }
    }
}