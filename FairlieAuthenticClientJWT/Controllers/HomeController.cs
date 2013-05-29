using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Tokens.JWT;

namespace FairlieAuthenticClientJWT.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Title = "Home";
            if (Request.IsAuthenticated)
            {
                ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

                return View();
            }
            else
            {
                ViewBag.MetaDataScript = ConfigurationManager.AppSettings["fa:LoginProviders"];
                return View("~/Views/Account/Login.cshtml");
            }
        }

        public ActionResult About()
        {
            BootstrapContext bc = ClaimsPrincipal.Current.Identities.First().BootstrapContext as BootstrapContext;
            JWTSecurityToken jwt = bc.SecurityToken as JWTSecurityToken;

            string rawToken = jwt.RawData;
            string api = ConfigurationManager.AppSettings["fa:APIEndPoint"];

            HttpWebRequest request = WebRequest.Create(api + "values/5") as HttpWebRequest;
            request.Method = "GET";
            request.Headers["Authorization"] = "Bearer " + rawToken;
            request.ContentType = "application/json";

            string responseTxt = String.Empty;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                var reader = new StreamReader(response.GetResponseStream());
                responseTxt = reader.ReadToEnd();
                response.Close();
            }

            ViewBag.Message =
                api + " : " + responseTxt;

            return View();
        }
        
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        [AllowAnonymous]
        public ActionResult Debug()
        {
            string api = ConfigurationManager.AppSettings["fa:APIEndPoint"];
            api += "<br/>" + ConfigurationManager.AppSettings["fa:APIEndPoint"];

            //HttpWebRequest request = WebRequest.Create(api + "debug") as HttpWebRequest;
            //request.Method = "GET";
            //request.ContentType = "application/json";

            //string responseTxt = String.Empty;
            //using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            //{
            //    var reader = new StreamReader(response.GetResponseStream());
            //    responseTxt = reader.ReadToEnd();
            //    response.Close();
            //}

            ViewBag.Message = api;

            return View();
        }
    }
}
