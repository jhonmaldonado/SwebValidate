using System.Web;
using System.Web.Optimization;

namespace SwebValidate
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/login").Include(
                "~/Content/login/main.css", 
                "~/Content/login/util.css",
                "~/Content/login/material-design-iconic-font.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/validation").Include(
                "~/Scripts/funciones.js",
                "~/Scripts/filereader.js",
                "~/Scripts/qrcodelib.js",
                "~/Scripts/webcodecamjs.js"));
        }
    }
}
