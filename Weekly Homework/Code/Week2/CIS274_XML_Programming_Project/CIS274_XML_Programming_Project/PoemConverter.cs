using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CIS274_XML_Programming_Project
{
    public class PoemConverter
    {
        public string SourceFolderPath { get; }
        public string TargetFilePath { get; }
        public XmlDocument PoemSet { get; }

        public PoemConverter(string folderPath, string targetFilePath)
        {
            SourceFolderPath = folderPath;
            TargetFilePath = targetFilePath;
            PoemSet = new XmlDocument();
        }

        public string[] FileTextToLines(string path)
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

        public void LinesToXml(string[] allLines)
        {
            bool startNewStanza = true;
            using (var writer = new StreamWriter(TargetFilePath, true))
            {
                writer.WriteLine("\t<Poem>");
                foreach(string line in allLines)
                {
                    if (startNewStanza)
                    {
                        writer.WriteLine("\t\t<Stanza>");
                        startNewStanza = false;
                    }
                    if (line.Length > 0)
                    {
                        writer.WriteLine($"\t\t\t<PoemLine>{line}</PoemLine>");
                    }
                    else
                    {
                        writer.WriteLine("\t\t</Stanza>");
                        startNewStanza = true;
                    }
                }
                writer.WriteLine("\t</Poem>");
            }
        }
    }
}
