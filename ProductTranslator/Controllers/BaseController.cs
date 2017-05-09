﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductTranslator.Controllers
{
    public class BaseController : Controller
    {
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
    }
}