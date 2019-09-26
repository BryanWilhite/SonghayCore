using Songhay.Diagnostics;
using Songhay.Extensions;
using System;
using System.Diagnostics;
using System.IO;
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
            backSlash = '\\';
            forwardSlash = '/';
            isForwardSlashSystem = Path.DirectorySeparatorChar.Equals(forwardSlash);
            traceSource = TraceSources.Instance.GetConfiguredTraceSource();
        }

        static readonly TraceSource traceSource;

        /// <summary>
        /// Counts the parent directory chars.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <remarks>
        /// This method is useful when running <see cref="GetParentDirectory(string, int)"/>.
        /// 
        /// WARNING: call <see cref="NormalizePath(string)"/> to prevent incorrectly returning <c>0</c>
        /// in cross-platform scenarios.
        /// </remarks>
        public static int CountParentDirectoryChars(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return default(int);

            var parentDirectoryCharsPattern = string.Format(@"\.\.\{0}", Path.DirectorySeparatorChar);
            var matches = Regex.Matches(path, parentDirectoryCharsPattern);
            return matches.Count;
        }

        /// <summary>
        /// Finds the parent directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="parentName">Name of the parent.</param>
        /// <param name="levels">The levels.</param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException">The expected directory is not here.</exception>
        public static string FindParentDirectory(string path, string parentName, int levels)
        {
            return FindParentDirectoryInfo(path, parentName, levels)?.FullName;
        }

        /// <summary>
        /// Finds the parent <see cref="DirectoryInfo"/>.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="parentName">Name of the parent.</param>
        /// <param name="levels">The levels.</param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException">The expected directory is not here.</exception>
        public static DirectoryInfo FindParentDirectoryInfo(string path, string parentName, int levels)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new DirectoryNotFoundException("The expected directory is not here.");

            var info = new DirectoryInfo(path);

            var isParent = (info.Name == parentName);
            var hasNullParent = (info.Parent == null);
            var hasTargetParent = !hasNullParent && (info.Parent.Name == parentName);

            if (!info.Exists) return null;
            if (isParent) return info;
            if (hasNullParent) return null;
            if (hasTargetParent) return info.Parent;

            levels = Math.Abs(levels);
            --levels;

            var hasNoMoreLevels = (levels == 0);

            if (hasNoMoreLevels) return null;

            return FindParentDirectoryInfo(info.Parent.FullName, parentName, levels);
        }

        /// <summary>Combines path and root based
        /// on the ambient value of <see cref="Path.DirectorySeparatorChar"/>
        /// of the current OS.</summary>
        /// <param name="root">The root.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">The expected root path is not here.
        /// or
        /// The expected path is not here.</exception>
        /// <remarks>
        /// For detail, see:
        /// 📚 https://github.com/BryanWilhite/SonghayCore/issues/14
        /// 📚 https://github.com/BryanWilhite/SonghayCore/issues/32
        /// </remarks>
        public static string GetCombinedPath(string root, string path)
        {
            if (string.IsNullOrWhiteSpace(root)) throw new NullReferenceException($"The expected {nameof(root)} is not here.");
            if (string.IsNullOrWhiteSpace(path)) throw new NullReferenceException($"The expected {nameof(path)} is not here.");

            return Path.Combine(NormalizePath(root), RemoveConventionalPrefixes(NormalizePath(path)));
        }

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
            if (string.IsNullOrWhiteSpace(path)) return path;

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
        /// Gets the parent <see cref="DirectoryInfo"/>.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="levels">The levels.</param>
        /// <returns></returns>
        /// <remarks>
        /// A recursive wrapper for <see cref="Directory.GetParent(string)"/>.
        /// </remarks>
        public static DirectoryInfo GetParentDirectoryInfo(string path, int levels)
        {
            if (string.IsNullOrWhiteSpace(path)) return null;

            var info = new DirectoryInfo(path);

            levels = Math.Abs(levels);
            if (levels == 0) return info;

            if (info.Parent == null) return info;
            path = info.FullName;

            --levels;
            return (levels >= 1) ? GetParentDirectoryInfo(path, levels) : info;
        }

        /// <summary>
        /// Returns <c>true</c> when the current OS
        /// uses forward-slash (<c>/</c>) paths or not.
        /// </summary>
        /// <returns></returns>
        public static bool IsForwardSlashSystem()
        {
            return isForwardSlashSystem;
        }

        /// <summary>
        /// Normalizes the specified path with respect
        /// to the ambient value of <see cref="Path.DirectorySeparatorChar"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string NormalizePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return null;

            return isForwardSlashSystem ?
                path.Replace(backSlash, forwardSlash)
                :
                path.Replace(forwardSlash, backSlash);
        }

        /// <summary>
        /// Removes conventional Directory prefixes
        /// for relative paths, e.g. <c>../</c> or <c>./</c>
        /// based on the ambient value of <see cref="Path.DirectorySeparatorChar"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string RemoveConventionalPrefixes(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return null;

            return isForwardSlashSystem ?
                path
                .TrimStart(forwardSlash)
                .Replace($"..{forwardSlash}", string.Empty)
                .Replace($".{forwardSlash}", string.Empty)
                :
                path
                .TrimStart(backSlash)
                .Replace($"..{backSlash}", string.Empty)
                .Replace($".{backSlash}", string.Empty)
                ;
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
            if (string.IsNullOrWhiteSpace(path)) return path;
            return path.TrimStart(new[] { backSlash, forwardSlash });
        }

        static readonly bool isForwardSlashSystem;
        static readonly char backSlash;
        static readonly char forwardSlash;
    }
}
