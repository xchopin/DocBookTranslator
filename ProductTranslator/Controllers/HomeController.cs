using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
                if (directory.EndsWith(this.language))
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

        /*
      *  Render the form translation page using the file browsing
      *  It also checks if the file path given by the client exists. 
      *  (never trust the client especially when we use IO.File)
      * */
        public ActionResult TranslationByBrowsing()
        {
            String productId = Request["productId"];
            if (Request["languageId"] != null)
            {
                TempData["languageId"] = Request["languageId"];
            }
            String path = Server.MapPath("~/Resources/DB/" + Request["path"] + "/");
            TempData["path"] = null;

            if (System.IO.File.Exists(path + productId + ".xml"))
            {
                TempData["path"] = path;
                TempData["file"] = path + productId + ".xml";
            }

            if (TempData["path"] == null)
            {
                this.Flash("danger", "Error: this product id does not exist!");
                return RedirectToAction("Index");
            }


            return RedirectToAction("Index", "Product", new { id = productId });
        }


        /**
         *  Render the page that searches an existing file to edit
         *
         **/
        public ActionResult SearchEdit()
        {
            this.sendLanguages();
            return View();
        }


        /*
         * Render the page containing all the files in a market given
         * 
         * @param String market
         * */
        public ActionResult MarketFiles(String market)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Server.MapPath("~/Resources/markets.xml"));
                XmlNode node = xml.SelectSingleNode("//*[@id='" + market + "']");
                String marketName = node["name"].InnerText;
                ViewBag.files = Directory.GetFiles(Server.MapPath("~/Resources/DB/" + marketName + "/" + this.language), "*.xml")
                                     .Select(Path.GetFileName)
                                     .ToList();

                ViewBag.path = marketName + "/" + this.language;
                ViewBag.market = marketName;
                ViewBag.marketId = market;
                this.sendLanguages();
                return View();
            }
            catch (Exception e)
            {
                this.Flash("danger", "This market does not exist");
                return RedirectToAction("Index", "Home");
            }
        }

        /*
         * Makes a link between the market page and the edit form page
        * 
        */
        public ActionResult SendToEditForm()
        {
            String id = Request.Params["productId"];
            String market = Request.Params["market"];
            String languageId = Request.Params["languageId"];
            TempData["file"] = Server.MapPath("~/Resources/DB/" + market + "/" + languageId + "/" +  id + ".xml");
            TempData["path"] = Server.MapPath("~/Resources/DB/" + market + "/" + languageId + "/");
            TempData["languageId"] = languageId;

          
           return RedirectToAction("EditForm", "Product", new { id = id });
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
