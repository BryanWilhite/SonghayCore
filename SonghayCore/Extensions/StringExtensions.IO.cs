using System;
using System.IO;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="System.String"/>.
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// Converts to a path based on the current OS.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">
        /// The expected root path is not here.
        /// or
        /// The expected path is not here.
        /// </exception>
        /// <remarks>
        /// For detail, see https://stackoverflow.com/a/3146854/22944.
        /// </remarks>
        public static string ToCombinedFullPath(this string root, string path)
        {
            if (string.IsNullOrEmpty(root)) throw new NullReferenceException("The expected root path is not here.");
            if (string.IsNullOrEmpty(path)) throw new NullReferenceException("The expected path is not here.");
            return Path.GetFullPath(Path.Combine(root, path));
        }
    }
}
