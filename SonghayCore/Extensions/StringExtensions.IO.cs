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
        /// Combines path and root based
        /// on the current value of <see cref="Path.DirectorySeparatorChar"/>
        /// of the current OS.
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
        /// For detail, see https://github.com/BryanWilhite/SonghayCore/issues/14.
        /// </remarks>
        public static string ToCombinedPath(this string root, string path)
        {
            if (string.IsNullOrEmpty(root)) throw new NullReferenceException("The expected root path is not here.");
            if (string.IsNullOrEmpty(path)) throw new NullReferenceException("The expected path is not here.");
            string normalize(string r)
            {
                return Path.DirectorySeparatorChar.Equals('/') ?
                    r.Replace('\\', '/') :
                    r.Replace('/', '\\');
            }
            return Path.Combine(normalize(root), normalize(path).TrimStart(new [] { '/', '\\' }));
        }
    }
}