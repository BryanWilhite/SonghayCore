using System;
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
            return FrameworkAssemblyUtility.GetPathFromAssembly(assembly);
        }

        /// <summary>Gets the path from assembly.</summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="fileSegment">The file segment.</param>
        /// <exception cref="ArgumentNullException">assembly - The expected assembly is not here.</exception>
        public static string GetPathFromAssembly(this Assembly assembly, string fileSegment)
        {
            return FrameworkAssemblyUtility.GetPathFromAssembly(assembly, fileSegment);
        }
    }
}
