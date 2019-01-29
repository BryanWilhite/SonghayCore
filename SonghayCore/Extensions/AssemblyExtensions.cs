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

            var proposedLocation = (string.IsNullOrEmpty(assembly.CodeBase))
                ? assembly.Location
                : assembly.CodeBase
                    .Replace("file:///", string.Empty);

            var root = Path.GetDirectoryName(proposedLocation);
            return root;
        }
    }
}
