﻿using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests
{
    public partial class ProgramFileUtilityTests
    {
        public ProgramFileUtilityTests(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
        }

        [SkippableTheory]
        [InlineData("root1", @"z:\one", "z:|one", true)]
        [InlineData(@"z:\root1", @"one", "z:|root1|one", true)]
        [InlineData("root2", @"/home/one", "root2|home|one", false)]
        [InlineData("path1", @"/two/three/four/", "path1|two|three|four|", false)]
        [InlineData("path2", @"\two\three\four", "path2|two|three|four", false)]
        public void GetCombinedPath_Test(string root, string path, string expectedResult, bool requiresWindows)
        {
            Skip.If(requiresWindows && ProgramFileUtility.IsForwardSlashSystem(), "OS is not Windows");

            var actual = ProgramFileUtility.GetCombinedPath(root, path);
            Assert.Equal(expectedResult.Replace('|', Path.DirectorySeparatorChar), actual);
        }

        [Theory]
        [InlineData("./one/", "one|")]
        [InlineData("../../one/", "one|")]
        [InlineData(@".\one/two\", @"one|two|")]
        [InlineData(@"..\..\one\", @"one|")]
        public void GetRelativePath_Test(string input, string expectedResult)
        {
            var actual = ProgramFileUtility.GetRelativePath(input);
            Assert.Equal(expectedResult.Replace('|', Path.DirectorySeparatorChar), actual);
        }

        [Theory, InlineData(@"/\foo\bar\my-file.json", @"foo\bar\my-file.json")]
        public void TrimLeadingDirectorySeparatorChars_Test(string path, string expectedResult)
        {
            var actual = ProgramFileUtility.TrimLeadingDirectorySeparatorChars(path);
            Assert.Equal(expectedResult, actual);
        }

        readonly ITestOutputHelper _testOutputHelper;
    }
}