/*
 * Student: Adam Wight
 * Class: CIS274M XML Programming
 * Instructor: Ed Cauthorn
 */

using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System;
using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace CIS274_XML_Programming_Project.Conversion
{
    class CsvToXml : ToXml
    {
        /// <summary>
        /// Converts CSV file at <paramref name="path"/> to an XElement; assumes the first line is the headers for the sheet.
        /// </summary>
        /// <param name="path">The file to be converted to XML.</param>
        /// <param name="fileId">The identifying string for the file being converted.</param>
        /// <returns>Returns an XElement containing a document as XML.</returns>
        public override XElement Convert(string path, string fileId)
        {
            // sheet[row][col]. Each string array is one row.
            List<string[]> sheet = ReadCsv(path);
            string idName = "ID";
            int classIdCounter = 1000;
            XNamespace ns = "https://www.mccnh.edu/academics/course-schedules || " + Path.GetFileName(path);
            var xml = new XElement(ns + "CourseSchedule",
                            new XAttribute(XNamespace.Xmlns + fileId, ns));
            XName classElementName = ns + "Class";

            // Assume row 0 is header row (later used for naming each element)
            if (sheet[0].Distinct().Count() != sheet[0].Count())
            {
                throw new Exception($"{path}\r\n" +
                                    "The first row (the headers) of this file has duplicate values in its cells; the first row must be distinct.");
            }

            for (int row = 1; row < sheet.Count; row++)
            {
                xml.Add(new XElement(classElementName,
                            new XAttribute(idName, classIdCounter)));
                XElement currentCourseElement = xml.Descendants(classElementName)
                                                   .Where(element => element.Attribute(idName)
                                                   .Value == $"{classIdCounter}")
                                                   .First();
                for (int col = 0; col < sheet[0].Length; col++)
                {
                    if (sheet[row][col].Length > 0)
                    {
                        currentCourseElement.Add(new XElement(ns + sheet[0][col], sheet[row][col],
                                                     new XAttribute(idName, $"{classIdCounter}:{col.ToAlpha('A')}")));

                    }
                }
                classIdCounter++;
            }

            return xml;
        }

        /// <summary>
        /// Reads all lines of text in the file at <paramref name="path"/> and returns them as a list of string arrays (one per line).
        /// </summary>
        /// <param name="path">The file to read from.</param>
        /// <returns>Returns all lines of text that were in the file.</returns>
        public List<string[]> ReadCsv(string path)
        {
            var lines = new List<string[]>();

            using (var reader = new TextFieldParser(path))
            {
                reader.SetDelimiters(new string[] { "," });
                while (!reader.EndOfData)
                {
                    try
                    {
                        lines.Add(reader.ReadFields());
                    }
                    catch (MalformedLineException e)
                    {
                        Console.WriteLine($"Invalid line {e} - skipped");
                    }
                }
            }

            return lines;
        }
    }
}
