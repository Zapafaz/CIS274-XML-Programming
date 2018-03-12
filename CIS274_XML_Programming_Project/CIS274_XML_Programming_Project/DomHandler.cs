/*
 * Student: Adam Wight
 * Class: CIS274M XML Programming
 * Instructor: Ed Cauthorn
 */

using System.Xml;

namespace CIS274_XML_Programming_Project
{
    class DomHandler
    {
        XmlDocument Document { get; }
        XmlNode Root { get; }

        public DomHandler(XmlDocument document, XmlNode root)
        {
            Document = document;
            Root = root;
        }

        /// <summary>
        /// Finds an XmlNode from the given <paramref name="identifier"/> (an ID attribute value OR a DocumentTitle element value)
        /// </summary>
        /// <param name="identifier">An ID attribute value OR a DocumentTitle element value</param>
        /// <returns>Returns the found node, or null if it could not be found</returns>
        public XmlNode FindNode(string identifier)
        {
            XmlNodeList documents = Document.GetElementsByTagName("Document");
            
            foreach(XmlNode node in documents)
            {
                string idVal = node.Attributes["ID"].Value;
                if (idVal == identifier)
                {
                    return node;
                }
                XmlNode titleNode = node.SelectSingleNode("DocumentTitle");
                if (titleNode != null)
                {
                    string title = titleNode.InnerText;
                    if (title == identifier)
                    {
                        return titleNode.ParentNode;
                    }
                }
            }
            return null;
        }
    }
}
