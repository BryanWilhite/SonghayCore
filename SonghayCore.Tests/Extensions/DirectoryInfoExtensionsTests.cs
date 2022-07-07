using Songhay.Extensions;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests.Extensions;

public class DirectoryInfoExtensionsTests
{
    public DirectoryInfoExtensionsTests(ITestOutputHelper testOutputHelper)
    {
        this._testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData(0, null)]
    [InlineData(3, "SonghayCore.Tests")]
    public void GetParentDirectory_Test(int expectedNumberOfLevels, string endsWith)
    {
        var path = this.GetType().Assembly.GetPathFromAssembly();
        this._testOutputHelper.WriteLine($"path from assembly: {path}");

        var directoryInfo = new DirectoryInfo(path);
        var nextDirectoryInfo = directoryInfo.GetParentDirectoryInfo(expectedNumberOfLevels);
        this._testOutputHelper.WriteLine($"path {expectedNumberOfLevels} levels above assembly: {nextDirectoryInfo.FullName}");

        Assert.EndsWith(endsWith ?? directoryInfo.Name, nextDirectoryInfo.FullName);
    }

    [Theory]
    [InlineData("../../../json/hello.json")]
    [InlineData(@"..\..\..\json/hello.json")]
    public void ToCombinedPath_Test(string expectedPath)
    {
        var path = this.GetType().Assembly.GetPathFromAssembly();
        this._testOutputHelper.WriteLine($"path from assembly: {path}");

        var levels = expectedPath.ToNumberOfDirectoryLevels();
        this._testOutputHelper.WriteLine($"directory levels: {levels}");

        var directoryInfo = new DirectoryInfo(path);
        directoryInfo = directoryInfo.GetParentDirectoryInfo(levels);
        this._testOutputHelper.WriteLine($"path {levels} levels above assembly: {directoryInfo.FullName}");

        path = directoryInfo.ToCombinedPath(expectedPath);
        Assert.True(File.Exists(path));
    }

    readonly ITestOutputHelper _testOutputHelper;
}