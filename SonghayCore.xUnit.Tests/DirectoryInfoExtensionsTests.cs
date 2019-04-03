using Songhay.Extensions;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests
{
    public class DirectoryInfoExtensionsTests
    {
        public DirectoryInfoExtensionsTests(ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
        }

        [SkippableTheory]
        [InlineData(0)]
        [InlineData(3)]
        public void GetParentDirectory_Test(int expectedNumberOfLevels)
        {
            var path = this.GetType().Assembly.GetPathFromAssembly();
            this._testOutputHelper.WriteLine($"path from assembly: {path}");

            var directoryInfo = new DirectoryInfo(path);
            var nextDirectoryInfo = directoryInfo.GetParentDirectoryInfo(expectedNumberOfLevels);
            this._testOutputHelper.WriteLine($"path {expectedNumberOfLevels} levels above assembly: {nextDirectoryInfo.FullName}");

            switch (expectedNumberOfLevels)
            {
                case 0:
                    Assert.Equal(directoryInfo, nextDirectoryInfo);
                    break;
                case 3:
                    Assert.EndsWith(this.GetType().Namespace, nextDirectoryInfo.FullName.Replace("Core.xUnit", string.Empty));
                    break;
                default:
                    Skip.If(true, "unexpected inline data");
                    break;
            }
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
}
