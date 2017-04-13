using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="TestContext"/>
    /// </summary>
    public static class TestContextExtensions
    {
        /// <summary>
        /// Gets the default process start information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="workingDirectory">The working directory.</param>
        /// <returns></returns>
        public static ProcessStartInfo GetDefaultProcessStartInfo(this TestContext context, string arguments, string fileName, string workingDirectory)
        {
            context.ShouldFindFile(fileName);
            context.ShouldFindFolder(workingDirectory);

            var startInfo = new ProcessStartInfo
            {
                Arguments = arguments,
                CreateNoWindow = true,
                FileName = fileName,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = workingDirectory
            };

            return startInfo;
        }

        /// <summary>
        /// Removes the previous test results.
        /// </summary>
        /// <param name="context">The context.</param>
        public static void RemovePreviousTestResults(this TestContext context)
        {
            if (context == null) return;

            var predicate = FuncSeed.True<FileInfo>()
                .And(f => f.Extension != ".ldf")
                .And(f => f.Extension != ".mdf");

            var directory = Directory.GetParent(context.TestDir);
            directory.EnumerateFiles()
                    .Where(predicate)
                    .OrderByDescending(f => f.LastAccessTime).Skip(1)
                    .ForEachInEnumerable(f => f.Delete());
            directory.EnumerateDirectories()
                    .OrderByDescending(d => d.LastAccessTime).Skip(1)
                    .ForEachInEnumerable(d => d.Delete(true));
        }

        /// <summary>
        /// Starts the <see cref="Process"/> and waits for exit.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="startInfo">The start information.</param>
        public static void StartProcessAndWaitForExit(this TestContext context, ProcessStartInfo startInfo)
        {
            using (var process = new Process())
            {
                process.StartInfo = startInfo;

                process.ErrorDataReceived += (s, args) => context.WriteLine(args.Data);

                process.OutputDataReceived += (s, args) => context.WriteLine(args.Data);

                process.Start();
                process.WaitForExit();
            }
        }

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
        /// Test context extensions: should get projects folder.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="typeInAssembly">The type in assembly.</param>
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