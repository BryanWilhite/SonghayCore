using Songhay.Extensions;
using Songhay.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests.Extensions
{
    public class HttpResponseMessageExtensionsTests
    {
        public HttpResponseMessageExtensionsTests(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
        }

        [Theory]
        [InlineData("https://songhaystorage.blob.core.windows.net/studio-public/songhay_icon.png", "../../../content")]
        public async Task DownloadByteArrayToFile_Test(string location, string target)
        {
            var root = ProgramAssemblyUtility.GetPathFromAssembly(this.GetType().Assembly, target);
            var rootInfo = new DirectoryInfo(root);
            Assert.True(rootInfo.Exists);

            var uri = new Uri(location);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            using var response = await request.SendAsync();

            var targetInfo = new FileInfo(rootInfo.ToCombinedPath(uri.ToFileName()));
            this._testOutputHelper.WriteLine($"Downloading to {targetInfo.FullName}...");

            await response.DownloadByteArrayToFileAsync(targetInfo);
        }

        [Theory]
        [InlineData("https://songhaystorage.blob.core.windows.net/studio-dash/app.json", "../../../json")]
        public async Task DownloadStringToFile_Test(string location, string target)
        {
            var root = ProgramAssemblyUtility.GetPathFromAssembly(this.GetType().Assembly, target);
            var rootInfo = new DirectoryInfo(root);
            Assert.True(rootInfo.Exists);

            var uri = new Uri(location);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            using var response = await request.SendAsync();

            var targetInfo = new FileInfo(rootInfo.ToCombinedPath(uri.ToFileName()));
            this._testOutputHelper.WriteLine($"Downloading to {targetInfo.FullName}...");

            await response.DownloadStringToFileAsync(targetInfo);
        }

        [Theory]
        [InlineData("https://songhaystorage.blob.core.windows.net/studio-public/json-generator.json")]
        public async Task StreamToInstance_Test(string location)
        {
            // https://next.json-generator.com/NyyaU2K8q

            var uri = new Uri(location);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            using var response = await request
                .SendAsync(
                    requestMessageAction: null,
                    optionalClientGetter: null,
                    HttpCompletionOption.ResponseHeadersRead);

            var instance = await response
                .StreamToInstanceAsync<DisplayItemModel[]>(options: null);

            Assert.NotNull(instance);
            Assert.NotEmpty(instance);
        }

        const string LIVE_API_BASE_URI = "http://jsonplaceholder.typicode.com";

        readonly ITestOutputHelper _testOutputHelper;
    }
}
