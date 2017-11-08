using System;
using System.Text;

namespace Songhay
{
    /// <summary>
    /// A few static helper members
    /// for <see cref="System.IO"/>.
    /// </summary>
    public static partial class FrameworkFileUtility
    {
        /// <summary>
        /// Gets the UTF-8 encoded string.
        /// </summary>
        /// <param name="rawValue">The raw value.</param>
        public static string GetEncodedString(string rawValue)
        {
            return FrameworkFileUtility.GetEncodedString(rawValue, Encoding.UTF8);
        }

        /// <summary>
        /// Gets the encoded string.
        /// </summary>
        /// <param name="rawValue">The raw value (<see cref="System.Text.Encoding.ASCII"/> by default).</param>
        /// <param name="encoding">The encoding.</param>
        public static string GetEncodedString(string rawValue, Encoding encoding)
        {
            if (encoding == null)
                throw new ArgumentNullException("encoding",
                    "The expected encoding is not here.");

            byte[] b = Encoding.Convert(Encoding.ASCII, encoding, encoding.GetBytes(rawValue));

            return new string(Encoding.ASCII.GetChars(b));
        }
    }
}
