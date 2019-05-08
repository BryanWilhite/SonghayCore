#if NET452 || NET462

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
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
            context.ShouldFindDirectory(workingDirectory);

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
    }
}

#endif
