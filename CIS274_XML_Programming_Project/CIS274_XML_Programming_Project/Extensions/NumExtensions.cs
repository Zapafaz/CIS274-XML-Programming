/*
 * Student: Adam Wight
 * Class: CIS274M XML Programming
 * Instructor: Ed Cauthorn
 */

using System;
using System.Text;

namespace CIS274_XML_Programming_Project.Conversion
{
    public static class NumExtensions
    {
        /// <summary>
        /// Converts non-negative integers to letters for easy alphabetizing; 0 = A, 1 = B ... 25 = Z, 26 = AA, 27 = AB ...
        /// </summary>
        /// <param name="num">Any non-negative integer.</param>
        /// <param name="start">Any (latin alphabet) character from a-z (or A-Z).</param>
        /// <returns>Returns <paramref name="num"/> as an easily alphabetized string representation (0 = A, 1 = B ... 25 = Z, 26 = AA, 27 = AB ...).</returns>
        public static string ToAlpha(this int num, char start)
        {
            if (num < 0)
            {
                throw new ArgumentException($"Number ({num}) must be non-negative (i.e. number must be greater than or equal to 0)");
            }

            num++;
            var builder = new StringBuilder();
            while (num > 0)
            {
                num--;
                int remainder = num % 26;
                builder.Insert(0, (char)(start + (remainder)));
                num = (num - remainder) / 26;
            }
            return builder.ToString();
        }
    }
}
