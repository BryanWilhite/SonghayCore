using Newtonsoft.Json.Linq;
using System.Net;
using Songhay.Models;
using Tavis.UriTemplates;
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
        [InlineData("https://songhaystorage.blob.core.windows.net/studio-public/json-generator.json")]
        public async Task StreamToInstance_Newtonsoft_Test(string location)
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
                .StreamToInstanceAsync<DisplayItemModel[]>(settings: null);

            Assert.NotNull(instance);
            Assert.NotEmpty(instance);
        }

        [Theory]
        [InlineData("photos/{photoId}", 1)]
        public async Task ToJContainerAsync_Test(string input, int id)
        {
            var template = new UriTemplate($"{LIVE_API_BASE_URI}/{input}");
            var uri = template.BindByPosition($"{id}");
            var jO = JObject.Parse("{\"output\": null}");

            var content = await new HttpRequestMessage(HttpMethod.Get, uri)
                .GetContentAsync(async response =>
                {
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                    jO["output"] = await response.ToJContainerAsync();
                });

            this._testOutputHelper.WriteLine(jO.ToString());
        }

        [Theory]
        [InlineData("photos")]
        public async Task ToJContainerAsync_JArray_Test(string input)
        {
            var builder = new UriBuilder(LIVE_API_BASE_URI);
            builder.Path = input;
            var uri = builder.Uri;
            var jO = JObject.Parse("{\"output\": null}");

            var content = await new HttpRequestMessage(HttpMethod.Get, uri)
                .GetContentAsync(async response =>
                {
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                    jO["output"] = await response.ToJContainerAsync();
                });
            this._testOutputHelper.WriteLine(jO.ToString());
        }

        const string LIVE_API_BASE_URI = "http://jsonplaceholder.typicode.com";

        readonly ITestOutputHelper _testOutputHelper;
    }
}
