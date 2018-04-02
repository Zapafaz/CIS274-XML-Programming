/*
 * Student: Adam Wight
 * Class: CIS274M XML Programming
 * Instructor: Ed Cauthorn
 */

using System.Collections.Generic;
using System.Text;

namespace CIS274_XML_Programming_Project
{
    public static class SetExtensions
    {
        /// <summary>
        /// Converts all elements of <paramref name="set"/> to a single string delimited by commas. e.g. { "Alice", "Pineapple", "Iron" } would be returned as "Alice, Pineapple, Iron"
        /// </summary>
        /// <returns>Returns all elements of <paramref name="set"/> to a single string delimited by commas. e.g. { "Alice", "Pineapple", "Iron" } would be returned as "Alice, Pineapple, Iron"</returns>
        public static string ShowElements(this string[] set)
        {
            var builder = new StringBuilder();
            foreach (string s in set)
            {
                builder.Append($"{s}, ");
            }

            builder.Remove(builder.Length, builder.Length - 2);

            return builder.ToString();
        }

        /// <summary>
        /// Converts all elements of <paramref name="set"/> to a single string delimited by commas. e.g. { "Alice", "Pineapple", "Iron" } would be returned as "Alice, Pineapple, Iron"
        /// </summary>
        /// <returns>Returns all elements of <paramref name="set"/> to a single string delimited by commas. e.g. { "Alice", "Pineapple", "Iron" } would be returned as "Alice, Pineapple, Iron"</returns>
        public static string ShowElements(this List<string> set)
        {
            return ShowElements(set.ToArray());
        }
    }
}
