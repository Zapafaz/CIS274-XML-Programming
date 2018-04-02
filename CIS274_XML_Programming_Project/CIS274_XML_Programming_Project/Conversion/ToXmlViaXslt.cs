/*
 * Student: Adam Wight
 * Class: CIS274M XML Programming
 * Instructor: Ed Cauthorn
 */

using System.IO;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace CIS274_XML_Programming_Project.Conversion
{
    class ToXmlViaXslt : ToXml
    {
        public string XsltPath { get; }

        public ToXmlViaXslt(string xsltPath)
        {
            XsltPath = xsltPath;
        }

        public override XElement Convert(string xmlInputPath, string fileId)
        {
            var transformer = new XslCompiledTransform();
            transformer.Load(XsltPath);

            // Use a temp file since XslCompiledTransform.Transform must write to a file but conversion is set up to use XElement
            using (var xmlStream = new FileStream(Path.GetTempFileName(), FileMode.Create))
            {
                transformer.Transform(xmlInputPath, arguments:null, results:xmlStream);
                return XElement.Load(xmlStream);
            }
        }
    }
}
