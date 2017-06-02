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
            name: "TranslationByBrowsing",
            url: "redirect/translation",
            defaults: new { controller = "Home", action = "TranslationByBrowsing" }
          );
            routes.MapRoute(
            name: "ShowForm",
            url: "datasheet/{id}",
            defaults: new { controller = "Product", action = "Index", id = String.Empty }
          );
            routes.MapRoute(
            name: "TranslateProduct",
            url: "translate",
            defaults: new { controller = "Product", action = "TranslateProduct"}
          );
           routes.MapRoute(
           name: "editSearch",
           url: "edit",
           defaults: new { controller = "Home", action = "SearchEdit" }
          );
           routes.MapRoute(
           name: "SearchTranslatedProduct",
           url: "search-edit",
           defaults: new { controller = "Home", action = "SearchTranslatedProduct" }
          );
           routes.MapRoute(
           name: "EditForm",
           url: "edit/{id}",
           defaults: new { controller = "Product", action = "EditForm", id = String.Empty }
          );
            routes.MapRoute(
            name: "BrowseByMarket",
            url: "market/{market}",
            defaults: new { controller = "Home", action = "MarketFiles", market = String.Empty }
          );
            routes.MapRoute(
            name: "IsTranslated",
            url: "api/markets/{market}/datasheet/{ProductId}",
            defaults: new { controller = "Product", action = "IsTranslated", market = String.Empty, productId = String.Empty }
          );
            routes.MapRoute(
            name: "SendToEditForm",
            url: "redirect/edit/{id}",
            defaults: new { controller = "Home", action = "SendToEditForm", id = "null"}
          );
            routes.MapRoute(
            name: "FilterByLanguage",
            url: "api/markets/{market}/languages/{countryId}",
            defaults: new { controller = "Product", action = "FilterByLanguage", market = String.Empty, countryId = String.Empty }
          );

        }
    }
}
