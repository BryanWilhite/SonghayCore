using System.IO;
using System.Threading.Tasks;
using Songhay;
using Songhay.Extensions;
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
            var projectRoot = ProgramAssemblyUtility.GetPathFromAssembly(this.GetType().Assembly, "../../../");
            var projectInfo = new DirectoryInfo(projectRoot);

            string[] args = new [] {
                "--log-file",
                ProgramFileUtility.GetCombinedPath(projectRoot,"txt/statiq"),
                "--input",
                $"{projectInfo.Parent.FullName}/**/**/*.cs",
                "--output",
                projectInfo.Parent.ToCombinedPath("docs2022/"),
                "--root",
                $"{projectRoot}/",
                "--normal"
            };

            var actual = await Bootstrapper
                .Factory
                .CreateDocs(args)
                .RunAsync();

            Assert.Equal((int)ExitCode.Normal, actual);
        }
    }
}
