namespace Songhay.Tests;

public class ProjectDirectoryDataAttributeTests(ITestOutputHelper helper)
{
    [Theory]
    [ProjectDirectoryData(
        "./content/json/hello.json", "./content/txt/latin-glyphs.txt")]
    public void ShouldLoadContentFiles(DirectoryInfo projectDirectoryInfo, string jsonPath, string txtPath)
    {
        jsonPath = projectDirectoryInfo.ToCombinedPath(jsonPath);

        helper.WriteLine(jsonPath);

        Assert.True(File.Exists(jsonPath));

        txtPath = projectDirectoryInfo.ToCombinedPath(txtPath);

        helper.WriteLine(txtPath);

        Assert.True(File.Exists(txtPath));
    }

    [Theory]
    [ProjectDirectoryData("./content/json/",
        ProjectDirectoryOption.AppendPathSuffixOnly,
        "hello.json")]
    public void ShouldLoadContentFileWithSuffix(DirectoryInfo rootDirectoryInfo, string jsonPath)
    {
        jsonPath = rootDirectoryInfo.ToCombinedPath(jsonPath);

        helper.WriteLine(jsonPath);

        Assert.True(File.Exists(jsonPath));
    }
}
