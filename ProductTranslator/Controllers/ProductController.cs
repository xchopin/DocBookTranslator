using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml;

namespace ProductTranslator.Controllers
{
    public class ProductController : BaseController
    {
        public ActionResult Index(String id)
        {
      
            if (id != null && System.IO.File.Exists(Server.MapPath("~/Resources/DB/Eau/fr/" + id + ".xml")))
            {
                this.Flash("success", "File exists !");
                return RedirectToAction("Index", "Home");
            }

            this.Flash("danger", "Error: this product id does not exist!");
            return RedirectToAction("Index", "Home");
        }

        private void AllElement(XmlNode node, String element)
        {
            if (node.HasChildNodes)
            {
                foreach (XmlNode subNode in node.ChildNodes)
                {
                    Console.WriteLine(subNode.Name);
                }
            }
        }

    }
}