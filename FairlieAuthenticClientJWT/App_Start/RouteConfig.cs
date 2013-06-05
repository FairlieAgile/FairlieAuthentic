using System.Web.Mvc;
using System.Web.Routing;

namespace FairlieAuthenticClientJWT.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            routes.MapRoute(
                name: "Account",
                url: "Account/Login",
                defaults: new { controller = "Account", action = "Login" }
            );

routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}