using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CIS274_XML_Programming_Project
{
    public class PoemConverter
    {
        public string SourceFolderPath { get; }
        private char poemId = 'A';

        public PoemConverter(string folderPath)
        {
            SourceFolderPath = folderPath;
        }

        public XElement ConvertFilesToPoemSet()
        {
            string[] allFiles = Directory.GetFiles(SourceFolderPath);
            var poemSet = new XElement("PoemSet");
            foreach (string file in allFiles)
            {
                poemSet.Add(ConvertPoem(ReadFileLines(file)));
                poemId++;
            }
            return poemSet;
        }

        private string[] ReadFileLines(string path)
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(path))
            {
                while(reader.Peek() > -1)
                {
                    lines.Add(reader.ReadLine());
                }
            }
            return lines.ToArray();
        }

        private XElement ConvertPoem(string[] allLines)
        {
            var xmlPoem = new XElement("Poem",
                        new XElement("PoemTitle", allLines[0]),
                        new XElement("PoemAuthor", allLines[1]),
                        new XAttribute("ID", poemId)
                     );
            return AddStanzasToPoem(allLines, xmlPoem);
        }

        private XElement AddStanzasToPoem(string[] allLines, XElement poemWithoutStanzas)
        {
            var currentStanza = new XElement("Stanza");
            var currentLine = new XElement("PoemLine");
            var poem = new XElement(poemWithoutStanzas);
            int stanzaCount = 1;
            for (int i = 2; i < allLines.Length; i++)
            {
                if (allLines[i].Length > 0)
                {
                    currentLine.Add(allLines[i]);
                    currentStanza.Add(new XElement(currentLine));
                    currentLine.RemoveAll();
                }
                else if (currentStanza.HasElements)
                {
                    currentStanza.Add(new XAttribute("ID", $"{poemId}:{stanzaCount}"));
                    poem.Add(new XElement(currentStanza));
                    currentStanza.RemoveAll();
                    stanzaCount++;
                }
            }
            return poem;
        }
    }
}
