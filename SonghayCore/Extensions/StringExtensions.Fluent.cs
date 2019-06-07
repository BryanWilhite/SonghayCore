using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="string"/>.
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// Escapes the interpolation tokens of <see cref="string.Format(string, object[])"/>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string EscapeInterpolation(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input.Replace("{", "{{").Replace("}", "}}");
        }

        /// <summary>
        /// Replaces “snake” underscores with caps of first <see cref="char"/>
        /// after the underscore.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FromSnakeToCaps(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input.Split('_').Aggregate((a, i) => $"{a}{i.ToPascalCase()}");
        }

        /// <summary>
        /// Returns <see cref="string"/> in double quotes.
        /// </summary>
        /// <param name="input">The input.</param>
        public static string InDoubleQuotes(this string input)
        {
            if (string.IsNullOrEmpty(input)) return null;
            input = input.Replace("\"", "\"\"");
            return string.Format("\"{0}\"", input);
        }

        /// <summary>
        /// Returns <see cref="string"/> in double quotes or default.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="defaultValue">The default value.</param>
        public static string InDoubleQuotesOrDefault(this string input, string defaultValue)
        {
            if (string.IsNullOrEmpty(input)) return defaultValue;
            return input.InDoubleQuotes();
        }

        /// <summary>
        /// Reverse the string
        /// from http://en.wikipedia.org/wiki/Extension_method
        /// </summary>
        /// <param name="input"></param>
        /// <remarks>
        /// From Tomas Kubes, http://www.codeproject.com/Articles/31050/String-Extension-Collection-for-C
        /// </remarks>
        public static string Reverse(this string input)
        {
            char[] chars = input.ToCharArray();
            Array.Reverse(chars);
            return new String(chars);
        }

        /// <summary>
        /// Reduce string to shorter preview which is optionally ended by some string (...).
        /// </summary>
        /// <param name="s">string to reduce</param>
        /// <param name="count">Length of returned string including endings.</param>
        /// <param name="endings">optional endings of reduced text</param>
        /// <example>
        /// string description = "This is very long description of something";
        /// string preview = description.Reduce(20,"...");
        /// produce -> "This is very long..."
        /// </example>
        /// <remarks>
        /// From Tomas Kubes, http://www.codeproject.com/Articles/31050/String-Extension-Collection-for-C
        /// </remarks>
        public static string Reduce(this string s, int count, string endings)
        {
            if (count < endings.Length) throw new Exception("Failed to reduce to less then endings length.");

            int sLength = s.Length;
            int len = sLength;
            if (endings != null) len += endings.Length;

            if (count > sLength) return s; //it's too short to reduce

            s = s.Substring(0, sLength - len + count);

            if (endings != null) s += endings;

            return s;
        }

        /// <summary>
        /// Converts the <see cref="String" /> into a ASCII letters with spacer.
        /// </summary>
        /// <param name="input">The input.</param>
        public static string ToAsciiLettersWithSpacer(this string input)
        {
            return input.ToAsciiLettersWithSpacer('\0');
        }

        /// <summary>
        /// Converts the <see cref="String" /> into ASCII letters with spacer.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="spacer">The spacer.</param>
        public static string ToAsciiLettersWithSpacer(this string input, char spacer)
        {
            if (string.IsNullOrEmpty(input)) return null;

            if (spacer == '\0')
            {
                var chars = input
                    .Where(c => char.IsLetter(c))
                    .Where(c => char.GetNumericValue(c) < 128);
                return (chars.Count() > 0) ? new string(chars.ToArray()) : null;
            }
            else
            {
                var chars = input
                    .Where(c => (char.IsLetter(c)) || (c == spacer))
                    .Where(c => char.GetNumericValue(c) < 128);
                return (chars.Count() > 0) ? new string(chars.ToArray()).Replace("--", "-").Trim(new char[] { spacer }) : null;
            }
        }

        /// <summary>
        /// Converts the <see cref="String"/> into a blog slug.
        /// </summary>
        /// <param name="input">The input.</param>
        public static string ToBlogSlug(this string input)
        {
            if (string.IsNullOrEmpty(input)) throw new NullReferenceException("The expected input is not here");

            // Remove/replace entities:
            input = input.Replace("&amp;", "and");
            input = Regex.Replace(input, @"\&\w+\;", string.Empty, RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"\&\#\d+\;", string.Empty, RegexOptions.IgnoreCase);

            // Replace any characters that are not alphanumeric with hyphen:
            input = Regex.Replace(input, "[^a-z^0-9]", "-", RegexOptions.IgnoreCase);

            // Replace all double hyphens with single hyphen
            var pattern = "--";
            while (Regex.IsMatch(input, pattern)) input = Regex.Replace(input, pattern, "-", RegexOptions.IgnoreCase);

            // Remove leading and trailing hyphens ("-")
            pattern = "^-|-$";
            input = Regex.Replace(input, pattern, "", RegexOptions.IgnoreCase);

            return input.ToLower();
        }

        /// <summary>
        /// Converts the <see cref="String"/> into camel case
        /// by lower-casing the first character.
        /// </summary>
        /// <param name="input">The input.</param>
        public static string ToCamelCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return $"{input[0].ToString().ToLowerInvariant()}{input.Substring(1)}";
        }

        /// <summary>
        /// Converts the <see cref="String"/> into digits only.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string ToDigitsOnly(this string input)
        {
            var digitsOnly = new Regex(@"[^\d]");
            return digitsOnly.Replace(input, string.Empty);
        }

        /// <summary>
        /// Prepares a string to be converted to <c>int</c>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string ToIntString(this string input)
        {
            return input.ToIntString("0");
        }

        /// <summary>
        /// Prepares a string to be converted to <c>int</c>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="defaultValue">The default value ("0" by default).</param>
        /// <returns></returns>
        public static string ToIntString(this string input, string defaultValue)
        {
            if (input == null) return null;

            var output = defaultValue;
            var array = input.Split('.');

            if (array.Length == 0) return output;
            if (string.IsNullOrEmpty(array[0].Trim())) return output;

            output = array[0].ToDigitsOnly();
            if (string.IsNullOrEmpty(output)) return defaultValue;

            return output;
        }

        /// <summary>
        /// Converts the <see cref="String"/> into camel case
        /// by upper-casing the first character.
        /// </summary>
        /// <param name="input">The input.</param>
        public static string ToPascalCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return $"{input[0].ToString().ToUpperInvariant()}{input.Substring(1)}";
        }

        /// <summary>
        /// Converts the <see cref="String"/> into camel case
        /// then replaces every upper case character
        /// with an underscore and its lowercase equivalent.
        /// </summary>
        /// <param name="input">The input.</param>
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input.ToCamelCase()
                .InsertSpacesBeforeCaps()
                .FromCharsToString()
                .Replace(' ', '_')
                .ToLowerInvariant();
        }

        /// <summary>
        /// Formats the <see cref="string"/> into a shortened form,
        /// showing the search text in context.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="searchText">The search text.</param>
        /// <param name="contextLength">Length of the context.</param>
        public static string ToSubstringInContext(this string input, string searchText, int contextLength)
        {
            if (string.IsNullOrEmpty(input)) return input;
            if (string.IsNullOrEmpty(searchText)) return input;

            if (input.Contains(searchText))
            {
                if (searchText.Length >= contextLength)
                    return searchText.Substring(0, contextLength);

                var edgesLength = Convert.ToInt32(Math.Ceiling((contextLength - searchText.Length) / 2d));

                var i0 = input.IndexOf(searchText) - edgesLength;
                if (i0 < 0) i0 = 0;

                var i1 = i0 + searchText.Length + edgesLength;
                if (i1 > (input.Length - 1)) i1 = input.Length;

                return input.Substring(i0, i1);
            }
            else
            {
                var i0 = 0;
                var i1 = contextLength - 1;
                if (i1 > (input.Length - 1)) i1 = input.Length;

                return input.Substring(i0, i1);
            }
        }

        /// <summary>
        /// Truncates the specified input to 16 characters.
        /// <param name="input">The input.</param>
        /// </summary>
        public static string Truncate(this string input)
        {
            return input.Truncate(length: 16, ellipsis: "…");
        }

        /// <summary>
        /// Truncates the specified input to 16 characters.
        /// <param name="input">The input.</param>
        /// <param name="length">The length.</param>
        /// </summary>
        public static string Truncate(this string input, int length)
        {
            return input.Truncate(length, ellipsis: "…");
        }

        /// <summary>
        /// Truncates the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="length">The length.</param>
        /// <param name="ellipsis"></param>
        public static string Truncate(this string input, int length, string ellipsis)
        {
            if (string.IsNullOrEmpty(input)) return input;
            if (input.Length <= length) return input;
            return string.Concat(input.Substring(0, length).TrimEnd(), ellipsis);
        }

        /// <summary>
        /// Unwraps for RIA endpoint.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="riaEndpointName">Name of the RIA endpoint.</param>
        public static string UnwrapForRiaEndpoint(this string input, string riaEndpointName)
        {
            if (string.IsNullOrEmpty(input)) return null;
            if (string.IsNullOrEmpty(riaEndpointName)) return null;

            var riaResultName = riaEndpointName + "Result";

            string wrapper = @"{""" + riaResultName + @""":";
            return input.Replace(wrapper, string.Empty).Replace("}}", "}").Replace("]}", "]");
        }

        /// <summary>
        /// Wraps for RIA endpoint input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="riaParameter">The RIA parameter.</param>
        public static string WrapForRiaEndpoint(this string input, string riaParameter)
        {
            if (string.IsNullOrEmpty(input)) return null;
            if (string.IsNullOrEmpty(riaParameter)) return null;

            string wrapperFormat = @"{{""" + riaParameter + @""":{0}}}";
            return string.Format(wrapperFormat, input);
        }
    }
}
