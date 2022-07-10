using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests;

public class ProjectFileDataAttributeTests
{
    public ProjectFileDataAttributeTests(ITestOutputHelper helper)
    {
        _testOutputHelper = helper;
    }

    [Theory]
    [ProjectFileData(typeof(ProjectFileDataAttributeTests), "../../../../LICENSE.md")]
    public void ShouldLoadLicenseFile(FileInfo fileInfo)
    {
        _testOutputHelper.WriteLine($"{fileInfo.FullName}");
    }

    readonly ITestOutputHelper _testOutputHelper;
}