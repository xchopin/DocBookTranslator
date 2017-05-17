using System;
using System.Diagnostics;
using System.IO;
using System.Web.Mvc;
using System.Xml;

namespace ProductTranslator.Controllers
{
    public class HomeController : BaseController
    {

        /*
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
                    if (System.IO.File.Exists(directory + "\\" + productId + ".xml"))
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

        public ActionResult SearchEdit()
        {
            this.sendLanguages();
            return View();
        }


       /*
        * Find a product in the database and redirect to the form page
        * 
        */ 
        public ActionResult SearchTranslatedProduct()
        {
            String productId = Request["productId"];
            String languageId = Request["input_languageId"];

            // Prevents against bad path ! (Check if the language exists)
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Server.MapPath("Resources/languages.xml"));
                String test = xml.SelectSingleNode("/languages/language[@id='" + languageId + "']").InnerText;
            }
            catch (Exception e)
            {
                this.Flash("danger", "This language does not exist, please verify the xml language file (developer)");
                return RedirectToAction("Index", "Home");
            }
        
            String[] directories = Directory.GetDirectories(Server.MapPath("~/Resources/DB/"), "*", System.IO.SearchOption.AllDirectories);

            TempData["path"] = null;
            TempData["languageId"] = languageId;

            foreach (String directory in directories)
            {
                if (directory.EndsWith(languageId))
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


            return RedirectToAction("EditForm", "Product", new { id = productId });
        }
    }
}
