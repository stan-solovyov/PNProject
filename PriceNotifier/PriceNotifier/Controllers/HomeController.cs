using OAuth2;
using OAuth2.Client;
using PriceNotifier.Models;
using System.Linq;
using System.Web.Mvc;
using Domain.EF;
using Domain.Entities;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PriceNotifier.Controllers
{
    public class HomeController : Controller
    {
        private readonly AuthorizationRoot _authorizationRoot;
        private UserContext db = new UserContext();

        public string GetHashString(string s)
        {
            //переводим строку в байт-массим  
            byte[] bytes = Encoding.Unicode.GetBytes(s);

            //создаем объект для получения средст шифрования  
            MD5CryptoServiceProvider csp = new MD5CryptoServiceProvider();

            //вычисляем хеш-представление в байтах  
            byte[] byteHash = csp.ComputeHash(bytes);
            string hash = string.Empty;

            //формируем одну цельную строку из массива  
            foreach (byte b in byteHash)
            {
                hash += $"{b:x2}";
            }

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
            if (Request.Cookies.AllKeys.Contains("Token"))
            {
                var token = Request.Cookies.Get("Token");
                if (token != null)
                {
                    ViewData["token"] = token.Value;
                }
                return View();
            }
            return RedirectToAction("Login");
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
                SocialNetworkUserId = a.Id,
                Token = ""
            };

            if (db.Users != null)
            {
                var userid = db.Users.FirstOrDefault(c => c.SocialNetworkUserId == user.SocialNetworkUserId);

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

            var userFound = db.Users.FirstOrDefault(c => c.SocialNetworkUserId == user.SocialNetworkUserId);
            if (userFound != null)
            {
                userFound.Token = GetHashString(user.Id + user.SocialNetworkName + user.SocialNetworkUserId);
                db.SaveChanges();
                HttpCookie cookie = new HttpCookie("Token");
                cookie.Value = userFound.Token;
                ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Home");
        }
        private IClient GetClient(string providerName)
        {
            return _authorizationRoot.Clients.First(c => c.Name == providerName);
        }
    }
}
