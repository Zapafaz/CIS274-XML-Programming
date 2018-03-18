using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CIS274_XML_Programming_Project.Conversion
{
    class TextConverter : XmlConverter
    {
        /// <summary>
        /// Converts <paramref name="lines"/> of a document to an XElement; assumes the first line is the document title and the second line is the author.
        /// </summary>
        /// <param name="path">The file to be converted to XML.</param>
        /// <param name="fileId">The identifying character for the file being converted.</param>
        /// <returns>Returns an XElement containing a document as XML.</returns>
        public override XElement Convert(string path, char fileId)
        {
            string[] lines = ReadLines(path);

            var xmlDocument = new XElement("Document",
                new XElement("DocumentTitle", lines[0]),
                new XElement("DocumentAuthor", lines[1]),
                new XAttribute("ID", fileId)
            );
            return AddLinesToDocument(lines, fileId, xmlDocument);
        }

        /// <summary>
        /// Adds all of the sections of a document (based on <paramref name="lines"/>) to the <paramref name="documentWithoutLines"/> XElement.
        /// </summary>
        /// <param name="lines">The lines of a document (with line 1 as title, line 2 as author)</param>
        /// <param name="fileId">The identifying character for the file being converted.</param>
        /// <param name="documentWithoutLines">An XElement that contains a document base.</param>
        /// <returns>Returns an XElement containing a document as XML.</returns>
        private XElement AddLinesToDocument(string[] lines, char fileId, XElement documentWithoutLines)
        {
            var currentSection = new XElement("DocumentSection");
            var currentLine = new XElement("DocumentLine");
            var document = new XElement(documentWithoutLines);
            int sectionCount = 1;

            for (int i = 2; i < lines.Length; i++)
            {
                if (lines[i].Length > 0)
                {
                    currentLine.Add(lines[i]);
                    currentSection.Add(new XElement(currentLine));
                    currentLine.RemoveAll();
                }
                else if (currentSection.HasElements)
                {
                    AddSection();
                }
            }

            if (currentSection.HasElements)
            {
                AddSection();
            }
            return document;

            void AddSection()
            {
                currentSection.Add(new XAttribute("ID", $"{fileId}:{sectionCount}"));
                document.Add(new XElement(currentSection));
                currentSection.RemoveAll();
                sectionCount++;
            }
        }
    }
}
