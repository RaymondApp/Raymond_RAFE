using System.Web;
using System.Web.Optimization;

namespace RAFE
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/js/jquery/jquery-{version}.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Content/js/jquery/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/js/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/js/bootstrap.min.js",
                      "~/Content/js/respond.min.js",
                      "~/Content/js/validator.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/bootstrap.css",
                      "~/Content/css/font-awesome.min.css",
                      "~/Content/css/AdminLTE.css",
                      "~/Content/css/_all-skins.min.css",
                      "~/Content/css/datatables/jquery.dataTables.css",
                      "~/Content/css/select2.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/BasicJS").Include(
                     "~/Content/js/sweetalert.min.js",
                     "~/Content/js/adminlte.js",
                     "~/Content/js/datatables/jquery.dataTables.min.js",
                     "~/Content/js/datatables/dataTables.fixedHeader.js",
                     "~/Content/js/main.js",
                     "~/Content/js/common.js",
                     "~/Content/js/fastclick.js",
                     "~/Content/js/select2.full.min.js",
                     "~/Content/js/custom.js"
                ));

            bundles.Add(new StyleBundle("~/Content/AutoCompletecss").Include(
                "~/Content/css/jquery-ui.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/AutoCompleteJS").Include(
                "~/Content/js/jquery-ui.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/jqueryAjaxUnobtrusive").Include(
            "~/Content/js/jquery/jquery.unobtrusive-ajax.min.js"
            ));
        }
    }
}
