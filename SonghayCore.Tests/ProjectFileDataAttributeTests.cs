using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests
{
    public class ProjectFileDataAttributeTests
    {
        public ProjectFileDataAttributeTests(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
        }

        [Theory]
        [ProjectFileData(typeof(ProjectFileDataAttributeTests), "../../../../LICENSE.md")]
        public void ShouldLoadLicenseFile(FileInfo fileInfo)
        {
            this._testOutputHelper.WriteLine($"{fileInfo.FullName}");
        }

        ITestOutputHelper _testOutputHelper;
    }
}