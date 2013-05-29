using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.IdentityModel.Tokens.JWT;

namespace FairlieAuthentic
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
    }

    internal class TokenValidationHandler : DelegatingHandler
    {
        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            IEnumerable<string> authzHeaders;
            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
            {
                return false;
            }
            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode;
            string token;
            var authorisationHeader = request.Headers.Authorization;

            //if (!TryRetrieveToken(request, out token))
            //{
            //    statusCode = HttpStatusCode.Unauthorized;
            //    return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode));
            //}
            try
            {
                //X509Store store = new X509Store(StoreName.TrustedPeople, StoreLocation.LocalMachine);
                //store.Open(OpenFlags.ReadOnly);
                //X509Certificate2 cert = store.Certificates.Find(
                //    X509FindType.FindByThumbprint,
                //    "F9D1CF40133449D89DE8236BF142BBACE92EC6E0",
                //    false)[0];
                //store.Close();

                //JWTSecurityTokenHandler tokenHandler = new JWTSecurityTokenHandler();
                //TokenValidationParameters validationParameters = new TokenValidationParameters()
                //    {
                //        AllowedAudience = ConfigurationManager.AppSettings["fa:AllowedAudience"],
                //        ValidIssuer = "https://fairlieauthentic.accesscontrol.windows.net/",
                //        SigningToken = new X509SecurityToken(cert)
                //    };

                //ClaimsPrincipal cp = tokenHandler.ValidateToken(token, validationParameters);
                //Thread.CurrentPrincipal = cp;
                //HttpContext.Current.User = cp;

                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException e)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (Exception)
            {
                statusCode = HttpStatusCode.InternalServerError;
            }
            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode));
        }

    }
}