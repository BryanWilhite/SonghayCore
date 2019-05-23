using System.IO;
using Xunit;

namespace Songhay.Tests
{
    public class FrameworkAssemblyUtilityTest
    {
        [Theory, InlineData(@"..\..\..\content\FrameworkAssemblyUtilityTest-ShouldGetPathFromAssembly.json")]
        public void ShouldGetPathFromAssembly(string fileSegment)
        {
            var actualPath = FrameworkAssemblyUtility.GetPathFromAssembly(this.GetType().Assembly, fileSegment);
            Assert.True(File.Exists(actualPath));
        }
    }
}
