using Songhay.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Songhay
{
    /// <summary>
    /// A few static helper members
    /// for <see cref="System.IO"/>.
    /// </summary>
    public static partial class FrameworkFileUtility
    {
        /// <summary>
        /// Read zip archive entries.
        /// </summary>
        /// <param name="archiveInfo">The ZIP archive <see cref="FileInfo"/>.</param>
        /// <param name="fileAction">The action to take per text file.</param>
        /// <remarks>
        /// Use <c>entriesProjector</c> for any filtering or sorting.
        /// </remarks>
        public static void ReadZipArchiveEntries(FileInfo archiveInfo, Action<string> fileAction)
        {
            ReadZipArchiveEntries(archiveInfo, fileAction, entriesProjector: null);
        }

        /// <summary>
        /// Read zip archive entries.
        /// </summary>
        /// <param name="archiveInfo">The ZIP archive <see cref="FileInfo"/>.</param>
        /// <param name="fileAction">The action to take per text file.</param>
        /// <param name="entriesProjector">The entries projector for LINQ filtering/sorting.</param>
        /// <remarks>
        /// Use <c>entriesProjector</c> for any filtering or sorting.
        /// </remarks>
        public static void ReadZipArchiveEntries(FileInfo archiveInfo, Action<string> fileAction, Func<ReadOnlyCollection<ZipArchiveEntry>, IEnumerable<ZipArchiveEntry>> entriesProjector)
        {
            UseZipArchiveEntries(archiveInfo,
                entries => entries.ForEachInEnumerable(i =>
                {
                    traceSource?.TraceVerbose(i.FullName);
                    using (var entryStream = new StreamReader(i.Open()))
                    {
                        var s = entryStream.ReadToEnd();
                        fileAction.Invoke(s);
                    }
                }),
                entriesProjector);
        }

        /// <summary>
        /// Read zip archive entries as strings, line by line.
        /// </summary>
        /// <param name="archiveInfo">The ZIP archive <see cref="FileInfo"/>.</param>
        /// <param name="lineAction">The action to take per text file line.</param>
        /// <remarks>
        /// This member is designed for compressed text documents that are too large to load into memory.
        /// The <c>fileAction</c> includes the line number and the current line.
        /// </remarks>
        public static void ReadZipArchiveEntriesByLine(FileInfo archiveInfo, Action<int, string> lineAction)
        {
            ReadZipArchiveEntriesByLine(archiveInfo, lineAction, entriesProjector: null);
        }

        /// <summary>
        /// Read zip archive entries as strings, line by line.
        /// </summary>
        /// <param name="archiveInfo">The ZIP archive <see cref="FileInfo"/>.</param>
        /// <param name="lineAction">The action to take per text file line.</param>
        /// <param name="entriesProjector">The entries projector for LINQ filtering/sorting.</param>
        /// <remarks>
        /// This member is designed for compressed text documents that are too large to load into memory.
        /// The <c>fileAction</c> includes the line number and the current line.
        /// </remarks>
        public static void ReadZipArchiveEntriesByLine(FileInfo archiveInfo, Action<int, string> lineAction, Func<ReadOnlyCollection<ZipArchiveEntry>, IEnumerable<ZipArchiveEntry>> entriesProjector)
        {
            UseZipArchiveEntries(archiveInfo,
                entries => entries.ForEachInEnumerable(i =>
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
                }),
                entriesProjector);
        }

        /// <summary>
        /// Centralizes the use of <see cref="ZipArchive"/>
        /// in <see cref="ZipArchiveMode.Read"/>.
        /// </summary>
        /// <param name="archiveInfo">The ZIP archive <see cref="FileInfo"/>.</param>
        /// <param name="archiveAction">The action to take for the ZIP archive in use.</param>
        public static void UseZipArchive(FileInfo archiveInfo, Action<ZipArchive> archiveAction)
        {
            FrameworkFileUtility.UseZipArchive(archiveInfo, archiveAction, ZipArchiveMode.Read);
        }

        /// <summary>
        /// Centralizes the use of <see cref="ZipArchive"/>
        /// </summary>
        /// <param name="archiveInfo">The ZIP archive <see cref="FileInfo"/>.</param>
        /// <param name="archiveAction">The action to take for the ZIP archive in use.</param>
        /// <param name="zipArchiveMode">The <see cref="ZipArchiveMode"/></param>
        public static void UseZipArchive(FileInfo archiveInfo, Action<ZipArchive> archiveAction, ZipArchiveMode zipArchiveMode)
        {
            if (archiveInfo == null) throw new NullReferenceException("The expected file or directory info is not here.");
            if (!archiveInfo.Exists) throw new FileNotFoundException($"The expected file {archiveInfo.FullName} is not here.");

            using (var stream = new FileStream(archiveInfo.FullName, FileMode.Open))
            using (var zipFile = new ZipArchive(stream, zipArchiveMode, leaveOpen: false))
            {
                archiveAction?.Invoke(zipFile);
            }
        }

        /// <summary>
        /// Centralizes the use of <see cref="ReadOnlyCollection{ZipArchiveEntry}"/>.
        /// </summary>
        /// <param name="archiveInfo">The ZIP archive <see cref="FileInfo"/>.</param>
        /// <param name="entriesAction">The action to take for ZIP archive entries.</param>
        public static void UseZipArchiveEntries(FileInfo archiveInfo, Action<ReadOnlyCollection<ZipArchiveEntry>> entriesAction)
        {
            UseZipArchiveEntries(archiveInfo, entriesAction, entriesProjector: null);
        }

        /// <summary>
        /// Centralizes the use of <see cref="ReadOnlyCollection{ZipArchiveEntry}"/>.
        /// </summary>
        /// <param name="archiveInfo">The ZIP archive <see cref="FileInfo"/>.</param>
        /// <param name="entriesAction">The action to take for ZIP archive entries.</param>
        /// <param name="entriesProjector">The entries projector for LINQ filtering/sorting.</param>
        public static void UseZipArchiveEntries(FileInfo archiveInfo, Action<ReadOnlyCollection<ZipArchiveEntry>> entriesAction, Func<ReadOnlyCollection<ZipArchiveEntry>, IEnumerable<ZipArchiveEntry>> entriesProjector)
        {
            UseZipArchive(archiveInfo, archive =>
            {
                var entries = archive.Entries;

                if ((entries != null) && (entriesProjector != null))
                    entries = entriesProjector.Invoke(entries)?.ToList()?.AsReadOnly();

                entriesAction?.Invoke(entries);
            });
        }

        /// <summary>
        /// Write zip archive entry with <see cref="CompressionLevel.Optimal"/>.
        /// </summary>
        /// <param name="archiveInfo">The ZIP archive <see cref="FileInfo"/>.</param>
        /// <param name="fileInfo">The file information for writing the entry.</param>
        public static void WriteZipArchiveEntry(FileInfo archiveInfo, FileInfo fileInfo)
        {
            WriteZipArchiveEntry(archiveInfo, fileInfo, CompressionLevel.Optimal);
        }

        /// <summary>
        /// Write zip archive entry.
        /// </summary>
        /// <param name="archiveInfo">The ZIP archive <see cref="FileInfo"/>.</param>
        /// <param name="fileInfo">The file information for writing the entry.</param>
        /// <param name="compressionLevel">The <see cref="CompressionLevel"/></param>
        public static void WriteZipArchiveEntry(FileInfo archiveInfo, FileInfo fileInfo, CompressionLevel compressionLevel)
        {
            UseZipArchive(archiveInfo, archive =>
            {
                var entry = archive.CreateEntry(fileInfo.Name, compressionLevel);
                using (var writer = new StreamWriter(entry.Open()))
                {
                    var s = File.ReadAllText(fileInfo.FullName);
                    writer.Write(s);
                    writer.Flush();
                }
            }, ZipArchiveMode.Update);
        }
    }
}
