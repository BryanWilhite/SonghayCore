using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests
{
    public partial class FrameworkFileUtilityTests
    {
        public FrameworkFileUtilityTests(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
        }

        [DebuggerAttachedTheory]
        [InlineData("root1", @"z:\one", "z:|one")]
        [InlineData(@"z:\root1", @"one", "z:|root1|one")]
        [InlineData("root2", @"/home/one", "root2|home|one")]
        [InlineData("path1", @"/two/three/four/", "path1|two|three|four|")]
        [InlineData("path2", @"\two\three\four", "path2|two|three|four")]
        public void GetCombinedPath_Test(string root, string path, string expectedResult)
        {
            var actual = FrameworkFileUtility.GetCombinedPath(root, path);
            Assert.Equal(expectedResult.Replace('|', Path.DirectorySeparatorChar), actual);
        }

        [Theory]
        [InlineData("./one/", "one|")]
        [InlineData("../../one/", "one|")]
        [InlineData(@".\one/two\", @"one|two|")]
        [InlineData(@"..\..\one\", @"one|")]
        public void GetRelativePath_Test(string input, string expectedResult)
        {
            var actual = FrameworkFileUtility.GetRelativePath(input);
            Assert.Equal(expectedResult.Replace('|', Path.DirectorySeparatorChar), actual);
        }

        [Theory, InlineData(@"/\foo\bar\my-file.json", @"foo\bar\my-file.json")]
        public void TrimLeadingDirectorySeparatorChars_Test(string path, string expectedResult)
        {
            var actual = FrameworkFileUtility.TrimLeadingDirectorySeparatorChars(path);
            Assert.Equal(expectedResult, actual);
        }

        readonly ITestOutputHelper _testOutputHelper;
    }
}
