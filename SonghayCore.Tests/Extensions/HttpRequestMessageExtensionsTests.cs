using System.Net;
using System.Text.Json.Nodes;
using Tavis.UriTemplates;

using Songhay.Net;

namespace Songhay.Tests.Extensions;

public class HttpRequestMessageExtensionsTests(ITestOutputHelper helper)
{
    [Theory(Skip = "slowwly server is down")]
    [InlineData("https://slowwly.robertomurray.co.uk/delay/3000/url/http://www.google.co.uk", 1)]
    public async Task ShouldCancel(string location, int timeInSeconds)
    {
        var handler = new TimeoutHandler
        {
            RequestTimeout = TimeSpan.FromSeconds(10),
            InnerHandler = new HttpClientHandler()
        };

        using var cts = new CancellationTokenSource();
        using var client = new HttpClient(handler);
        try
        {
            client.Timeout = Timeout.InfiniteTimeSpan;

            helper.WriteLine($"calling `{location}`...");
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

    [Trait(TestScalars.XunitCategory, TestScalars.XunitCategoryIntegrationManualTest)]
    [SkippableTheory]
    [InlineData(@"photos/{photoId}", 1)]
    public async Task ShouldDeletePhoto(string input, int id)
    {
        Skip.If(TestScalars.IsNotDebugging, TestScalars.ReasonForSkippingWhenNotDebugging);

        var template = new UriTemplate($"{LiveApiBaseUri}/{input}");
        var uri = template.BindByPosition($"{id}");
        var message = new HttpRequestMessage(HttpMethod.Delete, uri);
        var response = await message.SendAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Trait(TestScalars.XunitCategory, TestScalars.XunitCategoryIntegrationTest)]
    [Theory]
    [InlineData(@"photos/{photoId}", 1)]
    public async Task ShouldGetPhoto(string input, int id)
    {
        var template = new UriTemplate($"{LiveApiBaseUri}/{input}");
        var uri = template.BindByPosition($"{id}");
        var content = await new HttpRequestMessage(HttpMethod.Get, uri)
            .GetContentAsync(response => Assert.Equal(HttpStatusCode.OK, response.StatusCode));
        helper.WriteLine(content);
    }

    [Trait(TestScalars.XunitCategory, TestScalars.XunitCategoryIntegrationManualTest)]
    [SkippableTheory]
    [InlineData(@"photos/{photoId}", 1, 999)]
    public async Task ShouldPatchPhoto(string input, int id, int albumId)
    {
        Skip.If(TestScalars.IsNotDebugging, TestScalars.ReasonForSkippingWhenNotDebugging);

        var body = new JsonObject { [nameof(albumId)] = albumId };

        var template = new UriTemplate($"{LiveApiBaseUri}/{input}");
        var uri = template.BindByPosition($"{id}");
        var message = new HttpRequestMessage(HttpMethod.Patch, uri);
        var response = await message.SendBodyAsync(body.ToJsonString());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        helper.WriteLine(content);
        var jO = JsonElement.Parse(content);
        Assert.Equal(albumId, jO.GetJsonChildElementOrNull(nameof(albumId))?.GetInt32());
    }

    [Trait(TestScalars.XunitCategory, TestScalars.XunitCategoryIntegrationManualTest)]
    [SkippableTheory]
    [InlineData(@"photos/{photoId}", 1, 999)]
    public async Task ShouldPutPhoto(string input, int id, int albumId)
    {
        Skip.If(TestScalars.IsNotDebugging, TestScalars.ReasonForSkippingWhenNotDebugging);

        var body = new JsonObject
        {
            [nameof(id)] = id,
            [nameof(albumId)] = albumId,
            ["thumbnailUrl"] = "https://via.placeholder.com/150/92c952",
            ["title"] = "accusamus beatae ad facilis cum similique qui sunt",
            ["url"] = "https://via.placeholder.com/600/92c952"
        };

        var template = new UriTemplate($"{LiveApiBaseUri}/{input}");
        var uri = template.BindByPosition($"{id}");
        var message = new HttpRequestMessage(HttpMethod.Put, uri);
        var response = await message.SendBodyAsync(body.ToJsonString());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        helper.WriteLine(content);
        var jO = JsonElement.Parse(content);
        Assert.Equal(albumId, jO.GetJsonChildElementOrNull(nameof(albumId))?.GetInt32());
    }

    [Trait(TestScalars.XunitCategory, TestScalars.XunitCategoryIntegrationTest)]
    [Theory]
    [InlineData(@"photos/wrong/{photoId}", 1)]
    public async Task ShouldThrowNotFoundPhoto(string input, int id)
    {
        var template = new UriTemplate($"{LiveApiBaseUri}/{input}");
        var uri = template.BindByPosition($"{id}");
        var message = new HttpRequestMessage(HttpMethod.Get, uri);
        var response = await message.SendAsync();

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory(Skip = "slowwly server is down")]
    [InlineData("https://slowwly.robertomurray.co.uk/delay/3000/url/http://www.google.co.uk", 1)]
    public async Task ShouldTimeout(string location, int timeInSeconds)
    {
        var handler = new TimeoutHandler
        {
            RequestTimeout = TimeSpan.FromSeconds(timeInSeconds),
            InnerHandler = new HttpClientHandler()
        };

        using var cts = new CancellationTokenSource();
        using var client = new HttpClient(handler);
        try
        {
            client.Timeout = Timeout.InfiniteTimeSpan;

            helper.WriteLine($"calling `{location}`...");
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

    const string LiveApiBaseUri = "https://jsonplaceholder.typicode.com";
}