using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="TestContext"/>
    /// </summary>
    public static partial class TestContextExtensions
    {
        /// <summary>
        /// Test context extensions: should find the specified file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="path">The path.</param>
        public static void ShouldFindFile(this TestContext context, string path)
        {
            context.WriteLine("Finding file: {0}...", path);
            Assert.IsTrue(File.Exists(path), "The expected file, {0}, is not here.", path);
        }

        /// <summary>
        /// Test context extensions: should find the specified directory.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="path">The path.</param>
        public static void ShouldFindDirectory(this TestContext context, string path)
        {
            context.WriteLine("Finding directory: {0}...", path);
            Assert.IsTrue(Directory.Exists(path), "The expected directory, {0}, is not here.", path);
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
            context.ShouldFindDirectory(path);

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
            context.ShouldFindDirectory(path);
            return path;
        }

        /// <summary>
        /// Should get conventional project directory information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <remarks>
        /// The conventional project would be <c>MyCompany.Foo</c>
        /// for test project <c>MyCompany.Foo.Tests</c>.
        /// </remarks>
        public static DirectoryInfo ShouldGetConventionalProjectDirectoryInfo(this TestContext context, Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "The expected test-class Type is not here.");

            var projectDirectoryName = type.Namespace.Split('.').Take(2).Aggregate((a, i) => string.Concat(a, ".", i));
            var directoryInfo = context.ShouldGetSiblingDirectoryInfoByName(type, projectDirectoryName);
            return directoryInfo;
        }

        /// <summary>
        /// Should get sibling directory information by name.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="type">The type.</param>
        /// <param name="projectDirectoryName">Name of the project.</param>
        /// <returns></returns>
        /// <remarks>
        /// The sibling directory info is the output
        /// from <see cref="ShouldGetProjectDirectoryInfo"/>.
        /// </remarks>
        public static DirectoryInfo ShouldGetSiblingDirectoryInfoByName(this TestContext context, Type type, string projectDirectoryName)
        {

            var projectDirectoryInfo = context.ShouldGetProjectDirectoryInfo(type);
            var targetProjectDirectoryInfo = projectDirectoryInfo.Parent.GetDirectories().SingleOrDefault(i => i.Name.Equals(projectDirectoryName));
            context.ShouldFindDirectory(targetProjectDirectoryInfo?.FullName);

            return targetProjectDirectoryInfo;
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