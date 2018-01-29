using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS274_XML_Programming_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            CombinePoemsToXml();
            ContinuePrompt();
        }

        public static void CombinePoemsToXml()
        {
            var sourcePath = @"H:\Google Drive\Projects\School\SPRING 2018\CIS274 XML Programming\Resources\Poems";
            var targetPath = @"H:\Google Drive\Projects\School\SPRING 2018\CIS274 XML Programming\ScriptOutput\week2.xml";
            var converter = new PoemConverter(sourcePath, targetPath);
            string[] allFiles = Directory.GetFiles(sourcePath);
            foreach (string file in allFiles)
            {
                var fileLines = converter.FileTextToLines(file);
                converter.LinesToXml(fileLines);
            }
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
