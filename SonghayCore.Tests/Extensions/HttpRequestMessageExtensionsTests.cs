using System.Net;
using System.Text.Json.Nodes;
using Tavis.UriTemplates;

namespace Songhay.Tests.Extensions;

public class HttpRequestMessageExtensionsTests(ITestOutputHelper helper)
{
    [Trait(TestScalars.XunitCategory, TestScalars.XunitCategoryIntegrationManualTest)]
    [SkippableTheory]
    [InlineData(@"photos/{photoId}", 1)]
    public async Task ShouldDeletePhoto(string input, int id)
    {
        Skip.If(TestScalars.IsNotDebugging, TestScalars.ReasonForSkippingWhenNotDebugging);

        UriTemplate template = new UriTemplate($"{LiveApiBaseUri}/{input}");
        Uri? uri = template.BindByPosition($"{id}");
        HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Delete, uri);
        HttpResponseMessage response = await message.SendAsync(() => _httpClientFactory.CreateClient(nameof(ShouldDeletePhoto)));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Trait(TestScalars.XunitCategory, TestScalars.XunitCategoryIntegrationTest)]
    [Theory]
    [InlineData(@"photos/{photoId}", 1)]
    public async Task ShouldGetPhoto(string input, int id)
    {
        UriTemplate template = new UriTemplate($"{LiveApiBaseUri}/{input}");
        Uri? uri = template.BindByPosition($"{id}");
        string content = await new HttpRequestMessage(HttpMethod.Get, uri)
            .GetContentAsync(() => _httpClientFactory.CreateClient(nameof(ShouldDeletePhoto)),
                response => Assert.Equal(HttpStatusCode.OK, response.StatusCode));
        helper.WriteLine(content);
    }

    [Trait(TestScalars.XunitCategory, TestScalars.XunitCategoryIntegrationManualTest)]
    [SkippableTheory]
    [InlineData(@"photos/{photoId}", 1, 999)]
    public async Task ShouldPatchPhoto(string input, int id, int albumId)
    {
        Skip.If(TestScalars.IsNotDebugging, TestScalars.ReasonForSkippingWhenNotDebugging);

        JsonObject body = new JsonObject { [nameof(albumId)] = albumId };

        UriTemplate template = new UriTemplate($"{LiveApiBaseUri}/{input}");
        Uri? uri = template.BindByPosition($"{id}");
        HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Patch, uri);
        HttpResponseMessage response = await message.SendBodyAsync(body.ToJsonString(),
            () => _httpClientFactory.CreateClient(nameof(ShouldPatchPhoto)));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string content = await response.Content.ReadAsStringAsync();
        helper.WriteLine(content);
        JsonElement jO = JsonElement.Parse(content);
        Assert.Equal(albumId, jO.GetJsonChildElementOrNull(nameof(albumId))?.GetInt32());
    }

    [Trait(TestScalars.XunitCategory, TestScalars.XunitCategoryIntegrationManualTest)]
    [SkippableTheory]
    [InlineData(@"photos/{photoId}", 1, 999)]
    public async Task ShouldPutPhoto(string input, int id, int albumId)
    {
        Skip.If(TestScalars.IsNotDebugging, TestScalars.ReasonForSkippingWhenNotDebugging);

        JsonObject body = new JsonObject
        {
            [nameof(id)] = id,
            [nameof(albumId)] = albumId,
            ["thumbnailUrl"] = "https://via.placeholder.com/150/92c952",
            ["title"] = "accusamus beatae ad facilis cum similique qui sunt",
            ["url"] = "https://via.placeholder.com/600/92c952"
        };

        UriTemplate template = new UriTemplate($"{LiveApiBaseUri}/{input}");
        Uri? uri = template.BindByPosition($"{id}");
        HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Put, uri);
        HttpResponseMessage response = await message.SendBodyAsync(body.ToJsonString(),
            () => _httpClientFactory.CreateClient(nameof(ShouldPutPhoto)));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        string content = await response.Content.ReadAsStringAsync();
        helper.WriteLine(content);
        JsonElement jO = JsonElement.Parse(content);
        Assert.Equal(albumId, jO.GetJsonChildElementOrNull(nameof(albumId))?.GetInt32());
    }

    [Trait(TestScalars.XunitCategory, TestScalars.XunitCategoryIntegrationTest)]
    [Theory]
    [InlineData(@"photos/wrong/{photoId}", 1)]
    public async Task ShouldThrowNotFoundPhoto(string input, int id)
    {
        UriTemplate template = new UriTemplate($"{LiveApiBaseUri}/{input}");
        Uri? uri = template.BindByPosition($"{id}");
        HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, uri);
        HttpResponseMessage response = await message.SendAsync(() => _httpClientFactory.CreateClient(nameof(ShouldThrowNotFoundPhoto)));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    const string LiveApiBaseUri = "https://jsonplaceholder.typicode.com";

    private readonly IHttpClientFactory _httpClientFactory =
        ServiceCollectionUtility.GetHttpClientFactory(serviceCollection: null);
}
