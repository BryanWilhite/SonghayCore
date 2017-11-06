using System;
using System.Collections.Generic;
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
        /// Converts camel-case <see cref="System.String"/> to <see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/>.
        /// </summary>
        /// <param name="input">The input.</param>
        public static IEnumerable<string> FromCamelCaseToEnumerable(this string input)
        {
            return new string(input.InsertSpacesBeforeCaps().ToArray())
                .Split(' ').Where(i => !string.IsNullOrEmpty(i));
        }

        /// <summary>
        /// Determines whether the specified input is in the comma-delimited values.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="delimitedValues">The delimited values.</param>
        public static bool In(this string input, string delimitedValues)
        {
            return input.In(delimitedValues, ',');
        }

        /// <summary>
        /// Determines whether the specified input is in the delimited values.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="delimitedValues">The delimited values.</param>
        /// <param name="separator">The separator.</param>
        public static bool In(this string input, string delimitedValues, char separator)
        {
            if (string.IsNullOrEmpty(input)) return false;
            if (string.IsNullOrEmpty(delimitedValues)) return false;

            var set = delimitedValues.Split(separator);
            return set.Contains(input);
        }

        /// <summary>
        /// Ins the double quotes.
        /// </summary>
        /// <param name="input">The input.</param>
        public static string InDoubleQuotes(this string input)
        {
            if (string.IsNullOrEmpty(input)) return null;
            input = input.Replace("\"", "\"\"");
            return string.Format("\"{0}\"", input);
        }

        /// <summary>
        /// Ins the double quotes or default.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="defaultValue">The default value.</param>
        public static string InDoubleQuotesOrDefault(this string input, string defaultValue)
        {
            if (string.IsNullOrEmpty(input)) return defaultValue;
            return input.InDoubleQuotes();
        }

        /// <summary>
        /// Inserts the spaces before caps.
        /// </summary>
        /// <param name="input">The input.</param>
        public static IEnumerable<char> InsertSpacesBeforeCaps(this string input)
        {
            if (string.IsNullOrEmpty(input)) yield return '\0';
            foreach (char c in input)
            {
                if (char.IsUpper(c)) yield return ' ';
                yield return c;
            }
        }

        /// <summary>
        /// Determines whether the specified input is byte.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// 	<c>true</c> if the specified input is byte; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsByte(this string input)
        {
            return input.IsByte(null);
        }

        /// <summary>
        /// Determines whether the specified input is byte.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="secondaryTest">The secondary test.</param>
        /// <returns>
        /// 	<c>true</c> if the specified input is byte; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsByte(this string input, Predicate<byte> secondaryTest)
        {
            byte testValue;
            bool test = byte.TryParse(input, out testValue);

            if ((secondaryTest != null) && test) return secondaryTest(testValue);
            else return test;
        }

        /// <summary>
        /// Determines whether the specified input is decimal.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// 	<c>true</c> if the specified input is decimal; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDecimal(this string input)
        {
            return input.IsDecimal(null);
        }

        /// <summary>
        /// Determines whether the specified input is decimal.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="secondaryTest">The secondary test.</param>
        /// <returns>
        /// 	<c>true</c> if the specified input is decimal; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDecimal(this string input, Predicate<decimal> secondaryTest)
        {
            decimal testValue;
            bool test = decimal.TryParse(input, out testValue);

            if ((secondaryTest != null) && test) return secondaryTest(testValue);
            else return test;
        }

        /// <summary>
        /// Determines whether the specified input is integer.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        ///   <c>true</c> if the specified input is integer; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInteger(this string input)
        {
            return input.IsInteger(null);
        }

        /// <summary>
        /// Determines whether the specified input is integer.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="secondaryTest">The secondary test.</param>
        /// <returns>
        ///   <c>true</c> if the specified input is integer; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInteger(this string input, Predicate<int> secondaryTest)
        {
            int testValue;
            bool test = int.TryParse(input, out testValue);

            if ((secondaryTest != null) && test) return secondaryTest(testValue);
            else return test;
        }

        /// <summary>
        /// Determines whether the specified input is long.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// 	<c>true</c> if the specified input is long; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLong(this string input)
        {
            return input.IsLong(null);
        }

        /// <summary>
        /// Determines whether the specified input is long.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="secondaryTest">The secondary test.</param>
        /// <returns>
        /// 	<c>true</c> if the specified input is long; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLong(this string input, Predicate<long> secondaryTest)
        {
            long testValue;
            bool test = long.TryParse(input, out testValue);

            if ((secondaryTest != null) && test) return secondaryTest(testValue);
            else return test;
        }

        /// <summary>
        /// Determines whether the specified input is a telephone number.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        ///   <c>true</c> if is telephone number; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsTelephoneNumber(this string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            return Regex.IsMatch(input, @"^\(?[0-9]\d{2}\)?([-., ])?[0-9]\d{2}([-., ])?\d{4}$");
        }

        /// <summary>
        /// Determines whether the specified input looks like an email address.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        ///   <c>true</c> if seems to be an email address; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// “In short, don’t expect a single, usable regex to do a proper job.
        /// And the best regex will validate the syntax, not the validity
        /// of an e-mail (jhohn@example.com is correct but it will probably bounce…).”
        /// [http://stackoverflow.com/questions/201323/how-to-use-a-regular-expression-to-validate-an-email-addresses]
        /// </remarks>
        public static bool LooksLikeEmailAddress(this string input)
        {
            return Regex.IsMatch(input, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
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
        /// Truncates the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="length">The length.</param>
        public static string Truncate(this string input, int length = 16)
        {
            if (string.IsNullOrEmpty(input)) return input;
            if (input.Length < length) return input;
            return string.Concat(input.Substring(0, length), "…");
        }
    }
}
