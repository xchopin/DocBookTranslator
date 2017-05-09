using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace ProductTranslator.Controllers
{
    public class ProductController : BaseController
    {
        private static List<String> data;


        /**
         * Render the form page for the translation of a product
         * 
         * @param id : product id
         */ 
        public ActionResult Index(String id)
        {
            if (id != null)
            {
                String file = "~/Resources/DB/Eau/fr/" + id + ".xml";
                if (id != null && System.IO.File.Exists(Server.MapPath(file)))
                {
                    XmlDocument xml = new XmlDocument();
                    data = new List<String>();
                    xml.Load(Server.MapPath(file));
                    AllElement(xml, "title");
                    AllElement(xml, "para");
                    AllElement(xml, "emphasis");
                    AllElement(xml, "entry");

                    ViewBag.forms = data;
                    ViewBag.productId = id;

                    xml.Load(Server.MapPath("~/Resources/languages.xml"));
                    ViewBag.languages = xml.SelectNodes("/languages/language");

                    return View();
                }

            }

            this.Flash("danger", "Error: this product id does not exist!");
            return RedirectToAction("Index", "Home");
        }


       /**
        * Create a XML file of the translation previously done
        * 
        */
        public ActionResult TranslateProduct()
        {
            String id = Request.Params["productId"];
            String languageId = Request.Params["input_languageId"];
    
            /** CREATE A DIRECTORY */
            XmlDocument xml = new XmlDocument();
            xml.Load(Server.MapPath("~/Resources/languages.xml"));

            // Prevents against bad path ! (Check if the language exists)
            try
            {
                String test = xml.SelectSingleNode("/languages/language[@id='" + languageId + "']").InnerText;
                String dirName = "~/Resources/DB/Eau/" + languageId + "/";
                // Works only if the directory does not exist (https://msdn.microsoft.com/en-us/library/54a0at6s.aspx)
                Directory.CreateDirectory(Server.MapPath(dirName));

                /** CHANGE THE CONTENT */
                String src = "~/Resources/DB/Eau/fr/" + id + ".xml";
                data = new List<String>(Request.Form.Count - 1);

                // Node values start at i = 2 in POST params
                for (int i = 0; i < Request.Form.Count - 2; i++)
                {
                    data.Add(Request.Form["translation" + i]);
                }

                xml.Load(Server.MapPath(src));
                ReplaceContent(xml, "title");
                ReplaceContent(xml, "para");
                ReplaceContent(xml, "emphasis");
                ReplaceContent(xml, "entry");
                xml.PreserveWhitespace = true;
                xml.Save(Server.MapPath(dirName + id + ".xml"));

                this.Flash("success", "Translation succeeded!");
                return RedirectToAction("Index", "Home");
            } catch (Exception e) {
                this.Flash("danger", "This language does not exist, please verify the xml language file (developer)");
                return RedirectToAction("Index", "Home");
            }
       
            
        }


        /**
         * Get all the elements, given, in the XML file loaded
         * 
         * @param XmlNode node
         * @param String element
         */
        private static void AllElement(XmlNode node, String element)
        {
            if (node.HasChildNodes)
            {
                foreach (XmlNode subNode in node.ChildNodes)
                {
                    if (subNode.Name == element)
                    {
                        String value = subNode.InnerText;
                        if (!Regex.IsMatch(value, @"^\d+$") && !value.StartsWith("[!!!!!") && value.Length > 1)
                            data.Add(subNode.InnerText);
                    }
                    AllElement(subNode, element);
                }
            }
        }


        /**
         * Replace the content of all the elements, given, in the XML file loaded
         * 
         * @param XmlNode node
         * @param String element
         */
        private void ReplaceContent(XmlNode node, String element)
        {
            if (node.HasChildNodes)
            {
                foreach (XmlNode subNode in node.ChildNodes)
                {
                    if (subNode.Name == element)
                    {
                        String value = subNode.InnerText;
                        if (!Regex.IsMatch(value, @"^\d+$") && !value.StartsWith("[!!!!!") && value.Length > 1)
                        {
                            subNode.InnerText = data[0];
                            data.RemoveAt(0);
                        }

                    }

                    this.ReplaceContent(subNode, element);
                }
            }
        }
    }
}