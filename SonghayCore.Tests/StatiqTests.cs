using System;
using System.Threading.Tasks;
using Statiq.App;
using Statiq.Docs;
using Xunit;

namespace SonghayCore.Tests
{
    public class StatiqTests
    {
        [Fact]
        public async Task CreateDocs_Test()
        {
            string[] args = Array.Empty<string>();

            var actual = await Bootstrapper
                .Factory
                .CreateDocs(args)
                .RunAsync();

            Assert.Equal((int)ExitCode.Normal, actual);
        }
    }
}
