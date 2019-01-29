using Songhay.Extensions;
using System.IO;
using Xunit;

namespace Songhay.Tests
{
    public class DirectoryInfoExtensionsTests
    {
        [Fact]
        public void GetParentDirectory_Test()
        {
            var path = this.GetType().Assembly.GetPathFromAssembly();
            var directoryInfo = new DirectoryInfo(path);

            directoryInfo = directoryInfo.GetParentDirectoryInfo(3);

            Assert.EndsWith(this.GetType().Namespace, directoryInfo.FullName);
        }

        [Fact]
        public void GetParentDirectoryInfo_Level0_Test()
        {
            var path = this.GetType().Assembly.GetPathFromAssembly();
            var expectedDirectoryInfo = new DirectoryInfo(path);

            var actualDirectoryInfo = expectedDirectoryInfo.GetParentDirectoryInfo(0);

            Assert.Equal(expectedDirectoryInfo, actualDirectoryInfo);
        }

        [Fact]
        public void ToCombinedPath_Test()
        {
            var projectPath = @"json/hello.json";
            var path = this.GetType().Assembly.GetPathFromAssembly();
            var directoryInfo = new DirectoryInfo(path);

            directoryInfo = directoryInfo.GetParentDirectoryInfo(3);

            path = directoryInfo.ToCombinedPath(projectPath);
            Assert.True(File.Exists(path));
        }
    }
}
