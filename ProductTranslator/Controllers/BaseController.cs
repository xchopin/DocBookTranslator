using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductTranslator.Controllers
{
    public class BaseController : Controller
    {
        protected void Flash(String type, String message)
        {
            TempData["flash.type"] = type;
            TempData["flash.message"] = message;
        }

        protected ActionResult Render(String file)
        {
            return View("~/Views/" + file + ".cshtml");
        }
    }
}