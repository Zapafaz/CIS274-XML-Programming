using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CIS274_XML_Programming_Project
{
    /// <summary>
    /// Converts poems in text files to XML.
    /// </summary>
    /// <remarks>TODO: Maybe change this to do a single file rather than a full folder.
    /// Also, try out async stuff.</remarks>
    public class PoemConverter
    {
        /// <summary>
        /// The folder to pull poems from.
        /// </summary>
        public string SourceFolderPath { get; }
        /// <summary>
        /// Used to give Poem and Stanza elements their ID attributes.
        /// </summary>
        private char poemId = 'A';

        /// <summary>
        /// Create a poem converter for the given folder.
        /// </summary>
        /// <param name="folderPath">The folder path containing poem files.</param>
        public PoemConverter(string folderPath)
        {
            SourceFolderPath = folderPath;
        }

        /// <summary>
        /// Converts all files in the folder path to an XElement.
        /// </summary>
        /// <returns>Returns the set of poems in the folder path as an XElement.</returns>
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

        /// <summary>
        /// Reads all lines of text in the file at <paramref name="path"/> and returns them as a set of strings (one per line).
        /// </summary>
        /// <param name="path">The file to read from.</param>
        /// <returns>Returns all lines of text that were in the file.</returns>
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

        /// <summary>
        /// Converts <paramref name="allLines"/> of a poem to an XElement; assumes the first line is the poem title and the second line is the author.
        /// </summary>
        /// <param name="allLines">The lines of a poem (with line 1 as title, line 2 as author)</param>
        /// <returns>Returns an XElement containing a poem as XML.</returns>
        private XElement ConvertPoem(string[] allLines)
        {
            var xmlPoem = new XElement("Poem",
                        new XElement("PoemTitle", allLines[0]),
                        new XElement("PoemAuthor", allLines[1]),
                        new XAttribute("ID", poemId)
                     );
            return AddStanzasToPoem(allLines, xmlPoem);
        }

        /// <summary>
        /// Adds all of the stanzas of a poem (based on <paramref name="allLines"/>) to the <paramref name="poemWithoutStanzas"/> XElement.
        /// </summary>
        /// <param name="allLines">The lines of a poem (with line 1 as title, line 2 as author)</param>
        /// <param name="poemWithoutStanzas">An XElement that contains a poem in XML.</param>
        /// <returns>Returns an XElement containing a poem as XML.</returns>
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
