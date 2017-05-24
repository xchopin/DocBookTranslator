using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace ProductTranslator.Controllers
{
    public abstract class BaseController : Controller
    {

        protected String language = "fr";

        /**
         * Create flash messages
         * 
         * @param type (enter a Bootstrap class eg: danger, success)
         * @param message 
         */
        protected void Flash(String type, String message)
        {
            TempData["flash.type"] = type;
            TempData["flash.message"] = message;
        }

       /**
        * Render a .cshtml file from the view directory
        * 
        * @param file
        * @return ActionResult
        */
        protected ActionResult Render(String file)
        {
            return View("~/Views/" + file + ".cshtml");
        }


        /**
         * Add the languages from the XML file into the ViewBag
         * 
         */
        protected void sendLanguages()
        {
            System.Xml.XmlDocument xml = new XmlDocument();
            xml.Load(Server.MapPath("~/Resources/languages.xml"));
            ViewBag.languages = xml.SelectNodes("/languages/language");
        }
    }
}