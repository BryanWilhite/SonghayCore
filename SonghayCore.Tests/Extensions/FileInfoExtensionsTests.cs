using System.Text;

namespace Songhay.Tests.Extensions;

public class FileInfoExtensionsTests(ITestOutputHelper helper)
{
    [Theory, ProjectFileData(typeof(FileInfoExtensionsTests), "../../../content/zip/ReadZipArchiveEntries.zip")]
    public void ReadZipArchiveEntries_Test(FileInfo archiveInfo)
    {
        // arrange
        var isReading = false;

        // act
        archiveInfo.ReadZipArchiveEntries(entry =>
        {
            helper.WriteLine(entry);
            isReading = true;
        });

        // assert
        Assert.True(isReading);
    }

    [Theory, ProjectFileData(typeof(FileInfoExtensionsTests), "../../../content/zip/ReadZipArchiveEntries.zip")]
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
            helper.WriteLine(entry);
            builder.Append(entry);
        }, entries => entries.OrderByDescending(i => i.Name));

        // assert
        Assert.True(isReading);
        Assert.Equal(expectedOrder, builder.ToString());
    }

    [Theory, ProjectFileData(typeof(FileInfoExtensionsTests), "../../../content/zip/ReadZipArchiveEntriesByLine.zip")]
    public void ReadZipArchiveEntriesByLine_Test(FileInfo archiveInfo)
    {
        // arrange
        var isReading = false;

        // act
        archiveInfo.ReadZipArchiveEntriesByLine((lineNum, line) =>
        {
            helper.WriteLine($"{lineNum}: {line}");
            isReading = true;
        }, entries => entries.Where(i => i.Name.EqualsInvariant("c.txt")));

        // assert
        Assert.True(isReading);
    }

    [Theory, ProjectFileData(typeof(FileInfoExtensionsTests), "../../../content/zip/UseZipArchive.zip")]
    public void UseZipArchive_Test(FileInfo archiveInfo)
    {
        // arrange
        var isUsing = false;

        // act
        archiveInfo.UseZipArchive(archive => { isUsing = archive != null; });

        // assert
        Assert.True(isUsing);
    }

    [Trait(TestScalars.XunitCategory, TestScalars.XunitCategoryIntegrationManualTest)]
    [SkippableTheory,
     ProjectFileData(typeof(FileInfoExtensionsTests),
         "../../../json/hello.json",
         "../../../content/zip/WriteZipArchiveEntry.zip")]
    public void WriteZipArchiveEntry_Test(FileInfo fileInfo, FileInfo archiveInfo)
    {
        Skip.If(TestScalars.IsNotDebugging, TestScalars.ReasonForSkippingWhenNotDebugging);

        archiveInfo.WriteZipArchiveEntry(fileInfo);
    }
}