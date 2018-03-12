/*
 * Student: Adam Wight
 * Class: CIS274M XML Programming
 * Instructor: Ed Cauthorn
 * Due date: Monday, February 19th
 */

using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace CIS274_XML_Programming_Project
{
    /// <summary>
    /// Converts documents in text files to XML.
    /// </summary>
    // TODO: Maybe change this to read a single file rather than a full folder.
    // Also, try out async stuff.
    public class DocumentConverter
    {
        /// <summary>
        /// The folder to pull documents from.
        /// </summary>
        public string SourceFolderPath
        {
            get;
        }
        /// <summary>
        /// Used to give Document and DocumentSection elements their ID attributes.
        /// </summary>
        private char documentId = 'A';

        /// <summary>
        /// Create a document converter for the given folder.
        /// </summary>
        /// <param name="folderPath">The folder path containing document files.</param>
        public DocumentConverter(string folderPath)
        {
            SourceFolderPath = folderPath;
        }

        /// <summary>
        /// Converts all files in the folder path to an XElement.
        /// </summary>
        /// <returns>Returns the set of documents in the folder path as an XElement.</returns>
        public XElement ConvertFilesToDocumentSet()
        {
            string[] allFiles = Directory.GetFiles(SourceFolderPath);
            var documentSet = new XElement("DocumentSet");
            foreach (string file in allFiles)
            {
                documentSet.Add(ConvertDocument(ReadFileLines(file)));
                documentId++;
            }
            return documentSet;
        }

        /// <summary>
        /// Reads all lines of text in the file at <paramref name="path"/> and returns them as a set of strings (one per line).
        /// </summary>
        /// <param name="path">The file to read from.</param>
        /// <returns>Returns all lines of text that were in the file.</returns>
        private string[] ReadFileLines(string path)
        {
            var lines = new List<string>();
            using(var reader = new StreamReader(path))
            {
                while (reader.Peek()> -1)
                {
                    lines.Add(reader.ReadLine());
                }
            }
            return lines.ToArray();
        }

        /// <summary>
        /// Converts <paramref name="allLines"/> of a document to an XElement; assumes the first line is the document title and the second line is the author.
        /// </summary>
        /// <param name="allLines">The lines of a document (with line 1 as title, line 2 as author)</param>
        /// <returns>Returns an XElement containing a document as XML.</returns>
        private XElement ConvertDocument(string[] allLines)
        {
            var xmlDocument = new XElement("Document",
                new XElement("DocumentTitle", allLines[0]),
                new XElement("DocumentAuthor", allLines[1]),
                new XAttribute("ID", documentId)
            );
            return AddLinesToDocument(allLines, xmlDocument);
        }

        /// <summary>
        /// Adds all of the sections of a document (based on <paramref name="allLines"/>) to the <paramref name="documentWithoutLines"/> XElement.
        /// </summary>
        /// <param name="allLines">The lines of a document (with line 1 as title, line 2 as author)</param>
        /// <param name="documentWithoutLines">An XElement that contains a document base.</param>
        /// <returns>Returns an XElement containing a document as XML.</returns>
        private XElement AddLinesToDocument(string[] allLines, XElement documentWithoutLines)
        {
            var currentSection = new XElement("DocumentSection");
            var currentLine = new XElement("DocumentLine");
            var document = new XElement(documentWithoutLines);
            int sectionCount = 1;
            for (int i = 2; i < allLines.Length; i++)
            {
                if (allLines[i].Length > 0)
                {
                    currentLine.Add(allLines[i]);
                    currentSection.Add(new XElement(currentLine));
                    currentLine.RemoveAll();
                }
                else if (currentSection.HasElements)
                {
                    currentSection.Add(new XAttribute("ID", $"{documentId}:{sectionCount}"));
                    document.Add(new XElement(currentSection));
                    currentSection.RemoveAll();
                    sectionCount++;
                }
            }
            return document;
        }
    }
}