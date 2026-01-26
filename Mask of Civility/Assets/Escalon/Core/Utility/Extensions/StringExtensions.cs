
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Escalon
{
    public static class StringExtensions
    {
        public static string AddSpacesToSentence(this string text, bool preserveAcronyms = true)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }

            return newText.ToString();
        }

        public static string FormatInvariant(this string text, params object[] parameters)
        {
            return string.Format(CultureInfo.InvariantCulture, text, parameters);
        }
        
        public static string FirstCharToLowerCase(this string str)
        {
            if ( !string.IsNullOrEmpty(str) && char.IsUpper(str[0]))
                return str.Length == 1 ? char.ToLower(str[0]).ToString() : char.ToLower(str[0]) + str[1..];

            return str;
        }

        public static string FirstCharToUpperCase(this string str)
        {
            if (!string.IsNullOrEmpty(str))
                return str.Length == 1 ? char.ToUpper(str[0]).ToString() : char.ToUpper(str[0]) + str[1..];

            return str;
        }

        public static bool Contains(this string source, string target, StringComparison comparison)
        {
            int index = source.IndexOf(target, comparison);
            return index >= 0;
        }

        public static string Concatentate(this IEnumerable<string> input)
        {
            var concat = "";
            foreach (var s in input)
            {
                concat += s + "\n";
            }

            return concat;
        }

        public static string Concatentate(this IEnumerable<string> input, string delimiter)
        {
            var concat = "";
            foreach (var s in input)
            {
                concat += s + delimiter;
            }

            return concat;
        }
    }
}