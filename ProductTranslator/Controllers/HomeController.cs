using System;
using System.Diagnostics;
using System.IO;
using System.Web.Mvc;
using System.Xml;

namespace ProductTranslator.Controllers
{
    public class HomeController : BaseController
    {

        /**
         * Render the index page
         * 
         */
        public ActionResult Index()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(Server.MapPath("~/Resources/markets.xml"));
            ViewBag.markets = xml.SelectNodes("/markets/market");

            return View();
        }

        /*
         * Find a product in the database and redirect to the form page
         * 
         */ 
        public ActionResult SearchProduct()
        {
            String productId = Request["productId"];

            String[] directories = Directory.GetDirectories(Server.MapPath("~/Resources/DB/"), "*", System.IO.SearchOption.AllDirectories);

            TempData["path"] = null;

            foreach (String directory in directories)
            {
                if (directory.EndsWith("fr"))
                {
                    if (System.IO.File.Exists(directory + "\\" +  productId + ".xml"))
                    {
                        TempData["path"] = directory + "\\";
                        TempData["file"] = directory + "\\" + productId + ".xml";
                    }
                }                   
            }
         

            if (TempData["path"] == null)
            {
                this.Flash("danger", "Error: this product id does not exist!");
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "Product", new { id = productId });
        }
    }
}
