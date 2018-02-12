using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CIS274_XML_Programming_Project
{
    class Program
    {
        private XDocument poemSet;

        static void Main(string[] args)
        {
            CreatePoemDoc();
            ContinuePrompt();
        }

        public static void CreatePoemDoc()
        {
            var sourcePath = @"H:\Google Drive\Projects\School\SPRING 2018\CIS274 XML Programming\Resources\Poems";
            var targetPath = @"H:\Google Drive\Projects\School\SPRING 2018\CIS274 XML Programming\ScriptOutput\week3.xml";
            var doc = new XDocument();
            var converter = new PoemConverter(sourcePath);
            doc.Add(converter.ConvertFilesToPoemSet());
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
