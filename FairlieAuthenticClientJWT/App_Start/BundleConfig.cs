using System.Web;
using System.Web.Optimization;

namespace FairlieAuthenticClientJWT
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            const string jqueryCdnPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.min.js";

            bundles.Add(new ScriptBundle("~/bundles/jquery",
                        jqueryCdnPath).Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Main.css","~/Content/normalize.css"));

        }
    }
}