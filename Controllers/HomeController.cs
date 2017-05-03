using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace ProductTranslator.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            XmlDocument xml = new XmlDocument();
            xml.Load(Server.MapPath("~/Resources/markets.xml"));
            XmlNodeList xnList = xml.SelectNodes("/markets/market");
            ViewBag.markets = xnList;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
 