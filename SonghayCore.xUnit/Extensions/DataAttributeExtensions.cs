using Songhay.Extensions;
using System;
using System.IO;
using Xunit;
using Xunit.Sdk;

namespace Songhay.Tests.Extensions
{
    /// <summary>Extensions for <see cref="DataAttribute"/></summary>
    public static class DataAttributeExtensions
    {
        /// <summary>
        /// Test attribute extensions: should find the specified directory.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="path">The path.</param>
        public static void FindDirectory(this DataAttribute attribute, string path)
        {
            Assert.True(Directory.Exists(path), $"The expected directory, {path}, is not here.");
        }

        /// <summary>
        /// Test attribute extensions: should get  <see cref="DirectoryInfo"/>.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="typeInAssembly">The type in assembly.</param>
        /// <returns></returns>
        public static DirectoryInfo GetAssemblyDirectoryInfo(this DataAttribute attribute, Type typeInAssembly)
        {
            Assert.NotNull(typeInAssembly);

            var assembly = typeInAssembly.Assembly;
            var path = assembly.GetPathFromAssembly();
            attribute.FindDirectory(path);

            return new DirectoryInfo(path);
        }

        /// <summary>Gets the assembly parent directory path.</summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="typeInAssembly">The type in assembly.</param>
        /// <param name="levels">The levels of directories above the assembly.</param>
        /// <returns></returns>
        public static string GetAssemblyParentDirectory(this DataAttribute attribute, Type typeInAssembly, int levels)
        {
            var pathInfo = attribute.GetAssemblyDirectoryInfo(typeInAssembly);
            var path = pathInfo.GetParentDirectory(levels);
            attribute.FindDirectory(path);
            return path;
        }

        /// <summary>Gets the assembly parent <see cref="DirectoryInfo"/>.</summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="typeInAssembly">The type in assembly.</param>
        /// <param name="expectedLevels">The expected levels of directories above the assembly.</param>
        /// <returns></returns>
        public static DirectoryInfo GetAssemblyParentDirectoryInfo(this DataAttribute attribute, Type typeInAssembly, int expectedLevels)
        {
            var path = attribute.GetAssemblyParentDirectory(typeInAssembly, expectedLevels);
            return new DirectoryInfo(path);
        }
    }
}
