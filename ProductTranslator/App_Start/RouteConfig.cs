using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProductTranslator
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

       
            routes.MapRoute(
               name: "Homepage",
               url: "",
               defaults: new { controller = "Home", action = "Index" }
           );
             routes.MapRoute(
             name: "SearchProduct",
             url: "search",
             defaults: new { controller = "Home", action = "SearchProduct"}
           );
            routes.MapRoute(
            name: "ShowForm",
            url: "datasheet/{productId}",
            defaults: new { controller = "Product", action = "Index", productId = String.Empty }
          );
            routes.MapRoute(
             name: "TranslateProduct",
             url: "translate",
             defaults: new { controller = "Product", action = "TranslateProduct"}
          );




            

        }
    }
}
