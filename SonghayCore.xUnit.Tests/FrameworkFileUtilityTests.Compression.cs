using Songhay.Extensions;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Songhay.Tests
{
    public partial class FrameworkFileUtilityTests
    {
        [Theory, ProjectFileData(typeof(FrameworkFileUtilityTests), "../../../zip/ReadZipArchiveEntries.zip")]
        public void ReadZipArchiveEntries_Test(FileInfo archiveInfo)
        {
            // arrange
            var isReading = false;

            // act
            FrameworkFileUtility.ReadZipArchiveEntries(archiveInfo, entry =>
            {
                this._testOutputHelper.WriteLine(entry);
                isReading = true;
            });

            // assert
            Assert.True(isReading);
        }

        [Theory, ProjectFileData(typeof(FrameworkFileUtilityTests), "../../../zip/ReadZipArchiveEntries.zip")]
        public void ReadZipArchiveEntriesBackwards_Test(FileInfo archiveInfo)
        {
            // arrange
            var isReading = false;
            var expectedOrder = "dcba";
            var builder = new StringBuilder();

            // act
            FrameworkFileUtility.ReadZipArchiveEntries(archiveInfo, entry =>
            {
                isReading = true;
                this._testOutputHelper.WriteLine(entry);
                builder.Append(entry);
            }, entries => entries.OrderByDescending(i => i.Name));

            // assert
            Assert.True(isReading);
            Assert.Equal(expectedOrder, builder.ToString());
        }

        [Theory, ProjectFileData(typeof(FrameworkFileUtilityTests), "../../../zip/ReadZipArchiveEntriesByLine.zip")]
        public void ReadZipArchiveEntriesByLine_Test(FileInfo archiveInfo)
        {
            // arrange
            var isReading = false;

            // act
            FrameworkFileUtility.ReadZipArchiveEntriesByLine(archiveInfo, (lineNum, line) =>
            {
                this._testOutputHelper.WriteLine($"{lineNum}: {line}");
                isReading = true;
            }, entries => entries.Where(i => i.Name.EqualsInvariant("c.txt")));

            // assert
            Assert.True(isReading);
        }

        [Theory, ProjectFileData(typeof(FrameworkFileUtilityTests), "../../../zip/UseZipArchive.zip")]
        public void UseZipArchive_Test(FileInfo archiveInfo)
        {
            // arrange
            var isUsing = false;

            // act
            FrameworkFileUtility.UseZipArchive(archiveInfo, archive => { isUsing = (archive != null); });

            // assert
            Assert.True(isUsing);
        }

        [DebuggerAttachedTheory,
            ProjectFileData(typeof(FrameworkFileUtilityTests),
            "../../../json/hello.json",
            "../../../zip/WriteZipArchiveEntry.zip")]
        public void WriteZipArchiveEntry_Test(FileInfo fileInfo, FileInfo archiveInfo)
        {
            FrameworkFileUtility.WriteZipArchiveEntry(archiveInfo, fileInfo);
        }
    }
}
