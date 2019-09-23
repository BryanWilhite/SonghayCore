using Songhay.Extensions;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests.Extensions
{
    public class HttpClientExtensionsTest
    {
        public HttpClientExtensionsTest(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
            this._basePath = FrameworkAssemblyUtility.GetPathFromAssembly(this.GetType().Assembly, "../../../");
        }

        [Trait("Category", "Integration")]
        [Theory]
        [InlineData(
            @"Extensions\HttpClientExtensionsTest.ShouldDownloadToFileAsync.txt",
            "https://github.com/BryanWilhite/SonghayCore/blob/master/README.md")]
        public async Task ShouldDownloadToFileAsync(string file, string uri)
        {
            file = FrameworkFileUtility.GetCombinedPath(this._basePath, file);
            Assert.True(File.Exists(file), "The expected target download file is not here.");

            await GetHttpClient().DownloadToFileAsync(new Uri(uri, UriKind.Absolute), file);
        }

        [Trait("Category", "Integration")]
        [Theory]
        [InlineData(
            @"Extensions\HttpClientExtensionsTest.ShouldDownloadToFileAsync.txt",
            "https://github.com/BryanWilhite/SonghayCore/blob/master/README.md")]
        public async Task ShouldDownloadToStringAsync(string file, string uri)
        {
            file = FrameworkFileUtility.GetCombinedPath(this._basePath, file);
            Assert.True(File.Exists(file), "The expected target download file is not here.");

            var response = await GetHttpClient().DownloadToStringAsync(new Uri(uri, UriKind.Absolute));
            Assert.False(string.IsNullOrEmpty(response), "The expected response is not here.");
            this._testOutputHelper.WriteLine(response);
        }

        static HttpClient GetHttpClient() => HttpClientLazy.Value;

        static readonly Lazy<HttpClient> HttpClientLazy =
            new Lazy<HttpClient>(() => new HttpClient(), LazyThreadSafetyMode.PublicationOnly);

        readonly ITestOutputHelper _testOutputHelper;
        readonly string _basePath;
    }
}
