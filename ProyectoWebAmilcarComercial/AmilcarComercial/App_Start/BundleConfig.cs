using System.Web;
using System.Web.Optimization;

namespace AmilcarComercial
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/materialize").Include(
                      "~/Scripts/materialize.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/libraries").Include(
                      "~/Scripts/pace.js",
                      "~/Scripts/jquery.mCustomScrollbar.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryajax").Include(
                      "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/dropzone").Include(
                      "~/Scripts/dropzone.js"));

            bundles.Add(new ScriptBundle("~/bundles/contado").Include(
                      "~/Scripts/TransactionScripts/Contado.js"));

            bundles.Add(new ScriptBundle("~/bundles/compras").Include(
                      "~/Scripts/TransactionScripts/Compras.js"));

            bundles.Add(new ScriptBundle("~/bundles/apartado").Include(
                      "~/Scripts/TransactionScripts/Apartado.js"));

            bundles.Add(new ScriptBundle("~/bundles/credito").Include(
                      "~/Scripts/TransactionScripts/Credito.js"));

            bundles.Add(new ScriptBundle("~/bundles/devolucionCliente").Include(
                      "~/Scripts/TransactionScripts/DevolucionCliente.js"));

            bundles.Add(new ScriptBundle("~/bundles/devolucionProveedor").Include(
                      "~/Scripts/TransactionScripts/DevolucionProveedor.js"));

            bundles.Add(new ScriptBundle("~/bundles/public").Include(
                      "~/Scripts/public.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/normalize.css",
                      "~/Content/css/materialize.css",
                      "~/Content/css/site.css"));

            bundles.Add(new StyleBundle("~/Content/public").Include(
                      "~/Content/css/normalize.css",
                      "~/Content/css/materialize.css",
                      "~/Content/css/public.css"));

            bundles.Add(new StyleBundle("~/Content/fonts").Include(
                      "~/Content/css/fonts.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/Content/libraries").Include(
                      "~/Content/css/pace-theme-flash.css",
                      "~/Content/css/jquery.mCustomScrollbar.css"));

            bundles.Add(new StyleBundle("~/Content/dropzone").Include(
                      "~/Content/css/dropzone.css"));
        }
    }
}
