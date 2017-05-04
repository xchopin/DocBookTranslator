using System.Web;
using System.Web.Optimization;

namespace ProductTranslator
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/assets/Scripts/jquery-3.2.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/assets/Scripts/bootstrap.js",
                      "~/assets/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/assets/Content/css").Include(
                      "~/assets/Content/app.css"));
        }
    }
}
