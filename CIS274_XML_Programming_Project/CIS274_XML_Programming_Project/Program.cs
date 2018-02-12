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
        static void Main(string[] args)
        {
            CreatePoemDoc();
            ContinuePrompt();
        }

        /// <summary>
        /// Creates an XDocument with a set of poems from the poems folder and saves it to the script output folder.
        /// </summary>
        public static void CreatePoemDoc()
        {
            var sourcePath = @"H:\Google Drive\Projects\School\SPRING 2018\CIS274 XML Programming\Resources\Poems";
            var targetPath = @"H:\Google Drive\Projects\School\SPRING 2018\CIS274 XML Programming\ScriptOutput\week4.xml";
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
