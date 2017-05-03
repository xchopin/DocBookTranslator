using System;
using System.Web.Mvc;
using System.Xml;
using ProductTranslator.Resources.Helpers;

namespace ProductTranslator.Controllers
{
    public class HomeController : Controller
    {

    
        public ActionResult Index()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(Server.MapPath("~/Resources/markets.xml"));
            ViewBag.markets = xml.SelectNodes("/markets/market");

            return View();
        }

        public ActionResult SearchProduct()
        {
            //String productId = Request.Form["productId"];


            XmlDocument xml = new XmlDocument();
            xml.Load(Server.MapPath("~/Resources/markets.xml"));
            ViewBag.markets = xml.SelectNodes("/markets/market");

            Helper.flash("danger", "This product does not exist!");
          
            return Helper.render("Home/Index");
        
 
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
 