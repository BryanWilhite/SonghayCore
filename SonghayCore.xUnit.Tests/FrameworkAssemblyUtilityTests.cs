using System.IO;
using Xunit;

namespace Songhay.Tests
{
    public class FrameworkAssemblyUtilityTests
    {
        [Theory, InlineData(@"..\..\..\content\FrameworkAssemblyUtilityTest-ShouldGetPathFromAssembly.json")]
        public void GetPathFromAssembly_Test(string fileSegment)
        {
            var actualPath = FrameworkAssemblyUtility.GetPathFromAssembly(this.GetType().Assembly, fileSegment);
            Assert.True(File.Exists(actualPath));
        }
    }
}
