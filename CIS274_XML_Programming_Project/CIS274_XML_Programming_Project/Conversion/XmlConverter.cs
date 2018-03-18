using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace CIS274_XML_Programming_Project.Conversion
{
    public abstract class XmlConverter
    {
        public abstract XElement Convert(string path, char fileId);

        /// <summary>
        /// Reads all lines of text in the file at <paramref name="path"/> and returns them as a set of strings (one per line).
        /// </summary>
        /// <param name="path">The file to read from.</param>
        /// <returns>Returns all lines of text that were in the file.</returns>
        protected string[] ReadLines(string path)
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(path))
            {
                while (reader.Peek() > -1)
                {
                    lines.Add(reader.ReadLine());
                }
            }
            return lines.ToArray();
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
