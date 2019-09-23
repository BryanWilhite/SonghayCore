using Songhay.Extensions;
using System.IO;
using Xunit;

namespace Songhay.Tests
{
    public class AssemblyExtensionsTests
    {
        [Fact]
        public void GetPathFromAssembly_Test()
        {
            var assembly = this.GetType().Assembly;

            var path = assembly.GetPathFromAssembly();

            Assert.True(Directory.Exists(path));
        }
    }
}
