using Newtonsoft.Json.Linq;
using Songhay.Extensions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tavis.UriTemplates;
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

        [Trait("Category", "Integration")]
        [Theory]
        [InlineData(@"photos/{photoId}", 1)]
        public async Task ToJContainerAsync_Test(string input, int id)
        {
            var template = new UriTemplate($"{LIVE_API_BASE_URI}/{input}");
            var uri = template.BindByPosition($"{id}");
            var jO = JObject.Parse("{\"output\": null}");

            var content = await new HttpRequestMessage(HttpMethod.Get, uri)
                .GetServerResponseAsync(async response =>
                {
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                    jO["output"] = await response.ToJContainerAsync();
                });
            this._testOutputHelper.WriteLine(jO.ToString());
        }

        [Trait("Category", "Integration")]
        [Theory]
        [InlineData(@"photos")]
        public async Task ToJContainerAsync_JArray_Test(string input)
        {
            var builder = new UriBuilder(LIVE_API_BASE_URI);
            builder.Path = input;
            var uri = builder.Uri;
            var jO = JObject.Parse("{\"output\": null}");

            var content = await new HttpRequestMessage(HttpMethod.Get, uri)
                .GetServerResponseAsync(async response =>
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
