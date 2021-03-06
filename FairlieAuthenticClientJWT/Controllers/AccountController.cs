// 
// Code generated by Identity and Access VS Package
// 

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web.Mvc;
using FairlieAuthenticClientJWT.Models;
using Microsoft.IdentityModel.Tokens.JWT;

namespace FairlieAuthenticClientJWT.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            return View("Login");
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewBag.MetaDataScript = ConfigurationManager.AppSettings["fa:LoginProviders"];
            return View("~/Views/Account/Login.cshtml");
        }
        
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (Request.IsAuthenticated)
            {
                using (var client = new HttpClient())
                {
                    BootstrapContext bc = ClaimsPrincipal.Current.Identities.First().BootstrapContext as BootstrapContext;
                    JWTSecurityToken jwt = bc.SecurityToken as JWTSecurityToken;

                    string rawToken = jwt.RawData;
                    string api = ConfigurationManager.AppSettings["fa:APIEndPoint"];

                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + rawToken);

                    var productDetailUrl = new Uri(api + "customer/5");
                    var model = client
                        .GetAsync(productDetailUrl)
                        .Result
                        .Content.ReadAsAsync<Customer>().Result;

                    //ViewBag.role = model.Email;
                    return View();
                }
            }
            else
            {
                ViewBag.MetaDataScript = ConfigurationManager.AppSettings["fa:LoginProviders"];
                return View("~/Views/Account/Login.cshtml");
            }
        }

        [AllowAnonymous]
        public JsonResult AddCustomer(Customer customer)
        {
            using (var client = new HttpClient())
            {
                BootstrapContext bc = ClaimsPrincipal.Current.Identities.First().BootstrapContext as BootstrapContext;
                JWTSecurityToken jwt = bc.SecurityToken as JWTSecurityToken;

                string rawToken = jwt.RawData;
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + rawToken);

                string api = ConfigurationManager.AppSettings["fa:APIEndPoint"];
                var productDetailUrl = api + "customer";
                var response = client.PostAsJsonAsync(productDetailUrl, customer).Result;

                string message = response.IsSuccessStatusCode ? "OK": "Failed";
                return Json(new { message });
            }
        }

        public void Logout()
        {
            string signoutUrl = new LogoutHandler().Signout();

            Response.Redirect(signoutUrl);
        }
    }
}
