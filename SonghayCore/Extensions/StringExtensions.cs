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
        /// Returns <c>true</c> when the strings are equal without regard to cultural locales
        /// or casing.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="otherString"></param>
        public static bool EqualsInvariant(this string input, string otherString)
        {
            return input.EqualsInvariant(otherString, ignoreCase: true);
        }

        /// <summary>
        /// Returns <c>true</c> when the strings are equal without regard to cultural locales.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="otherString"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static bool EqualsInvariant(this string input, string otherString, bool ignoreCase)
        {
            return ignoreCase ?
                string.Equals(input, otherString, StringComparison.InvariantCultureIgnoreCase)
                :
                string.Equals(input, otherString, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Converts camel-case <see cref="System.String"/> to <see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/>.
        /// </summary>
        /// <param name="input">The input.</param>
        public static IEnumerable<string> FromCamelCaseToEnumerable(this string input)
        {
            return new string(input.InsertSpacesBeforeCaps().ToArray())
                .Split(' ').Where(i => !string.IsNullOrWhiteSpace(i));
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
            if (string.IsNullOrWhiteSpace(input)) return false;
            if (string.IsNullOrWhiteSpace(delimitedValues)) return false;

            var set = delimitedValues.Split(separator);
            return set.Contains(input);
        }

        /// <summary>
        /// Inserts the spaces before caps.
        /// </summary>
        /// <param name="input">The input.</param>
        public static IEnumerable<char> InsertSpacesBeforeCaps(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) yield return '\0';
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
            if (string.IsNullOrWhiteSpace(input)) return false;
            return Regex.IsMatch(input, @"^\(?[0-9]\d{2}\)?([-., ])?[0-9]\d{2}([-., ])?\d{4}$");
        }

        /// <summary>
        /// Determines whether the specified input is a UNC.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        ///   <c>true</c> if is a UNC; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        ///  📚 https://stackoverflow.com/a/47531093/22944
        ///  📚 https://en.wikipedia.org/wiki/Path_(computing)#Uniform_Naming_Convention
        /// </remarks>
        public static bool IsUnc(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;
            return Regex.IsMatch(input, @"^(\\(\\[^\s\\]+)+|([A-Za-z]:(\\)?|[A-z]:(\\[^\s\\]+)+))(\\)?$");
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
        /// Returns the number of directory levels
        /// based on the conventions <c>../</c> or <c>..\</c>.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static int ToNumberOfDirectoryLevels(this string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return 0;
            var relative_parent_path_matches = Regex.Matches(path, @"\.\./|\.\.\\");
            return relative_parent_path_matches.Count;
        }
    }
}
