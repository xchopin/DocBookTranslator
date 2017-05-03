using System;
using System.Web.Mvc;
using System.Xml;

namespace ProductTranslator.Controllers
{
    public class HomeController : BaseController
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
            String productId = Request["productId"];

            if (System.IO.File.Exists(Server.MapPath("~/Resources/DB/Eau/fr/" + productId + ".xml"))) {
                this.Flash("success", "File exists !");
                return RedirectToAction("Index", "Product", new { id = productId });
            }

            this.Flash("danger", "Error: this product id does not exist!");
            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
 