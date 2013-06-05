using System;
using System.Collections.Generic;
using System.IdentityModel;
using System.IdentityModel.Services;
using System.IdentityModel.Services.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FairlieAuthenticClientJWT.App_Start;
using FairlieAuthenticClientJWT.Controllers;
using Microsoft.Ajax.Utilities;

namespace FairlieAuthenticClientJWT
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //FederatedAuthentication.FederationConfigurationCreated += OnFederationConfigurationCreated;
        }

        protected void Application_Error()
        {
            //var exception = Server.GetLastError();
            //var httpException = exception as HttpException;
            //Response.Clear();
            //Server.ClearError();

            //var routeData = new RouteData();
            //routeData.Values["controller"] = "Errors";
            //routeData.Values["action"] = "General";
            //routeData.Values["exception"] = exception;
            //Response.StatusCode = 500;

            //if (httpException != null)
            //{
            //    Response.StatusCode = httpException.GetHttpCode();
            //    switch (Response.StatusCode)
            //    {
            //        case 403:
            //            routeData.Values["action"] = "Http403";
            //            break;
            //        case 404:
            //            routeData.Values["action"] = "Http404";
            //            break;
            //    }
            //}

            //IController errorsController = new ErrorsController();
            //var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
            //errorsController.Execute(rc);
        }

        void OnFederationConfigurationCreated(object sender, FederationConfigurationCreatedEventArgs e)
        {
            var sessionTransforms = new List<CookieTransform>(
                new CookieTransform[]
            {
                new DeflateCookieTransform(),
                new RsaEncryptionCookieTransform(e.FederationConfiguration.ServiceCertificate),
                new RsaSignatureCookieTransform(e.FederationConfiguration.ServiceCertificate)
            });
            var sessionHandler = new SessionSecurityTokenHandler(sessionTransforms.AsReadOnly());

            e.FederationConfiguration
                .IdentityConfiguration
                .SecurityTokenHandlers
                .AddOrReplace(sessionHandler);
        }
    }
}