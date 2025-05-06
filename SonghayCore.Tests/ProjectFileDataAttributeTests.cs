namespace Songhay.Tests;

public class ProjectFileDataAttributeTests(ITestOutputHelper helper)
{
    [Theory]
    [ProjectFileData("../../../content/json/hello.json", "../../../content/txt/latin-glyphs.txt")]
    public void ShouldLoadContentFilesWithoutSpecifyingType(FileInfo jsonFile, FileInfo txtFile)
    {
        helper.WriteLine(jsonFile.FullName);

        Assert.True(jsonFile.Exists, "The expected JSON file is not here.");

        helper.WriteLine(txtFile.FullName);

        Assert.True(txtFile.Exists, "The expected text file is not here.");
    }

    [Theory]
    [ProjectFileData(typeof(ProjectFileDataAttributeTests), "../../../../LICENSE.md")]
    public void ShouldLoadLicenseFile(FileInfo fileInfo)
    {
        helper.WriteLine($"{fileInfo.FullName}");

        Assert.True(fileInfo.Exists, "The expected file is not here.");
    }

    [Theory]
    [ProjectFileData("../../../../LICENSE.md")]
    public void ShouldLoadLicenseFileWithoutSpecifyingType(FileInfo fileInfo)
    {
        helper.WriteLine($"{fileInfo.FullName}");

        Assert.True(fileInfo.Exists, "The expected file is not here.");
    }

    [Theory]
    [ProjectFileData(["inline data", true], "../../../../LICENSE.md")]
    public void ShouldLoadLicenseFileWithoutSpecifyingTypeWhileSpecifyingInlineData(string caption, bool isInline, FileInfo fileInfo)
    {
        helper.WriteLine($"{caption}: {isInline}");
        helper.WriteLine($"{fileInfo.FullName}");

        Assert.True(fileInfo.Exists, "The expected file is not here.");
    }
}
