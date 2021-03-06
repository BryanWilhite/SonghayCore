﻿using Songhay.Extensions;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Songhay.Tests
{
    public partial class ProgramFileUtilityTests
    {
        [Theory, ProjectFileData(typeof(ProgramFileUtilityTests), "../../../zip/ReadZipArchiveEntries.zip")]
        public void ReadZipArchiveEntries_Test(FileInfo archiveInfo)
        {
            // arrange
            var isReading = false;

            // act
            ProgramFileUtility.ReadZipArchiveEntries(archiveInfo, entry =>
            {
                this._testOutputHelper.WriteLine(entry);
                isReading = true;
            });

            // assert
            Assert.True(isReading);
        }

        [Theory, ProjectFileData(typeof(ProgramFileUtilityTests), "../../../zip/ReadZipArchiveEntries.zip")]
        public void ReadZipArchiveEntriesBackwards_Test(FileInfo archiveInfo)
        {
            // arrange
            var isReading = false;
            var expectedOrder = "dcba";
            var builder = new StringBuilder();

            // act
            ProgramFileUtility.ReadZipArchiveEntries(archiveInfo, entry =>
            {
                isReading = true;
                this._testOutputHelper.WriteLine(entry);
                builder.Append(entry);
            }, entries => entries.OrderByDescending(i => i.Name));

            // assert
            Assert.True(isReading);
            Assert.Equal(expectedOrder, builder.ToString());
        }

        [Theory, ProjectFileData(typeof(ProgramFileUtilityTests), "../../../zip/ReadZipArchiveEntriesByLine.zip")]
        public void ReadZipArchiveEntriesByLine_Test(FileInfo archiveInfo)
        {
            // arrange
            var isReading = false;

            // act
            ProgramFileUtility.ReadZipArchiveEntriesByLine(archiveInfo, (lineNum, line) =>
            {
                this._testOutputHelper.WriteLine($"{lineNum}: {line}");
                isReading = true;
            }, entries => entries.Where(i => i.Name.EqualsInvariant("c.txt")));

            // assert
            Assert.True(isReading);
        }

        [Theory, ProjectFileData(typeof(ProgramFileUtilityTests), "../../../zip/UseZipArchive.zip")]
        public void UseZipArchive_Test(FileInfo archiveInfo)
        {
            // arrange
            var isUsing = false;

            // act
            ProgramFileUtility.UseZipArchive(archiveInfo, archive => { isUsing = (archive != null); });

            // assert
            Assert.True(isUsing);
        }

        [DebuggerAttachedTheory,
            ProjectFileData(typeof(ProgramFileUtilityTests),
            "../../../json/hello.json",
            "../../../zip/WriteZipArchiveEntry.zip")]
        public void WriteZipArchiveEntry_Test(FileInfo fileInfo, FileInfo archiveInfo)
        {
            ProgramFileUtility.WriteZipArchiveEntry(archiveInfo, fileInfo);
        }
    }
}
