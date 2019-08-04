using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extension of <see cref="FileInfo"/>
    /// </summary>
    public static class FileInfoExtensions
    {
        /// <summary>
        /// Read zip archive entries.
        /// </summary>
        /// <param name="archiveInfo">The <see cref="FileInfo"/>.</param>
        /// <param name="fileAction">The file action.</param>
        /// <remarks>
        /// Use <c>entriesProjector</c> for any filtering or sorting.
        /// </remarks>
        public static void ReadZipArchiveEntries(this FileInfo archiveInfo, Action<string> fileAction)
        {
            FrameworkFileUtility.ReadZipArchiveEntries(archiveInfo, fileAction);
        }

        /// <summary>
        /// Read zip archive entries.
        /// </summary>
        /// <param name="archiveInfo">The <see cref="FileInfo"/>.</param>
        /// <param name="fileAction">The file action.</param>
        /// <param name="entriesProjector">The entries projector.</param>
        /// <remarks>
        /// Use <c>entriesProjector</c> for any filtering or sorting.
        /// </remarks>
        public static void ReadZipArchiveEntries(this FileInfo archiveInfo, Action<string> fileAction, Func<ReadOnlyCollection<ZipArchiveEntry>, IEnumerable<ZipArchiveEntry>> entriesProjector)
        {
            FrameworkFileUtility.ReadZipArchiveEntries(archiveInfo, fileAction, entriesProjector);
        }

        /// <summary>
        /// Read zip archive entries as strings, line by line.
        /// </summary>
        /// <param name="archiveInfo">The file and/or directory info.</param>
        /// <param name="lineAction">The line action.</param>
        /// <remarks>
        /// This member is designed for compressed text documents that are too large to load into memory.
        /// The <c>fileAction</c> includes the line number and the current line.
        /// </remarks>
        public static void ReadZipArchiveEntriesByLine(this FileInfo archiveInfo, Action<int, string> lineAction)
        {
            FrameworkFileUtility.ReadZipArchiveEntriesByLine(archiveInfo, lineAction);
        }

        /// <summary>
        /// Read zip archive entries as strings, line by line.
        /// </summary>
        /// <param name="archiveInfo">The file and/or directory info.</param>
        /// <param name="lineAction">The line action.</param>
        /// <param name="entriesProjector">The filter zip archive entries.</param>
        /// <remarks>
        /// This member is designed for compressed text documents that are too large to load into memory.
        /// The <c>fileAction</c> includes the line number and the current line.
        /// </remarks>
        public static void ReadZipArchiveEntriesByLine(this FileInfo archiveInfo, Action<int, string> lineAction, Func<ReadOnlyCollection<ZipArchiveEntry>, IEnumerable<ZipArchiveEntry>> entriesProjector)
        {
            FrameworkFileUtility.ReadZipArchiveEntriesByLine(archiveInfo, lineAction, entriesProjector);
        }

        /// <summary>
        /// Centralizes the use of <see cref="ZipArchive"/>
        /// </summary>
        /// <param name="archiveInfo"></param>
        /// <param name="archiveAction"></param>
        public static void UseZipArchive(this FileInfo archiveInfo, Action<ZipArchive> archiveAction)
        {
            FrameworkFileUtility.UseZipArchive(archiveInfo, archiveAction);
        }

        /// <summary>
        /// Centralizes the use of <see cref="ReadOnlyCollection{ZipArchiveEntry}"/>.
        /// </summary>
        /// <param name="archiveInfo"></param>
        /// <param name="entriesAction"></param>
        public static void UseZipArchiveEntries(this FileInfo archiveInfo, Action<ReadOnlyCollection<ZipArchiveEntry>> entriesAction)
        {
            FrameworkFileUtility.UseZipArchiveEntries(archiveInfo, entriesAction);
        }

        /// <summary>
        /// Centralizes the use of <see cref="ReadOnlyCollection{ZipArchiveEntry}"/>.
        /// </summary>
        /// <param name="archiveInfo"></param>
        /// <param name="entriesAction"></param>
        /// <param name="entriesProjector"></param>
        public static void UseZipArchiveEntries(this FileInfo archiveInfo, Action<ReadOnlyCollection<ZipArchiveEntry>> entriesAction, Func<ReadOnlyCollection<ZipArchiveEntry>, ReadOnlyCollection<ZipArchiveEntry>> entriesProjector)
        {
            FrameworkFileUtility.UseZipArchiveEntries(archiveInfo, entriesAction, entriesProjector);
        }

        /// <summary>
        /// Write zip archive entry with <see cref="CompressionLevel.Optimal"/>.
        /// </summary>
        /// <param name="archiveInfo">The file and/or directory info.</param>
        /// <param name="fileInfo">The file information.</param>
        public static void WriteZipArchiveEntry(this FileInfo archiveInfo, FileInfo fileInfo)
        {
            FrameworkFileUtility.WriteZipArchiveEntry(archiveInfo, fileInfo);
        }

        /// <summary>
        /// Write zip archive entry.
        /// </summary>
        /// <param name="archiveInfo">The file and/or directory info.</param>
        /// <param name="fileInfo">The file information.</param>
        /// <param name="compressionLevel">The <see cref="CompressionLevel"/></param>
        public static void WriteZipArchiveEntry(this FileInfo archiveInfo, FileInfo fileInfo, CompressionLevel compressionLevel)
        {
            FrameworkFileUtility.WriteZipArchiveEntry(archiveInfo, fileInfo, compressionLevel);
        }
    }
}
