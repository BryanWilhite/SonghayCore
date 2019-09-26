using Songhay.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;

// <copyright file="FrameworkAssembly.cs" company="Songhay System">
//     Copyright 2008, Bryan D. Wilhite, Songhay System. All rights reserved.
// </copyright>
namespace Songhay
{
    /// <summary>
    /// Static members related to <see cref="System.Reflection"/>.
    /// </summary>
    public static partial class FrameworkAssemblyUtility
    {
        /// <summary>
        /// Returns a <see cref="System.String"/>
        /// about the executing assembly.
        /// </summary>
        /// <param name="targetAssembly">
        /// The executing <see cref="System.Reflection.Assembly"/>.
        /// </param>
        /// <returns>Returns <see cref="System.String"/></returns>
        public static string GetAssemblyInfo(Assembly targetAssembly)
        {
            return GetAssemblyInfo(targetAssembly, false);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/>
        /// about the executing assembly.
        /// </summary>
        /// <param name="targetAssembly">
        /// The executing <see cref="System.Reflection.Assembly"/>.
        /// </param>
        /// <param name="useConsoleChars">
        /// When <c>true</c> selected “special” characters are formatted for the Windows Console.
        /// </param>
        /// <returns>Returns <see cref="System.String"/></returns>
        public static string GetAssemblyInfo(Assembly targetAssembly, bool useConsoleChars)
        {
            var sb = new StringBuilder();
            FrameworkAssemblyInfo info = new FrameworkAssemblyInfo(targetAssembly);

            sb.AppendFormat("{0} {1}{2}", info.AssemblyTitle, info.AssemblyVersion, Environment.NewLine);
            sb.Append(info.AssemblyDescription);
            sb.Append(Environment.NewLine);
            sb.Append(info.AssemblyCopyright);
            sb.Append(Environment.NewLine);

            return useConsoleChars ? FrameworkUtility.GetConsoleCharacters(sb.ToString()) : sb.ToString();
        }

        /// <summary>
        /// Gets the directory name from assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">assembly;The expected assembly is not here.</exception>
        public static string GetPathFromAssembly(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly), "The expected assembly is not here.");

            var hasCodeBaseOnWindows =
                !string.IsNullOrWhiteSpace(assembly.CodeBase)
                &&
                !FrameworkFileUtility.IsForwardSlashSystem()
                ;

            var location = hasCodeBaseOnWindows ?
                assembly.CodeBase.Replace("file:///", string.Empty) :
                assembly.Location;

            var root = Path.GetDirectoryName(location);
            return root;
        }

        /// <summary>
        /// Gets the path from assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="fileSegment">The file segment.</param>
        public static string GetPathFromAssembly(Assembly assembly, string fileSegment)
        {
            if (string.IsNullOrWhiteSpace(fileSegment)) throw new ArgumentNullException("fileSegment", "The expected file segment is not here.");

            fileSegment = FrameworkFileUtility.TrimLeadingDirectorySeparatorChars(fileSegment);
            if (Path.IsPathRooted(fileSegment)) throw new FormatException("The expected relative path is not here.");

            fileSegment = FrameworkFileUtility.NormalizePath(fileSegment);

            var root = GetPathFromAssembly(assembly);
            var levels = FrameworkFileUtility.CountParentDirectoryChars(fileSegment);
            if (levels > 0) root = FrameworkFileUtility.GetParentDirectory(root, levels);

            var path = FrameworkFileUtility.GetCombinedPath(root, fileSegment);
            return path;
        }
    }
}
