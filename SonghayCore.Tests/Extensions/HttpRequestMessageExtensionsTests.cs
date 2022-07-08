using Newtonsoft.Json.Linq;
using Songhay.Extensions;
using Songhay.Net;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tavis.UriTemplates;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests.Extensions;

public class HttpRequestMessageExtensionsTests
{
    public HttpRequestMessageExtensionsTests(ITestOutputHelper helper)
    {
        this._testOutputHelper = helper;
    }

    [Theory(Skip = "slowwly server is down")]
    [InlineData("http://slowwly.robertomurray.co.uk/delay/3000/url/http://www.google.co.uk", 1)]
    public async Task ShouldCancel(string location, int timeInSeconds)
    {
        var handler = new TimeoutHandler
        {
            RequestTimeout = TimeSpan.FromSeconds(10),
            InnerHandler = new HttpClientHandler()
        };

        using (var cts = new CancellationTokenSource())
        using (var client = new HttpClient(handler))
        {
            try
            {
                client.Timeout = Timeout.InfiniteTimeSpan;

                this._testOutputHelper.WriteLine($"calling `{location}`...");
                var request = new HttpRequestMessage(HttpMethod.Get, location);

                cts.CancelAfter(TimeSpan.FromSeconds(timeInSeconds));

                await request.GetContentAsync(
                    responseMessageAction: null,
                    optionalClientGetter: () => client);
            }
            catch (Exception ex)
            {
                Assert.IsType<TaskCanceledException>(ex);
            }
        }
    }

    [Trait("Category", "Integration")]
    [DebuggerAttachedTheory]
    [InlineData(@"photos/{photoId}", 1)]
    public async Task ShouldDeletePhoto(string input, int id)
    {
        var template = new UriTemplate($"{LIVE_API_BASE_URI}/{input}");
        var uri = template.BindByPosition($"{id}");
        var message = new HttpRequestMessage(HttpMethod.Delete, uri);
        var response = await message.SendAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Trait("Category", "Integration")]
    [Theory]
    [InlineData(@"photos/{photoId}", 1)]
    public async Task ShouldGetPhoto(string input, int id)
    {
        var template = new UriTemplate($"{LIVE_API_BASE_URI}/{input}");
        var uri = template.BindByPosition($"{id}");
        var content = await new HttpRequestMessage(HttpMethod.Get, uri)
            .GetContentAsync(response => Assert.Equal(HttpStatusCode.OK, response.StatusCode));
        this._testOutputHelper.WriteLine(content);
    }

    [Trait("Category", "Integration")]
    [DebuggerAttachedTheory]
    [InlineData(@"photos/{photoId}", 1, 999)]
    public async Task ShouldPatchPhoto(string input, int id, int albumId)
    {
        var body = JObject.FromObject(new { albumId }).ToString();

        var template = new UriTemplate($"{LIVE_API_BASE_URI}/{input}");
        var uri = template.BindByPosition($"{id}");
        var message = new HttpRequestMessage(HttpMethod.Patch, uri);
        var response = await message.SendBodyAsync(body);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        this._testOutputHelper.WriteLine(content);
        var jO = JObject.Parse(content);
        Assert.Equal(albumId, jO.GetValue(nameof(albumId)).Value<int>());
    }

    [Trait("Category", "Integration")]
    [DebuggerAttachedTheory]
    [InlineData(@"photos/{photoId}", 1, 999)]
    public async Task ShouldPutPhoto(string input, int id, int albumId)
    {
        var body = JObject.FromObject(new
        {
            id,
            albumId,
            thumbnailUrl = "https://via.placeholder.com/150/92c952",
            title = "accusamus beatae ad facilis cum similique qui sunt",
            url = "https://via.placeholder.com/600/92c952"
        }).ToString();

        var template = new UriTemplate($"{LIVE_API_BASE_URI}/{input}");
        var uri = template.BindByPosition($"{id}");
        var message = new HttpRequestMessage(HttpMethod.Put, uri);
        var response = await message.SendBodyAsync(body);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        this._testOutputHelper.WriteLine(content);
        var jO = JObject.Parse(content);
        Assert.Equal(albumId, jO.GetValue(nameof(albumId)).Value<int>());
    }

    [Trait("Category", "Integration")]
    [Theory]
    [InlineData(@"photos/wrong/{photoId}", 1)]
    public async Task ShouldThrowNotFoundPhoto(string input, int id)
    {
        var template = new UriTemplate($"{LIVE_API_BASE_URI}/{input}");
        var uri = template.BindByPosition($"{id}");
        var message = new HttpRequestMessage(HttpMethod.Get, uri);
        var response = await message.SendAsync();

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory(Skip = "slowwly server is down")]
    [InlineData("http://slowwly.robertomurray.co.uk/delay/3000/url/http://www.google.co.uk", 1)]
    public async Task ShouldTimeout(string location, int timeInSeconds)
    {
        var handler = new TimeoutHandler
        {
            RequestTimeout = TimeSpan.FromSeconds(timeInSeconds),
            InnerHandler = new HttpClientHandler()
        };

        using (var cts = new CancellationTokenSource())
        using (var client = new HttpClient(handler))
        {
            try
            {
                client.Timeout = Timeout.InfiniteTimeSpan;

                this._testOutputHelper.WriteLine($"calling `{location}`...");
                var request = new HttpRequestMessage(HttpMethod.Get, location);

                await request.GetContentAsync(
                    responseMessageAction: null,
                    optionalClientGetter: () => client);
            }
            catch (Exception ex)
            {
                Assert.IsType<TimeoutException>(ex);
            }
        }
    }

    const string LIVE_API_BASE_URI = "http://jsonplaceholder.typicode.com";

    readonly ITestOutputHelper _testOutputHelper;
}