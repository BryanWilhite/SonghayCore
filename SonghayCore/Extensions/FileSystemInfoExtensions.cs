using Songhay.Diagnostics;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="FileSystemInfo"/>
    /// </summary>
    public static class FileSystemInfoExtensions
    {
        static FileSystemInfoExtensions() => traceSource = TraceSources.Instance.GetConfiguredTraceSource();

        static readonly TraceSource traceSource;

        /// <summary>
        /// Read zip archive entries.
        /// </summary>
        /// <param name="info">The file and/or directory info.</param>
        /// <param name="archiveFile">The archive file.</param>
        /// <param name="fileAction">The file action.</param>
        public static void ReadZipArchiveEntries(this FileSystemInfo info, string archiveFile, Action<string> fileAction)
        {
            info.ReadZipArchiveEntries(archiveFile, fileAction, filterZipArchiveEntries: null);
        }

        /// <summary>
        /// Read zip archive entries.
        /// </summary>
        /// <param name="info">The file and/or directory info.</param>
        /// <param name="archiveFile">The archive file.</param>
        /// <param name="fileAction">The file action.</param>
        /// <param name="filterZipArchiveEntries">The filter zip archive entries.</param>
        public static void ReadZipArchiveEntries(this FileSystemInfo info, string archiveFile, Action<string> fileAction, Func<ZipArchiveEntry, bool> filterZipArchiveEntries)
        {
            if (info == null) throw new NullReferenceException("The expected file or directory info is not here.");
            if (!info.Exists) throw new FileNotFoundException($"The expected file {info.FullName} is not here.");
            if (fileAction == null) throw new NullReferenceException("The expected ZIP archive file action is not here.");

            if (filterZipArchiveEntries == null) filterZipArchiveEntries = i => true;

            using (var stream = new FileStream(archiveFile, FileMode.Open))
            using (var zipFile = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: false))
            {
                zipFile.Entries.Where(filterZipArchiveEntries).ForEachInEnumerable(i =>
                {
                    traceSource?.TraceVerbose(i.FullName);
                    using (var entryStream = new StreamReader(i.Open()))
                    {
                        var s = entryStream.ReadToEnd();
                        fileAction.Invoke(s);
                    }
                });
            }
        }

        /// <summary>
        /// Read zip archive entries by line.
        /// </summary>
        /// <param name="info">The file and/or directory info.</param>
        /// <param name="archiveFile">The archive file.</param>
        /// <param name="lineAction">The line action.</param>
        /// <remarks>
        /// The <c>fileAction</c> includes the line number and the current line.
        /// </remarks>
        public static void ReadZipArchiveEntriesByLine(this FileSystemInfo info, string archiveFile, Action<int, string> lineAction)
        {
            info.ReadZipArchiveEntriesByLine(archiveFile, lineAction, filterZipArchiveEntries: null);
        }

        /// <summary>
        /// Read zip archive entries by line.
        /// </summary>
        /// <param name="info">The file and/or directory info.</param>
        /// <param name="archiveFile">The archive file.</param>
        /// <param name="lineAction">The line action.</param>
        /// <param name="filterZipArchiveEntries">The filter zip archive entries.</param>
        /// <remarks>
        /// The <c>fileAction</c> includes the line number and the current line.
        /// </remarks>
        public static void ReadZipArchiveEntriesByLine(this FileSystemInfo info, string archiveFile, Action<int, string> lineAction, Func<ZipArchiveEntry, bool> filterZipArchiveEntries)
        {
            if (info == null) throw new NullReferenceException("The expected file or directory info is not here.");
            if (!info.Exists) throw new FileNotFoundException($"The expected file {info.FullName} is not here.");
            if (lineAction == null) throw new NullReferenceException("The expected ZIP archive file action is not here.");

            if (filterZipArchiveEntries == null) filterZipArchiveEntries = i => true;

            using (var stream = new FileStream(archiveFile, FileMode.Open))
            using (var zipFile = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: false))
            {
                zipFile.Entries.Where(filterZipArchiveEntries).ForEachInEnumerable(i =>
                {
                    traceSource?.TraceVerbose(i.FullName);
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
        /// Write zip archive entry with <see cref="CompressionLevel.Optimal"/>.
        /// </summary>
        /// <param name="info">The file and/or directory info.</param>
        /// <param name="archiveFile">The archive file.</param>
        /// <param name="fileInfo">The file information.</param>
        public static void WriteZipArchiveEntry(this FileSystemInfo info, string archiveFile, FileInfo fileInfo)
        {
            info.WriteZipArchiveEntry(archiveFile, fileInfo, CompressionLevel.Optimal);
        }

        /// <summary>
        /// Write zip archive entry.
        /// </summary>
        /// <param name="info">The file and/or directory info.</param>
        /// <param name="archiveFile">The archive file.</param>
        /// <param name="fileInfo">The file information.</param>
        /// <param name="compressionLevel">The <see cref="CompressionLevel"/></param>
        public static void WriteZipArchiveEntry(this FileSystemInfo info, string archiveFile, FileInfo fileInfo, CompressionLevel compressionLevel)
        {
            if (info == null) throw new NullReferenceException("The expected file or directory info is not here.");
            if (!info.Exists) throw new FileNotFoundException($"The expected file {info.FullName} is not here.");

            if (fileInfo == null) throw new NullReferenceException("The expected ZIP archive file info entry is not here.");
            if (!fileInfo.Exists) throw new FileNotFoundException($"The expected ZIP archive file {fileInfo.FullName} is not here.");

            using (var stream = new FileStream(archiveFile, FileMode.Open))
            using (var zipFile = new ZipArchive(stream, ZipArchiveMode.Update, leaveOpen: false))
            {
                var entry = zipFile.CreateEntry(fileInfo.Name, compressionLevel);
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
