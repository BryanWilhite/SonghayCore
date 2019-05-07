#if NETSTANDARD

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Songhay.Tests.Extensions
{
    [TestClass]
    public class HttpClientExtensionsTest
    {
        public TestContext TestContext { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        [TestProperty("file", @"Extensions\HttpClientExtensionsTest.ShouldDownloadToFileAsync.txt")]
        [TestProperty("uri", "https://github.com/BryanWilhite/SonghayCore/blob/master/README.md")]
        public async Task ShouldDownloadToFileAsync()
        {
            var projectDirectoryInfo = this.TestContext.ShouldGetProjectDirectoryInfo(this.GetType());

            #region test peoperties:

            var file = this.TestContext.Properties["file"].ToString();
            file = Path.Combine(projectDirectoryInfo.FullName, file);
            this.TestContext.ShouldFindFile(file);

            var uri = new Uri(this.TestContext.Properties["uri"].ToString(), UriKind.Absolute);

            #endregion

            await (new HttpClient()).DownloadToFileAsync(uri, file);
        }


        [TestCategory("Integration")]
        [TestMethod]
        [TestProperty("file", @"Extensions\HttpClientExtensionsTest.ShouldDownloadToFileAsync.txt")]
        [TestProperty("uri", "https://github.com/BryanWilhite/SonghayCore/blob/master/README.md")]
        public async Task ShouldDownloadToStringAsync()
        {
            var projectDirectoryInfo = this.TestContext.ShouldGetProjectDirectoryInfo(this.GetType());

            #region test peoperties:

            var file = this.TestContext.Properties["file"].ToString();
            file = Path.Combine(projectDirectoryInfo.FullName, file);
            this.TestContext.ShouldFindFile(file);

            var uri = new Uri(this.TestContext.Properties["uri"].ToString(), UriKind.Absolute);

            #endregion

            var response = await (new HttpClient()).DownloadToStringAsync(uri);
            Assert.IsFalse(string.IsNullOrEmpty(response), "The expected response is not here.");
            this.TestContext.WriteLine(response);
        }

    }
}

#endif
