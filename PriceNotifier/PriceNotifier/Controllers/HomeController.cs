using OAuth2;
using OAuth2.Client;
using PriceNotifier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Domain.EF;
using Domain.Entities;
using System.Security.Cryptography;
using System.Text;
using PriceNotifier.AuthFilter;
using OAuth2.Models;
using System.Net;

namespace PriceNotifier.Controllers
{
    public class HomeController : Controller
    {
        private readonly AuthorizationRoot _authorizationRoot;

        //private static string ProviderName = "";

        private UserContext db = new UserContext();

       
        public string GetHashString(string s)
        {
            //переводим строку в байт-массим  
            byte[] bytes = Encoding.Unicode.GetBytes(s);

            //создаем объект для получения средст шифрования  
            MD5CryptoServiceProvider CSP =
                new MD5CryptoServiceProvider();

            //вычисляем хеш-представление в байтах  
            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;

            //формируем одну цельную строку из массива  
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return hash;
        }
       

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="authorizationRoot">The authorization manager.</param>
        public HomeController(AuthorizationRoot authorizationRoot)
        {
            _authorizationRoot = authorizationRoot;
           
        }

        public ActionResult Index()
        {
            
            return View();
        }


        /// <summary>
        /// Renders home page with login link.
        /// </summary>
        public ActionResult Login()
        {
            var model = _authorizationRoot.Clients.Select(client => new LoginInfoModel
            {
                ProviderName = client.Name,
                LoginLinkUri = client.GetLoginLinkUri()
            });
            return View(model);
        }


        /// <summary>
        /// Renders information received from authentication service.
        /// </summary>
        
        public ActionResult Auth(string providerName)
        {
           
            var a = GetClient(providerName).GetUserInfo(Request.QueryString);

            User user = new User
            {
                SocialNetworkName = a.ProviderName,
                Username = a.FirstName,
                UserId = a.Id,
                Token = ""
            };


            if (db.Users != null)
            {
                var userid = db.Users.FirstOrDefault(c => c.UserId == user.UserId);

                if (userid == null)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                else
                {
                    userid.Token = user.Token;
                    userid.SocialNetworkName = user.SocialNetworkName;
                    userid.Username = user.Username;
                    db.SaveChanges();
                }
            }

            var userFound = db.Users.FirstOrDefault(c => c.UserId == user.UserId);
            if (userFound != null)
            {
                userFound.Token = GetHashString(user.Id.ToString() + user.SocialNetworkName + user.UserId);
                db.SaveChanges();

                return View(userFound);
            }
            else
            {
                return View();
            }
        }

        private IClient GetClient(string providerName)
        {
            return _authorizationRoot.Clients.First(c => c.Name == providerName);
        }
    }
}
