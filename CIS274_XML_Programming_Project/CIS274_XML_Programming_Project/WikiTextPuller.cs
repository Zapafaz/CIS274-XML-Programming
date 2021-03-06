﻿/*
 * Student: Adam Wight
 * Class: CIS274M XML Programming
 * Instructor: Ed Cauthorn
 */

using System.Collections.Generic;
using System.IO;

namespace CIS274_XML_Programming_Project
{
    class WikiTextPuller
    {
        private string author;
        private string title;
        private List<string> text = new List<string>();

        private const string TITLE_ID_TEXT = "id=\"ws-title\">";
        private const string AUTHOR_ID_TEXT = "id=\"ws-author\">";
        private const string POEM_ID_TEXT = "<div class=\"poem\">";

        public void ConvertWikiPoemToPlainText(string fullWebPage, string targetPath)
        {
            bool foundAuthor = false;
            bool foundPoem = false;
            bool foundTitle = false;
            string line;
            using (var reader = new StringReader(fullWebPage))
            {
                while (reader.Peek() > 0)
                {
                    line = reader.ReadLine();
                    if (!foundTitle && line.Contains(TITLE_ID_TEXT))
                    {
                        foundTitle = true;
                        title = GetElementTextById(TITLE_ID_TEXT, line);
                        if (!foundAuthor && line.Contains(AUTHOR_ID_TEXT))
                        {
                            foundAuthor = true;
                            author = GetElementTextById(AUTHOR_ID_TEXT, line);
                        }
                    }
                    else if (!foundAuthor && line.Contains(AUTHOR_ID_TEXT))
                    {
                        foundAuthor = true;
                        author = GetElementTextById(AUTHOR_ID_TEXT, line);
                    }
                    else if (!foundPoem && line.Contains(POEM_ID_TEXT))
                    {
                        foundPoem = true;
                    }
                    else if (foundPoem && !line.StartsWith("</div>") && !line.StartsWith("<p><b>"))
                    {
                        int end = line.IndexOf("<");
                        text.Add(line.Remove(end));
                    }
                    else if (foundPoem && line.StartsWith("</div>"))
                    {
                        break;
                    }
                }
            }
            WriteToFile(targetPath);
        }

        private void WriteToFile(string targetPath)
        {
            using (var writer = new StreamWriter(targetPath + $"{title}.txt"))
            {
                writer.WriteLine(title);
                writer.WriteLine(author);
                writer.WriteLine();
                foreach(string line in text)
                {
                    writer.WriteLine(line);
                }
            }
        }

        private string GetElementTextById(string id, string line)
        {
            int start = line.IndexOf(id) + id.Length;
            string sub = line.Substring(start);
            int end = sub.IndexOf("</");
            return sub.Remove(end);
        }
    }
}
