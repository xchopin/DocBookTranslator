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
           if (TempData["file"] != null && TempData["path"] != null)
           {
                XmlDocument xml = new XmlDocument();
                data = new List<String>();
                xml.Load(TempData["file"].ToString());
                AllElement(xml, "title");
                AllElement(xml, "para");
                AllElement(xml, "emphasis");
                AllElement(xml, "entry");

                ViewBag.forms = data;
                ViewBag.productId = id;

                xml.Load(Server.MapPath("~/Resources/languages.xml"));
                ViewBag.languages = xml.SelectNodes("/languages/language");

                TempData["path"] = TempData["path"]; // To be sure it persists: TempData has a short-lived instance
                TempData["file"] = TempData["file"];

                return View();
           }

            this.Flash("danger", "Please use the quicksearch instead of typing in the address bar");
            return RedirectToAction("Index", "Home");
        }


       /**
        * Create a XML file of the translation previously done
        * 
        */
        public ActionResult TranslateProduct()
        {

            if (TempData["file"] != null && TempData["path"] != null)
            {
                String id = Request.Params["productId"];
                String languageId = Request.Params["input_languageId"];

                XmlDocument xml = new XmlDocument();
                xml.Load(Server.MapPath("~/Resources/languages.xml"));

                // Prevents against bad path ! (Check if the language exists)
                try
                {
                    String test = xml.SelectSingleNode("/languages/language[@id='" + languageId + "']").InnerText;

                    // Delete the two last letters of the source dir (in this case 'fr')
                    String marketPath = TempData["path"].ToString().Remove(TempData["path"].ToString().Length - 3);
                    String dirName = marketPath + languageId + "/";
                   
                    // Works only if the directory does not exist (https://msdn.microsoft.com/en-us/library/54a0at6s.aspx)
                    Directory.CreateDirectory(dirName);

                    // Node values start at i = 2 in POST params (if you don't know why check the view)
                    int fieldsNumber = Request.Form.Count - 2;  
                    data = new List<String>(fieldsNumber);
                    for (int i = 0; i < fieldsNumber; i++)
                    {
                        data.Add(Request.Form["translation" + i]);
                    }

                    // This part change and save the new content
                    xml.Load(TempData["file"].ToString());
                    ReplaceContent(xml, "title");
                    ReplaceContent(xml, "para");
                    ReplaceContent(xml, "emphasis");
                    ReplaceContent(xml, "entry");
                    xml.PreserveWhitespace = true;
                    xml.Save(dirName + id + ".xml");

                    this.Flash("success", "Translation succeeded!");
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    this.Flash("danger", "This language does not exist, please verify the xml language file (developer)");
                    return RedirectToAction("Index", "Home");
                }
            }

            this.Flash("danger", "An error occured, you might be faster to fill the fields.");
            return RedirectToAction("Index", "Home");
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