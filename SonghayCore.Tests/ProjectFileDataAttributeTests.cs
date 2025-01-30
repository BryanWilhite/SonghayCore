namespace Songhay.Tests;

public class ProjectFileDataAttributeTests(ITestOutputHelper helper)
{
    [Theory]
    [ProjectFileData(typeof(ProjectFileDataAttributeTests), "../../../../LICENSE.md")]
    public void ShouldLoadLicenseFile(FileInfo fileInfo)
    {
        helper.WriteLine($"{fileInfo.FullName}");
    }
}