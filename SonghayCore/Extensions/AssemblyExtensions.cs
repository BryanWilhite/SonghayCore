using System;
using System.IO;
using System.Reflection;

namespace Songhay.Extensions
{
    /// <summary>Extensions of <see cref="Assembly"/>.</summary>
    public static class AssemblyExtensions
    {
        /// <summary>Gets the path from assembly.</summary>
        /// <param name="assembly">The assembly.</param>
        /// <exception cref="ArgumentNullException">assembly - The expected assembly is not here.</exception>
        public static string GetPathFromAssembly(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly), "The expected assembly is not here.");

            var hasCodeBaseOnWindows =
                !string.IsNullOrEmpty(assembly.CodeBase)
                &&
                !FrameworkFileUtility.IsForwardSlashSystem()
                ;

            var location = hasCodeBaseOnWindows ?
                assembly.CodeBase.Replace("file:///", string.Empty) :
                assembly.Location;

            var root = Path.GetDirectoryName(location);
            return root;
        }
    }
}
