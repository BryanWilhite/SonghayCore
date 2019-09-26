using System.Linq;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="string"/>.
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// Converts the <see cref="string"/> into a numeric format for parsing.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// Returns a numeric string ready for integer or float parsing.
        /// </returns>
        public static string ToNumericString(this string input)
        {
            return input.ToNumericString("0");
        }

        /// <summary>
        /// Converts the <see cref="string"/> into a numeric format for parsing.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="defaultValue">The default value ("0" by default).</param>
        /// <returns>
        /// Returns a numeric string ready for integer or float parsing.
        /// </returns>
        public static string ToNumericString(this string input, string defaultValue)
        {
            if(string.IsNullOrWhiteSpace(input)) return defaultValue;
            if(string.IsNullOrWhiteSpace(input)) return defaultValue;
            return new string(input.Trim().Where(i => char.IsDigit(i) || i.Equals('.')).ToArray());
        }
    }
}
