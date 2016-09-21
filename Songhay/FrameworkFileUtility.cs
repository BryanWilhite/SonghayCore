using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Songhay
{
    /// <summary>
    /// A few static helper members
    /// for <see cref="System.IO"/>.
    /// </summary>
    public static partial class FrameworkFileUtility
    {
        static FrameworkFileUtility()
        {
            parentDirectoryCharsPattern = string.Format(@"\.\.\{0}", Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Counts the parent directory chars.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <remarks>
        /// This method is useful when running <see cref="GetParentDirectory(string, int)"/>.
        /// </remarks>
        public static int CountParentDirectoryChars(string path)
        {
            if (string.IsNullOrEmpty(path)) return default(int);
            var matches = Regex.Matches(path, parentDirectoryCharsPattern);
            return matches.Count;
        }

#if !SILVERLIGHT
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
#endif

        /// <summary>
        /// Gets the parent directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="levels">The levels.</param>
        /// <returns></returns>
        /// <remarks>
        /// A recursive wrapper for <see cref="Directory.GetParent(string)"/>.
        /// </remarks>
        public static string GetParentDirectory(string path, int levels)
        {
            if (string.IsNullOrEmpty(path)) return path;

            levels = Math.Abs(levels);
            if (levels == 0) return path;

            var info = Directory.GetParent(path);
            if (info == null) return path;
            path = info.FullName;

            --levels;
            if (levels >= 1) return GetParentDirectory(path, levels);
            return path;
        }

        /// <summary>
        /// Trims the leading directory separator chars.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <remarks>
        /// Trims leading <see cref="Path.AltDirectorySeparatorChar"/> and/or <see cref="Path.DirectorySeparatorChar"/>,
        /// formatting relative paths for <see cref="Path.Combine(string, string)"/>.
        /// </remarks>
        public static string TrimLeadingDirectorySeparatorChars(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;
            return path.TrimStart(new[] { Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar });
        }

        /// <summary>
        /// Trims the parent directory chars.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string TrimParentDirectoryChars(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;

            return Regex.Replace(path, parentDirectoryCharsPattern, string.Empty);
        }

        static readonly string parentDirectoryCharsPattern;
    }
}
