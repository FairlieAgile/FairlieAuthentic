// 
// Code generated by Identity and Access VS Package
// 

using System.Globalization;
using System.Web.Mvc;

namespace HrdAuthentication.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewBag.MetaDataScript = "https://FairlieAuthentic.accesscontrol.windows.net/v2/metadata/identityProviders.js?protocol=wsfederation&realm=http://fairlieauthenticclient/&version=1.0&callback=ShowSigninPage";
            return View("~/Views/Account/Login0.cshtml");
        }
    }
}