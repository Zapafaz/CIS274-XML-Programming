/*
 * Student: Adam Wight
 * Class: CIS274M XML Programming
 * Instructor: Ed Cauthorn
 */

using System.IO;
using System.Xml.Linq;

namespace CIS274_XML_Programming_Project.Conversion
{
    /// <summary>
    /// Converts documents in text files to another format.
    /// </summary>
    public class FileAggregator
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
        protected int fileId = 0;

        /// <summary>
        /// Create a document converter for the given folder.
        /// </summary>
        /// <param name="folderPath">The folder path containing document files.</param>
        public FileAggregator(string folderPath)
        {
            SourceFolderPath = folderPath;
        }

        /// <summary>
        /// Using <paramref name="converter"/>, converts text in files to XML with <paramref name="root"/> as the root element.
        /// </summary>
        /// <returns>Returns the set of documents in the folder path as an XElement.</returns>
        public XElement ConvertFilesToXml(XElement root, ToXml converter)
        {
            string[] paths = Directory.GetFiles(SourceFolderPath);

            foreach (string file in paths)
            {
                root.Add(converter.Convert(file, fileId.ToAlpha('a')));
                fileId++;
            }
            return root;
        }
    }
}