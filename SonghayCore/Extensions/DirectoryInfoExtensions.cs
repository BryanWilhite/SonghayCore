using System;
using System.IO;
using System.Linq;

namespace Songhay.Extensions
{
    /// <summary>Extensions of <see cref="DirectoryInfo"/>.</summary>
    public static class DirectoryInfoExtensions
    {
        /// <summary>
        /// Finds the specified sub <see cref="DirectoryInfo"/>
        /// under the specified <see cref="DirectoryInfo"/>.
        /// </summary>
        /// <param name="directoryInfo">the specified <see cref="DirectoryInfo"/></param>
        /// <param name="expectedDirectoryName">the specified sub <see cref="DirectoryInfo.Name"/></param>
        /// <returns></returns>
        public static DirectoryInfo FindDirectory(this DirectoryInfo directoryInfo, string expectedDirectoryName)
        {
            if (directoryInfo == null)
                throw new DirectoryNotFoundException("The expected directory is not here.");

            if (!directoryInfo.Exists)
                throw new DirectoryNotFoundException("The expected directory does not exist.");

            var subDirectoryInfo = directoryInfo.GetDirectories(expectedDirectoryName).FirstOrDefault();

            if (subDirectoryInfo == null)
                throw new DirectoryNotFoundException("The expected directory is not here.");

            return subDirectoryInfo;
        }

        /// <summary>
        /// Finds the specified <see cref="FileInfo"/>
        /// under the specified <see cref="DirectoryInfo"/>.
        /// </summary>
        /// <param name="directoryInfo">the specified <see cref="DirectoryInfo"/></param>
        /// <param name="expectedFileName">the specified <see cref="FileInfo.Name"/></param>
        /// <returns></returns>
        public static FileInfo FindFile(this DirectoryInfo directoryInfo, string expectedFileName)
        {
            if (directoryInfo == null)
                throw new DirectoryNotFoundException("The expected directory is not here.");

            if (!directoryInfo.Exists)
                throw new DirectoryNotFoundException("The expected directory does not exist.");

            var fileInfo = directoryInfo.GetFiles(expectedFileName).FirstOrDefault();

            if (fileInfo == null)
                throw new FileNotFoundException("The expected file is not here.");

            return fileInfo;
        }

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
            return FrameworkFileUtility.GetCombinedPath(directoryInfo.FullName, path);
        }

        /// <summary>
        /// Verifies the specified <see cref="DirectoryInfo"/>
        /// with conventional error handling.
        /// </summary>
        /// <param name="directoryInfo">the specified <see cref="DirectoryInfo"/></param>
        /// <param name="expectedDirectoryName">the expected directory name</param>
        /// <returns></returns>
        public static void VerifyDirectory(this DirectoryInfo directoryInfo, string expectedDirectoryName)
        {
            if (directoryInfo == null)
                throw new DirectoryNotFoundException("The expected directory is not here.");

            if (!directoryInfo.Exists)
                throw new DirectoryNotFoundException("The expected directory does not exist.");

            if (!directoryInfo.Name.EqualsInvariant(expectedDirectoryName))
                throw new DirectoryNotFoundException($"The expected directory is not here. [actual: { expectedDirectoryName ?? "[name]" }");
        }
    }
}
