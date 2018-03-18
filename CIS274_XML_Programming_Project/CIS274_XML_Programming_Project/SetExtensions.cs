using System.Collections.Generic;
using System.Text;

namespace CIS274_XML_Programming_Project
{
    public static class SetExtensions
    {
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

        public static string ShowElements(this List<string> set)
        {
            return ShowElements(set.ToArray());
        }

        public static string[,] To2dArray(this List<string[]> set)
        {
            var array = new string[set.Count, set[0].Length];
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    array[i,j] = set[i][j];
                }
            }
            return array;
        }
    }
}
