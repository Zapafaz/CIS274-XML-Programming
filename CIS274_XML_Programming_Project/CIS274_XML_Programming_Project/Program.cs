/*
 * Student: Adam Wight
 * Class: CIS274M XML Programming
 * Instructor: Ed Cauthorn
 * Due date: Monday, February 19th
 */

using System;
using System.Xml.Linq;

namespace CIS274_XML_Programming_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateDocument();
            ContinuePrompt();
        }

        /// <summary>
        /// Creates an XDocument with a set of documents from the documents folder and saves it to the script output folder.
        /// </summary>
        public static void CreateDocument()
        {
            var sourcePath = @"H:\Projects\School\CIS274 XML Programming\Resources\Documents";
            var targetPath = @"H:\Projects\School\CIS274 XML Programming\ScriptOutput\week4.xml";
            var doc = new XDocument();
            var converter = new DocumentConverter(sourcePath);
            doc.Add(converter.ConvertFilesToDocumentSet());
            doc.Save(targetPath);
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