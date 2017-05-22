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

        string[] elements = { "title", "para", "emphasis", "entry" }; // Nodes containing text


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
                xml.PreserveWhitespace = true;
                xml.Load(TempData["file"].ToString());
                AllElement(xml, this.elements);

                ViewBag.forms = data;
                ViewBag.productId = id;

                this.sendLanguages();

                Session["path"] = TempData["path"]; // To be sure it persists: TempData has a short-lived instance
                Session["file"] = TempData["file"];

                return View();
           }

            this.Flash("danger", "Please use the quicksearch instead of typing in the address bar");
            return RedirectToAction("Index", "Home");
        }


        /*
         * Create the form page for editing a datasheet
         * 
         */ 
        public ActionResult EditForm(String id)
        {
             if (TempData["file"] != null && TempData["path"] != null)
           {

                XmlDocument xml = new XmlDocument();
                data = new List<String>();
                xml.PreserveWhitespace = true;

                // Original Content [FR]
                String path = TempData["path"].ToString();
                xml.Load(path.Remove(path.Length - 3) + "fr\\" + id + ".xml");

                AllElement(xml, elements);

                ViewBag.forms = data;
                ViewBag.productId = id;
                ViewBag.languageId = TempData["languageId"];

                // Edited Content
                data = new List<String>();
                xml.PreserveWhitespace = true;
                xml.Load(TempData["file"].ToString());

                
                AllElement(xml, this.elements);
       
                if (data.Count != ViewBag.forms.Count)
                {
                    this.Flash("warning", "This translation was not well settled. Create one instead");
                    return RedirectToAction("Index", "Home");
                }

                // Load the name of the language and check if it exists 
                try
                {
                    xml.Load(Server.MapPath("~/Resources/languages.xml"));
                    XmlNode node = xml.SelectSingleNode("/languages/language[@id='" + TempData["languageId"] + "']");
                    ViewBag.language = node.Attributes["name"].Value;
                }
                catch (Exception e)
                {
                    this.Flash("danger", "This language does not exist, please verify the xml language file (developer)");
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.edited = data;

                ViewBag.productId = id;

                // To be sure it persists: TempData has a short-lived instance
                Session["path"] = TempData["path"];
                Session["file"] = TempData["file"];

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
        

            if (Session["file"] != null && Session["path"] != null)
            {
                String id = Request.Params["productId"];
                String languageId = Request.Params["input_languageId"];
                XmlDocument xml = new XmlDocument();
                // Prevents against bad path ! (Check if the language exists)
                try
                {
                    xml.Load(Server.MapPath("~/Resources/languages.xml"));
                    String test = xml.SelectSingleNode("/languages/language[@id='" + languageId + "']").InnerText;
                }
                catch (Exception e)
                {
                    //Debug.WriteLine(e);
                    this.Flash("danger", "This language does not exist, please verify the xml language file (developer)");
                    return RedirectToAction("Index", "Home");
                }

                // Delete the two last letters of the source dir (in this case 'fr')
                String marketPath = Session["path"].ToString().Remove(Session["path"].ToString().Length - 3);
                String dirName = marketPath + languageId + "/";

                // Works only if the directory does not exist (https://msdn.microsoft.com/en-us/library/54a0at6s.aspx)
                Directory.CreateDirectory(dirName);

                data = new List<String>();

                foreach (String key in Request.Form)
                {
                    if (key.StartsWith("translation"))
                        data.Add(Request.Form[key]);
                }

                // This part change and save the new content
                xml.PreserveWhitespace = true;
                xml.Load(Session["file"].ToString());
                ReplaceContent(xml, this.elements);
                xml.Save(dirName + id + ".xml");

                this.Flash("success", "Translation succeeded!");
                return RedirectToAction("Index", "Home");
            }

            this.Flash("danger", "An error occured, you might be faster to fill the fields.");
            return RedirectToAction("Index", "Home");
        }



        /**
         * Get all the elements, given, in the XML file loaded
         * 
         * @param XmlNode node
         * @param Array elements
         */
        private static void AllElement(XmlNode node, String[] elements)
        {
            if (node.HasChildNodes)
            {
                foreach (XmlNode subNode in node.ChildNodes)
                {
                    if (elements.Contains(subNode.Name))
                    {
                        if (!subNode.InnerXml.TrimStart().StartsWith("<")) // Solve the issue of twice elements
                        {
                            String value = subNode.InnerText;
                            if (!Regex.IsMatch(value, @"^[a-zA-Z\d\*]*$") && value.Length != 1 && !value.StartsWith("[!!!!!") && !String.IsNullOrWhiteSpace(value))
                                data.Add(subNode.InnerText);
                        }
                    }
                    AllElement(subNode, elements);
                }
            }
        }

        
        /**
         * Replace the content of all the elements, given, in the XML file loaded
         * 
         * @param XmlNode node
         * @param Array elements
         */
        private void ReplaceContent(XmlNode node, String[] elements)
        {
            if (node.HasChildNodes)
            {
                foreach (XmlNode subNode in node.ChildNodes)
                {
                    if (elements.Contains(subNode.Name))
                    {
                        if (!subNode.InnerXml.TrimStart().StartsWith("<"))
                        {
                            String value = subNode.InnerText;
                            if (!Regex.IsMatch(value, @"^[a-zA-Z\d\*]*$") && value.Length != 1 && !value.StartsWith("[!!!!!") && !String.IsNullOrWhiteSpace(value))
                            {
                                subNode.InnerText = data[0];
                                data.RemoveAt(0);
                            }
                        }
                    }
                    this.ReplaceContent(subNode, elements);
                }
            }
        }
    }
}