namespace Songhay.Tests.Extensions;

public class DirectoryInfoExtensionsTests(ITestOutputHelper testOutputHelper)
{
    [Theory]
    [InlineData(0, null)]
    [InlineData(3, "SonghayCore.Tests")]
    public void GetParentDirectory_Test(int expectedNumberOfLevels, string? endsWith)
    {
        var path = GetType().Assembly.GetPathFromAssembly().ToReferenceTypeValueOrThrow();
        testOutputHelper.WriteLine($"path from assembly: {path}");

        var directoryInfo = new DirectoryInfo(path);
        var nextDirectoryInfo = directoryInfo.GetParentDirectoryInfo(expectedNumberOfLevels).ToReferenceTypeValueOrThrow();
        testOutputHelper.WriteLine($"path {expectedNumberOfLevels} levels above assembly: {nextDirectoryInfo.FullName}");

        Assert.EndsWith(endsWith ?? directoryInfo.Name, nextDirectoryInfo.FullName);
    }

    [Theory]
    [InlineData("../../../content/json/hello.json")]
    [InlineData(@"..\..\../content\json/hello.json")]
    public void ToCombinedPath_Test(string expectedPath)
    {
        var path = GetType().Assembly.GetPathFromAssembly().ToReferenceTypeValueOrThrow();
        testOutputHelper.WriteLine($"path from assembly: {path}");

        var levels = expectedPath.ToNumberOfDirectoryLevels();
        testOutputHelper.WriteLine($"directory levels: {levels}");

        var directoryInfo = new DirectoryInfo(path);
        directoryInfo = directoryInfo.GetParentDirectoryInfo(levels).ToReferenceTypeValueOrThrow();
        testOutputHelper.WriteLine($"path {levels} levels above assembly: {directoryInfo.FullName}");

        path = directoryInfo.ToCombinedPath(expectedPath);
        Assert.True(File.Exists(path));
    }
}
