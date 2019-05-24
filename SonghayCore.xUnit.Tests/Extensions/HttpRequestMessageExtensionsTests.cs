using Songhay.Extensions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tavis.UriTemplates;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests.Extensions
{
    public class HttpRequestMessageExtensionsTests
    {
        public HttpRequestMessageExtensionsTests(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
        }

        [Theory]
        [InlineData(@"photos/{photoId}", 1)]
        public async Task ShouldDeletePhoto(string input, int id)
        {
            var template = new UriTemplate($"{LIVE_API_BASE_URI}/{input}");
            var uri = template.BindByPosition($"{id}");
            var message = new HttpRequestMessage(HttpMethod.Delete, uri);
            var response = await message.SendAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(@"photos/{photoId}", 1)]
        public async Task ShouldGetPhoto(string input, int id)
        {
            var template = new UriTemplate($"{LIVE_API_BASE_URI}/{input}");
            var uri = template.BindByPosition($"{id}");
            var content = await new HttpRequestMessage(HttpMethod.Get, uri)
                .GetServerResponseAsync(response => Assert.Equal(HttpStatusCode.OK, response.StatusCode));
            this._testOutputHelper.WriteLine(content);
        }

        const string LIVE_API_BASE_URI = "http://jsonplaceholder.typicode.com";

        readonly ITestOutputHelper _testOutputHelper;
    }
}
