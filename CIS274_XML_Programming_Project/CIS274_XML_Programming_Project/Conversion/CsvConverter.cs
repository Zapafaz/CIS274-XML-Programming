using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace CIS274_XML_Programming_Project.Conversion
{
    class CsvConverter : XmlConverter
    {
        /// <summary>
        /// Converts CSV file at <paramref name="path"/> to an XElement; assumes the first line is the headers for the sheet.
        /// </summary>
        /// <param name="path">The file to be converted to XML.</param>
        /// <param name="fileId">The identifying character for the file being converted.</param>
        /// <returns>Returns an XElement containing a document as XML.</returns>
        public override XElement Convert(string path, char fileId)
        {
            List<string[]> sheet = ReadCsv(path);
            var test = sheet.ToArray();
            var test2d = sheet.To2dArray();

            int distinct = FindDistinctColumn(sheet);

            string crn = "CRN";
            string[] headers = sheet[0];

            int crnCol = FindColumnByHeader(headers, crn);

            var xml = new XElement("CourseSchedule",
                    new XAttribute("SheetId", fileId)
            );

            for (int row = 1; row < sheet.Count; row++)
            {
                string courseElementName = "Course";
                xml.Add(
                    new XElement(courseElementName,
                    new XAttribute(crn, sheet[crnCol][row]))
                    );
                XElement currentCourseElement = xml.Element(courseElementName);
                for (int col = 0; col < headers.Length; col++)
                {
                    if (col != crnCol)
                    {
                        currentCourseElement.Add();
                    }
                }
            }

            return xml;
        }

        private int FindColumnByHeader(string[] headers, string match)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i] == match)
                {
                    return i;
                }
            }
            return -1;
        }

        public int FindDistinctColumn(List<string[]> sheet)
        {
            for (int col = 0; col < sheet.Count; col++)
            {
                var checker = new HashSet<string>();
                if (sheet[col].All(checker.Add))
                {
                    return col;
                }
            }
            return -1;
        }
    }
}
