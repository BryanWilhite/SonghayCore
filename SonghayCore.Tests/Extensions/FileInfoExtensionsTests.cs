using Songhay.Extensions;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests;

public class FileInfoExtensionsTests
{
    public FileInfoExtensionsTests(ITestOutputHelper helper)
    {
        this._testOutputHelper = helper;
    }

    [Theory, ProjectFileData(typeof(FileInfoExtensionsTests), "../../../zip/ReadZipArchiveEntries.zip")]
    public void ReadZipArchiveEntries_Test(FileInfo archiveInfo)
    {
        // arrange
        var isReading = false;

        // act
        archiveInfo.ReadZipArchiveEntries(entry =>
        {
            this._testOutputHelper.WriteLine(entry);
            isReading = true;
        });

        // assert
        Assert.True(isReading);
    }

    [Theory, ProjectFileData(typeof(FileInfoExtensionsTests), "../../../zip/ReadZipArchiveEntries.zip")]
    public void ReadZipArchiveEntriesBackwards_Test(FileInfo archiveInfo)
    {
        // arrange
        var isReading = false;
        var expectedOrder = "dcba";
        var builder = new StringBuilder();

        // act
        archiveInfo.ReadZipArchiveEntries(entry =>
        {
            isReading = true;
            this._testOutputHelper.WriteLine(entry);
            builder.Append(entry);
        }, entries => entries.OrderByDescending(i => i.Name));

        // assert
        Assert.True(isReading);
        Assert.Equal(expectedOrder, builder.ToString());
    }

    [Theory, ProjectFileData(typeof(FileInfoExtensionsTests), "../../../zip/ReadZipArchiveEntriesByLine.zip")]
    public void ReadZipArchiveEntriesByLine_Test(FileInfo archiveInfo)
    {
        // arrange
        var isReading = false;

        // act
        archiveInfo.ReadZipArchiveEntriesByLine((lineNum, line) =>
        {
            this._testOutputHelper.WriteLine($"{lineNum}: {line}");
            isReading = true;
        }, entries => entries.Where(i => i.Name.EqualsInvariant("c.txt")));

        // assert
        Assert.True(isReading);
    }

    [Theory, ProjectFileData(typeof(FileInfoExtensionsTests), "../../../zip/UseZipArchive.zip")]
    public void UseZipArchive_Test(FileInfo archiveInfo)
    {
        // arrange
        var isUsing = false;

        // act
        archiveInfo.UseZipArchive(archive => { isUsing = (archive != null); });

        // assert
        Assert.True(isUsing);
    }

    [DebuggerAttachedTheory,
     ProjectFileData(typeof(FileInfoExtensionsTests),
         "../../../json/hello.json",
         "../../../zip/WriteZipArchiveEntry.zip")]
    public void WriteZipArchiveEntry_Test(FileInfo fileInfo, FileInfo archiveInfo)
    {
        archiveInfo.WriteZipArchiveEntry(fileInfo);
    }

    private readonly ITestOutputHelper _testOutputHelper;
}