using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductTranslator.Resources.Helpers
{
    public class Helper : Controller
    {
       
        public static void flash(String type, String message)
        {
            ViewBag.flash_message = message;
            ViewBag.flash_type = type;
        }

        public static ActionResult render(String file)
        {
            return View(file + ".cshtml");
        }


    }
}