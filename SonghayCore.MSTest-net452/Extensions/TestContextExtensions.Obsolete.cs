using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
        /// Test context extensions: should find folder.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="path">The path.</param>
        [Obsolete("Naming-convention change: use TestContextExtensions.ShouldFindDirectory() instead.")]
        public static void ShouldFindFolder(this TestContext context, string path)
        {
            context.WriteLine("Finding folder: {0}...", path);
            Assert.IsTrue(Directory.Exists(path), "The expected folder, {0}, is not here.", path);
        }

        /// <summary>
        /// Test context extensions: should get projects folder.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="typeInAssembly">The type in assembly.</param>
        [Obsolete("Do use TestContextExtensions.ShouldGetAssemblyDirectoryParent() instead.")]
        public static string ShouldGetProjectsFolder(this TestContext context, Type typeInAssembly)
        {
            return context.ShouldGetProjectsFolder(typeInAssembly, namespaceArrayModifier: null);
        }

        /// <summary>
        /// Test context extensions: should get projects folder.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="typeInAssembly">The type in assembly.</param>
        /// <param name="namespaceArrayModifier">The namespace array modifier.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">The expected number of Namespace parts is not here.</exception>
        /// <remarks>
        /// A Projects Folder for this method is not the default Solution (*.sln) in its own folder.
        /// This is the unconventional layout:
        /// <code>
        /// SourceFolder\
        ///     .vs\
        ///     Foo.Solution1.sln
        ///     Foo.Solution2.sln
        ///     Foo.Solution.Project1\
        ///     Foo.Solution.Project2\
        ///     Foo.Solution.Project3\
        /// </code>
        /// </remarks>
        [Obsolete("Do use TestContextExtensions.ShouldGetAssemblyDirectoryParent() instead.")]
        public static string ShouldGetProjectsFolder(this TestContext context, Type typeInAssembly, Func<string[], string[]> namespaceArrayModifier)
        {
            var path = context.ShouldGetAssemblyDirectory(typeInAssembly);
            var namespaceArray = typeInAssembly.Namespace.Split('.');
            if (namespaceArrayModifier != null) namespaceArray = namespaceArrayModifier(namespaceArray);
            if (namespaceArray.Count() < 2) throw new ArgumentException("The expected number of Namespace parts is not here.");

            var name = string.Join(".", namespaceArray.Take(2).ToArray());
            var index = path.IndexOf(name);
            if (index < 0) throw new FileNotFoundException(string.Format("{0} was not found in the Assembly path.", name));

            path = path.Remove(index);
            context.ShouldFindFolder(path);

            return path;
        }

        /// <summary>
        /// Test context extensions: should get projects folder.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="typeInAssembly">The type in assembly.</param>
        [Obsolete("Do use TestContextExtensions.ShouldGetAssemblyDirectoryParent() instead.")]
        public static string ShouldGetSolutionFolder(this TestContext context, Type typeInAssembly)
        {
            return context.ShouldGetSolutionFolder(typeInAssembly, namespaceArrayModifier: null);
        }

        /// <summary>
        /// Test context extensions: should get projects folder.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="typeInAssembly">The type in assembly.</param>
        /// <param name="namespaceArrayModifier">The namespace array modifier.</param>
        /// <returns></returns>
        /// <remarks>
        /// This method is designed for the conventional Solution file in a folder with its projects:
        /// This is the conventional layout:
        /// <code>
        /// SourceFolder\
        ///     Foo.Solution1\
        ///         .vs\
        ///         Foo.Solution1.sln
        ///         Foo.Solution.Project1\
        ///         Foo.Solution.Project2\
        ///         Foo.Solution.Project3\
        /// </code>
        /// </remarks>
        [Obsolete("Do use TestContextExtensions.ShouldGetAssemblyDirectoryParent() instead.")]
        public static string ShouldGetSolutionFolder(this TestContext context, Type typeInAssembly, Func<string[], string[]> namespaceArrayModifier)
        {
            var path = context.ShouldGetProjectsFolder(typeInAssembly, namespaceArrayModifier);
            path = Path.Combine(path, string.Join(".", typeInAssembly.Namespace.Split('.').Take(2)), typeInAssembly.Namespace);
            context.ShouldFindFolder(path);

            return path;
        }
    }
}
