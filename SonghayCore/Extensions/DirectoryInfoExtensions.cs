using System;
using System.IO;

namespace Songhay.Extensions
{
    /// <summary>Extensions of <see cref="DirectoryInfo"/>.</summary>
    public static class DirectoryInfoExtensions
    {
        /// <summary>Gets the parent directory.</summary>
        /// <param name="directoryInfo">The directory information.</param>
        /// <param name="levels">The levels.</param>
        /// <returns></returns>
        public static string GetParentDirectory(this DirectoryInfo directoryInfo, int levels)
        {
            var info = directoryInfo.GetParentDirectoryInfo(levels);
            return info?.FullName;
        }

        /// <summary>Gets the parent <see cref="DirectoryInfo"/>.</summary>
        /// <param name="directoryInfo">The directory information.</param>
        /// <param name="levels">The levels.</param>
        /// <returns></returns>
        public static DirectoryInfo GetParentDirectoryInfo(this DirectoryInfo directoryInfo, int levels)
        {
            if (directoryInfo == null) throw new NullReferenceException($"The expected {nameof(DirectoryInfo)} is not here.");

            levels = Math.Abs(levels);
            if (levels == 0) return directoryInfo;

            var parentDirectoryInfo = directoryInfo.Parent;
            if (parentDirectoryInfo == null) return directoryInfo;

            --levels;
            if (levels >= 1) return parentDirectoryInfo.GetParentDirectoryInfo(levels);
            return parentDirectoryInfo;
        }

        /// <summary>Combines path and root based
        /// on the current value of <see cref="Path.DirectorySeparatorChar"/>
        /// of the current OS.</summary>
        /// <param name="directoryInfo">The directory information.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">The expected root path is not here.
        /// or
        /// The expected path is not here.</exception>
        /// <remarks>For detail, see https://github.com/BryanWilhite/SonghayCore/issues/14.</remarks>
        public static string ToCombinedPath(this DirectoryInfo directoryInfo, string path)
        {
            if (directoryInfo == null) throw new NullReferenceException($"The expected {nameof(DirectoryInfo)} is not here.");
            if (string.IsNullOrEmpty(path)) throw new NullReferenceException("The expected path is not here.");

            var root = directoryInfo.FullName;
            var backSlash = '\\';
            var forwardSlash = '/';
            var isForwardSlashSystem = Path.DirectorySeparatorChar.Equals(forwardSlash);

            string Normalize(string r)
            {
                return isForwardSlashSystem ?
                    r.Replace(backSlash, forwardSlash)
                    :
                    r.Replace(forwardSlash, backSlash);
            }

            string RemoveConventionalPrefixes(string r)
            {
                return isForwardSlashSystem ?
                    r
                    .TrimStart(forwardSlash)
                    .Replace($"..{forwardSlash}", string.Empty)
                    .Replace($".{forwardSlash}", string.Empty)
                    :
                    r
                    .TrimStart(backSlash)
                    .Replace($"..{backSlash}", string.Empty)
                    .Replace($".{backSlash}", string.Empty)
                    ;
            }

            return Path.Combine(Normalize(root), RemoveConventionalPrefixes(Normalize(path)));
        }
    }
}
