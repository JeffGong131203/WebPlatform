using System.Web;
using System.Web.Optimization;

namespace WebPlatform
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                                   "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/echarts").Include(
                                      "~/Scripts/echarts.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/AdminLTE/bower_components/Switch/bootstrapSwitch.js",
                      "~/AdminLTE/bower_components/jquery-slimscroll/jquery.slimscroll.min.js",
                      "~/AdminLTE/bower_components/Chart.js/Chart.js",
                      "~/AdminLTE/bower_components/fastclick/lib/fastclick.js",
                      "~/AdminLTE/bower_components/select2/dist/js/select2.full.min.js",
                      "~/AdminLTE/dist/js/adminlte.min.js",
                      "~/AdminLTE/dist/js/demo.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/AdminLTE/css").Include(
                      "~/AdminLTE/bower_components/font-awesome/css/font-awesome.min.css",
                      "~/AdminLTE/bower_components/Ionicons/css/ionicons.min.css",
                      "~/AdminLTE/bower_components/select2/dist/css/select2.min.css",
                      "~/AdminLTE/dist/css/AdminLTE.min.css",
                      "~/AdminLTE/dist/css/skins/_all-skins.min.css",
                      "~/AdminLTE/bower_components/Switch/bootstrapSwitch.css"));
        }
    }
}