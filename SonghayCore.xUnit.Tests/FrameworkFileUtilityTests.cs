using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests
{
    public class FrameworkFileUtilityTests
    {
        public FrameworkFileUtilityTests(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
        }

        [Theory]
        [InlineData("root1", @"z:\one")]
        [InlineData("root2", @"/home/one")]
        [InlineData("path1", @"/two/three/four/")]
        [InlineData("path2", @"\two\three\four")]
        public void GetCombinedPath_Test(string root, string path)
        {
            path = FrameworkFileUtility.GetCombinedPath(root, path);
            this._testOutputHelper.WriteLine(path);
        }

        [Theory, InlineData(@"foo\bar\my-file.json", @"/\foo\bar\my-file.json")]
        public void TrimLeadingDirectorySeparatorChars_Test(string expectedPath, string path)
        {
            var actualPath = FrameworkFileUtility.TrimLeadingDirectorySeparatorChars(path);
            Assert.Equal(expectedPath, actualPath);
        }

        readonly ITestOutputHelper _testOutputHelper;
    }
}
