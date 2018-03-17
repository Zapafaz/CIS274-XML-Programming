/*
 * Student: Adam Wight
 * Class: CIS274M XML Programming
 * Instructor: Ed Cauthorn
 */

using System;
using System.Xml.Linq;
using System.Xml;

namespace CIS274_XML_Programming_Project
{
    class Program
    {
        private static string outputPath = @"H:\Projects\School\CIS274 XML Programming\ScriptOutput\week7.xml";
        private static string documentsPath = @"H:\Projects\School\CIS274 XML Programming\Resources\Documents";

        private static string targetNewPoemPath = @"H:\Projects\School\CIS274 XML Programming\ScriptOutput\";

        static void Main(string[] args)
        {
            // GetNewPoemFromWeb();
            // CreateDocument();
            RemoveDocumentsViaDom(new string[] { "E", "Richard Cory"});
            ContinuePrompt();
        }

        /// <summary>
        /// Creates an XDocument with a set of documents from the documents folder and saves it to the script output folder.
        /// </summary>
        public static void CreateDocument()
        {
            var doc = new XDocument();
            var converter = new DocumentConverter(documentsPath);
            doc.Add(converter.ConvertFilesToDocumentSet());
            doc.Save(outputPath);
        }

        /// <summary>
        /// Gets a new document (poem) from wikisource.org via a GET HTTPrequest, then parses the HTTPresponse.
        /// </summary>
        public static void GetNewDocumentFromWeb()
        {
            var handler = new WebHandler();
            handler.GetResponse(handler.SendRequest(@"https://en.wikisource.org/wiki/Ozymandias_(Shelley)", "GET"));
            var converter = new WikiConvert();
            converter.ConvertWikiPoemToPlainText(handler.SavedResponse, targetNewPoemPath);
        }

        /// <summary>
        /// Uses the XML DOM to find and remove all elements matching <paramref name="identifiers"/> (which are either a DocumentTitle element or an ID attribute)
        /// </summary>
        /// <param name="identifiers">A set of identifiers for XML nodes; each string can be an ID attribute value OR a DocumentTitle element value</param>
        public static void RemoveDocumentsViaDom(string[] identifiers)
        {
            var doc = new XmlDocument();
            string path = @"H:\Projects\School\CIS274 XML Programming\Resources\XML\week6.xml";
            doc.Load(path);

            XmlNode root = doc.DocumentElement;

            var handler = new DomHandler(doc, root);

            foreach (string id in identifiers)
            {
                XmlNode node = handler.FindNode(id);
                if (node != null)
                {
                    root.RemoveChild(node);
                }
                else
                {
                    Console.WriteLine($"Could not find any matches to ID: {id} in {path}");
                }
            }

            doc.Save(outputPath);
        }

        /// <summary>
        /// Prompts the user to continue by pressing enter.
        /// </summary>
        public static void ContinuePrompt()
        {
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}