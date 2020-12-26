using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests
{
    public class ProgramAssemblyUtilityTests
    {
        public ProgramAssemblyUtilityTests(ITestOutputHelper helper)
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
            var actualPath = ProgramAssemblyUtility.GetPathFromAssembly(this.GetType().Assembly, fileSegment);
            this._testOutputHelper.WriteLine(actualPath);
            Assert.True(File.Exists(actualPath));
        }

        [Fact]
        public void ProposedLocationTest()
        {
            var assembly = this.GetType().Assembly;

            var proposedLocation = (!string.IsNullOrWhiteSpace(assembly.CodeBase) &&
                    !ProgramFileUtility.IsForwardSlashSystem()) ?
                assembly.CodeBase.Replace("file:///", string.Empty) :
                assembly.Location;

            this._testOutputHelper.WriteLine($"{nameof(assembly.Location)}: {assembly.Location}");
            this._testOutputHelper.WriteLine($"{nameof(assembly.CodeBase)}: {assembly.CodeBase}");
            this._testOutputHelper.WriteLine($"{nameof(proposedLocation)}: {proposedLocation}");
        }

        readonly ITestOutputHelper _testOutputHelper;
    }
}