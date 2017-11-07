using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="TestContext"/>
    /// </summary>
    public static partial class TestContextExtensions
    {
        /// <summary>
        /// Test context extensions: should find file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="path">The path.</param>
        public static void ShouldFindFile(this TestContext context, string path)
        {
            context.WriteLine("Finding file: {0}...", path);
            Assert.IsTrue(File.Exists(path), "The expected file, {0}, is not here.", path);
        }

        /// <summary>
        /// Test context extensions: should find folder.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="path">The path.</param>
        public static void ShouldFindFolder(this TestContext context, string path)
        {
            context.WriteLine("Finding folder: {0}...", path);
            Assert.IsTrue(Directory.Exists(path), "The expected folder, {0}, is not here.", path);
        }

        /// <summary>
        /// Test context extensions: should get assembly directory.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="typeInAssembly">The type in assembly.</param>
        /// <returns></returns>
        public static string ShouldGetAssemblyDirectory(this TestContext context, Type typeInAssembly)
        {
            Assert.IsNotNull(typeInAssembly, "The expected type instance is not here.");

            var assembly = typeInAssembly.Assembly;
            var path = FrameworkAssemblyUtility.GetPathFromAssembly(assembly);
            context.ShouldFindFolder(path);

            return path;
        }

        /// <summary>
        /// Test context extensions: should get assembly directory information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="typeInAssembly">The type in assembly.</param>
        /// <returns></returns>
        public static DirectoryInfo ShouldGetAssemblyDirectoryInfo(this TestContext context, Type typeInAssembly)
        {
            var info = new DirectoryInfo(context.ShouldGetAssemblyDirectory(typeInAssembly));
            return info;
        }

        /// <summary>
        /// Test context extensions: should get assembly directory.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="typeInAssembly">The type in assembly.</param>
        /// <param name="expectedLevels">The expected levels of directories above the assembly.</param>
        /// <returns></returns>
        public static string ShouldGetAssemblyDirectoryParent(this TestContext context, Type typeInAssembly, int expectedLevels)
        {
            var path = context.ShouldGetAssemblyDirectory(typeInAssembly);
            path = FrameworkFileUtility.GetParentDirectory(path, expectedLevels);
            context.ShouldFindFolder(path);
            return path;
        }

        /// <summary>
        /// Test context extensions: should load list of strings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="path">The path.</param>
        public static IEnumerable<string> ShouldLoadListOfStrings(this TestContext context, string path)
        {
            context.ShouldFindFile(path);
            return File.ReadAllLines(path);
        }
    }
}