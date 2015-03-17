using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Services;
using System.IdentityModel.Services.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Claims;

namespace RelyingParty1.Controllers
{
    public class UserInfo
    {
        public string NameIdentifier { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<string> Roles { get; set; }
        public string EmailAddress { get; set; }
        public string Sid { get; set; }
        public List<string> Groups{ get; set; }
    }

    public class UserController : Controller
    {
        public ActionResult Index()
        {
            var identity = ((System.Security.Claims.ClaimsIdentity) Thread.CurrentPrincipal.Identity);

            var result = new Dictionary<string, string>();
            // Access claims
            var count = 0;
            foreach (var claim in identity.Claims)
            {
                var key = result.ContainsKey(claim.Type) ? claim.Type + (count++).ToString() : claim.Type;
                result.Add(key, claim.Value);
            }

            ViewBag.Claims = result;
            return View();
        }

        public ActionResult Logout()
        {
            // Load Identity Configuration
            FederationConfiguration config = FederatedAuthentication.FederationConfiguration;

            // Sign out of WIF.
            WSFederationAuthenticationModule.FederatedSignOut(new Uri(ConfigurationManager.AppSettings["ida:Issuer"]), new Uri(config.WsFederationConfiguration.Realm));

            return View();
        }
    }
}
