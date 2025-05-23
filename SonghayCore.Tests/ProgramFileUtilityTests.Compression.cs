﻿using System.Text;

namespace Songhay.Tests;

public partial class ProgramFileUtilityTests
{
    [Theory, ProjectFileData(typeof(ProgramFileUtilityTests), "../../../content/zip/ReadZipArchiveEntries.zip")]
    public void ReadZipArchiveEntries_Test(FileInfo archiveInfo)
    {
        // arrange
        var isReading = false;

        // act
        ProgramFileUtility.ReadZipArchiveEntries(archiveInfo, entry =>
        {
            helper.WriteLine(entry);
            isReading = true;
        });

        // assert
        Assert.True(isReading);
    }

    [Theory, ProjectFileData(typeof(ProgramFileUtilityTests), "../../../content/zip/ReadZipArchiveEntries.zip")]
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
            helper.WriteLine(entry);
            builder.Append(entry);
        }, entries => entries.OrderByDescending(i => i.Name));

        // assert
        Assert.True(isReading);
        Assert.Equal(expectedOrder, builder.ToString());
    }

    [Theory, ProjectFileData(typeof(ProgramFileUtilityTests), "../../../content/zip/ReadZipArchiveEntriesByLine.zip")]
    public void ReadZipArchiveEntriesByLine_Test(FileInfo archiveInfo)
    {
        // arrange
        var isReading = false;

        // act
        ProgramFileUtility.ReadZipArchiveEntriesByLine(archiveInfo, (lineNum, line) =>
        {
            helper.WriteLine($"{lineNum}: {line}");
            isReading = true;
        }, entries => entries.Where(i => i.Name.EqualsInvariant("c.txt")));

        // assert
        Assert.True(isReading);
    }

    [Theory, ProjectFileData(typeof(ProgramFileUtilityTests), "../../../content/zip/UseZipArchive.zip")]
    public void UseZipArchive_Test(FileInfo archiveInfo)
    {
        // arrange
        var isUsing = false;

        // act
        ProgramFileUtility.UseZipArchive(archiveInfo, archive => { isUsing = archive != null; });

        // assert
        Assert.True(isUsing);
    }

    [Trait(TestScalars.XunitCategory, TestScalars.XunitCategoryIntegrationManualTest)]
    [SkippableTheory,
     ProjectFileData(typeof(ProgramFileUtilityTests),
         "../../../json/hello.json",
         "../../../content/zip/WriteZipArchiveEntry.zip")]
    public void WriteZipArchiveEntry_Test(FileInfo fileInfo, FileInfo archiveInfo)
    {
        Skip.If(TestScalars.IsNotDebugging, TestScalars.ReasonForSkippingWhenNotDebugging);

        ProgramFileUtility.WriteZipArchiveEntry(archiveInfo, fileInfo);
    }
}
