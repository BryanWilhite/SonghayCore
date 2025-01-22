using System.Text;
using Songhay.Tests.Extensions;

namespace Songhay.Tests;

public partial class ProgramFileUtilityTests(ITestOutputHelper helper)
{
    [Trait(TestScalars.XunitCategory, TestScalars.XunitCategoryIntegrationManualTest)]
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
    [InlineData(true)]
    [InlineData(false)]
    public void GetCombinedPath_fileIsExpected_Test(bool fileIsExpected)
    {
        DirectoryInfo projectDirectoryInfo = GetType().Assembly.GetNetCoreProjectDirectoryInfo();

        if (fileIsExpected)
        {
            Assert.Throws<FileNotFoundException>(() =>
                ProgramFileUtility
                    .GetCombinedPath(
                        projectDirectoryInfo.FullName,
                        projectDirectoryInfo.GetDirectories().First().Name,
                        fileIsExpected));

            ProgramFileUtility
                .GetCombinedPath(
                    projectDirectoryInfo.FullName,
                    projectDirectoryInfo.GetFiles().First().Name,
                    fileIsExpected);
        }
        else
        {
            Assert.Throws<DirectoryNotFoundException>(() =>
                ProgramFileUtility
                    .GetCombinedPath(
                        projectDirectoryInfo.FullName,
                        projectDirectoryInfo.GetFiles().First().Name,
                        fileIsExpected));

            ProgramFileUtility
                .GetCombinedPath(
                    projectDirectoryInfo.FullName,
                    projectDirectoryInfo.GetDirectories().First().Name,
                    fileIsExpected);
        }
    }

    [Fact]
    public void GetEncodedString_Test()
    {
        const string unicode = "This string contains the unicode character Pi (\u03a0)";

        string actual = ProgramFileUtility.GetEncodedString(unicode, Encoding.ASCII);

        Assert.NotNull(actual);
        Assert.True(actual.Contains('?'));
    }

    [Fact]
    public void GetParentDirectory_Test()
    {
        string expected = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, TestScalars.NetCoreRelativePathToProjectFolder);
        string? actual = ProgramFileUtility.GetParentDirectory(GetType().Assembly.Location, 4);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetParentDirectoryInfo_Test()
    {
        string expected = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, TestScalars.NetCoreRelativePathToProjectFolder);
        DirectoryInfo? actual = ProgramFileUtility.GetParentDirectoryInfo(GetType().Assembly.Location, 4);

        Assert.Equal(expected, actual.ToReferenceTypeValueOrThrow().FullName);
    }

    [Theory]
    [InlineData("./one/", "one|")]
    [InlineData("../../one/", "one|")]
    [InlineData(@".\one/two\", @"one|two|")]
    [InlineData(@"..\..\one\", @"one|")]
    public void GetRelativePath_Test(string input, string expectedResult)
    {
        string? actual = ProgramFileUtility.GetRelativePath(input);
        Assert.Equal(expectedResult.Replace('|', Path.DirectorySeparatorChar), actual);
    }

    [Theory, InlineData(@"/\foo\bar\my-file.json", @"foo\bar\my-file.json")]
    public void TrimLeadingDirectorySeparatorChars_Test(string path, string expectedResult)
    {
        string? actual = ProgramFileUtility.TrimLeadingDirectorySeparatorChars(path);
        Assert.Equal(expectedResult, actual);
    }
}