using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests
{
    public class FrameworkAssemblyUtilityTests
    {
        public FrameworkAssemblyUtilityTests(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
        }

        [Theory]
        [InlineData(@"..\..\..\content\FrameworkAssemblyUtilityTest-ShouldGetPathFromAssembly.json")]
        [InlineData("../../../content/FrameworkAssemblyUtilityTest-ShouldGetPathFromAssembly.json")]
        [InlineData(@"..\..\..\..\SonghayCore\SonghayCore.nuspec")]
        [InlineData("../../../../SonghayCore/SonghayCore.nuspec")]
        public void GetPathFromAssembly_Test(string fileSegment)
        {
            var actualPath = FrameworkAssemblyUtility.GetPathFromAssembly(this.GetType().Assembly, fileSegment);
            this._testOutputHelper.WriteLine(actualPath);
            Assert.True(File.Exists(actualPath));
        }

        readonly ITestOutputHelper _testOutputHelper;
    }
}
