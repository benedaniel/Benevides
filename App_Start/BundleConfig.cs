using System.Web;
using System.Web.Optimization;

namespace FT.Web.Site
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery/*.js"));

            bundles.Add(new ScriptBundle("~/bundles/agencia").Include(
                        "~/Scripts/AgenciaMapaModel.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-and-validations").Include(new string[]{
                        "~/Scripts/jquery/jquery-2.0.3.js",
                        "~/Scripts/jquery/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery/jquery.validate.js",
                        "~/Scripts/jquery/jquery.validate.unobtrusive.js",
                        "~/Scripts/jquery/jquery.maskedinput.js"}));

            bundles.Add(new ScriptBundle("~/bundles/Site").Include(
                        "~/Scripts/Site.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/jquery").Include(
                        "~/Content/jquery/*.css"));
        }
    }
}