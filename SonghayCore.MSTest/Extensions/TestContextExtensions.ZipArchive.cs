#if NETSTANDARD

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="TestContext"/>.
    /// </summary>
    public static partial class TestContextExtensions
    {
        /// <summary>
        /// Should read zip archive entries.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="archiveFile">The archive file.</param>
        /// <param name="fileAction">The file action.</param>
        public static void ShouldReadZipArchiveEntries(this TestContext context, string archiveFile, Action<string> fileAction)
        {
            context.ShouldReadZipArchiveEntries(archiveFile, fileAction, filterZipArchiveEntries: null);
        }

        /// <summary>
        /// Should read zip archive entries.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="archiveFile">The archive file.</param>
        /// <param name="fileAction">The file action.</param>
        /// <param name="filterZipArchiveEntries">The filter zip archive entries.</param>
        public static void ShouldReadZipArchiveEntries(this TestContext context, string archiveFile, Action<string> fileAction, Func<ZipArchiveEntry, bool> filterZipArchiveEntries)
        {
            context.ShouldFindFile(archiveFile);

            Assert.IsNotNull(fileAction, "The expected ZIP archive file action is not here.");

            if (filterZipArchiveEntries == null) filterZipArchiveEntries = i => true;

            using (var stream = new FileStream(archiveFile, FileMode.Open))
            using (var zipFile = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: false))
            {
                zipFile.Entries.Where(filterZipArchiveEntries).ForEachInEnumerable(i =>
                {
                    context.WriteLine(i.FullName);
                    using (var entryStream = new StreamReader(i.Open()))
                    {
                        var s = entryStream.ReadToEnd();
                        fileAction.Invoke(s);
                    }
                });
            }
        }

        /// <summary>
        /// Should read zip archive entries by line.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="archiveFile">The archive file.</param>
        /// <param name="fileAction">The file action.</param>
        /// <remarks>
        /// The <c>fileAction</c> includes the line number and the current line.
        /// </remarks>
        public static void ShouldReadZipArchiveEntriesByLine(this TestContext context, string archiveFile, Action<int, string> lineAction)
        {
            context.ShouldReadZipArchiveEntriesByLine(archiveFile, lineAction, filterZipArchiveEntries: null);
        }

        /// <summary>
        /// Should read zip archive entries by line.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="archiveFile">The archive file.</param>
        /// <param name="fileAction">The file action.</param>
        /// <param name="filterZipArchiveEntries">The filter zip archive entries.</param>
        /// <remarks>
        /// The <c>fileAction</c> includes the line number and the current line.
        /// </remarks>
        public static void ShouldReadZipArchiveEntriesByLine(this TestContext context, string archiveFile, Action<int, string> lineAction, Func<ZipArchiveEntry, bool> filterZipArchiveEntries)
        {
            context.ShouldFindFile(archiveFile);

            Assert.IsNotNull(lineAction, "The expected ZIP archive file action is not here.");

            if (filterZipArchiveEntries == null) filterZipArchiveEntries = i => true;

            using (var stream = new FileStream(archiveFile, FileMode.Open))
            using (var zipFile = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: false))
            {
                zipFile.Entries.Where(filterZipArchiveEntries).ForEachInEnumerable(i =>
                {
                    context.WriteLine(i.FullName);
                    using (var entryStream = new StreamReader(i.Open()))
                    {
                        var line = default(string);
                        var lineNumber = 0;
                        while ((line = entryStream.ReadLine()) != null)
                        {
                            ++lineNumber;
                            lineAction.Invoke(lineNumber, line);
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Should write zip archive entry.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="archiveFile">The archive file.</param>
        /// <param name="fileInfo">The file information.</param>
        public static void ShouldWriteZipArchiveEntry(this TestContext context, string archiveFile, FileInfo fileInfo)
        {
            context.ShouldFindFile(archiveFile);

            Assert.IsNotNull(fileInfo, "The expected ZIP archive file info is not here.");
            context.ShouldFindFile(fileInfo.FullName);

            using (var stream = new FileStream(archiveFile, FileMode.Open))
            using (var zipFile = new ZipArchive(stream, ZipArchiveMode.Update, leaveOpen: false))
            {
                var entry = zipFile.CreateEntry(fileInfo.Name, CompressionLevel.Optimal);
                using (var writer = new StreamWriter(entry.Open()))
                {
                    var s = File.ReadAllText(fileInfo.FullName);
                    writer.Write(s);
                    writer.Flush();
                }
            }
        }
    }
}

#endif
