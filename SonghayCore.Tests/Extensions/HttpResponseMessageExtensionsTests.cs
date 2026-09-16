namespace Songhay.Tests.Extensions;

public class HttpResponseMessageExtensionsTests(ITestOutputHelper helper)
{
    [Theory]
    [ProjectDirectoryData("https://placecats.com/300/200", "content/jpg/placecat.jpg")]
    public async Task DownloadByteArrayToFile_Test(DirectoryInfo projectInfo, string location, string targetPath)
    {
        //arrange:
        Uri uri = new Uri(location);
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

        //act:
        using HttpResponseMessage response = await request.SendAsync(() => _httpClientFactory.CreateClient(nameof(DownloadByteArrayToFile_Test)));

        //archive:
        FileInfo targetInfo = new FileInfo(projectInfo.ToCombinedPath(targetPath));
        helper.WriteLine($"Downloading to {targetInfo.FullName}...");

        await response.DownloadByteArrayToFileAsync(targetInfo);
    }

    [Theory]
    [ProjectDirectoryData("https://api.chucknorris.io/jokes/random", "content/json/chucknorris.json")]
    public async Task DownloadStringToFile_Test(DirectoryInfo projectInfo, string location, string targetPath)
    {
        //arrange:
        Uri uri = new Uri(location);
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

        //act:
        using HttpResponseMessage response = await request.SendAsync(() => _httpClientFactory.CreateClient(nameof(DownloadStringToFile_Test)));

        //archive:
        FileInfo targetInfo = new FileInfo(projectInfo.ToCombinedPath(targetPath));
        helper.WriteLine($"Downloading to {targetInfo.FullName}...");

        await response.DownloadStringToFileAsync(targetInfo);
    }

    [Theory]
    [InlineData("https://api.chucknorris.io/jokes/random")]
    public async Task StreamToInstance_Test(string location)
    {
        //arrange:
        Uri uri = new Uri(location);
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

        //act:
        using HttpResponseMessage response = await request
            .SendAsync(
                () => _httpClientFactory.CreateClient(nameof(StreamToInstance_Test)),
                HttpCompletionOption.ResponseHeadersRead,
                requestMessageAction: null);

        //assert:
        Dictionary<string, object>? instance = await response
            .StreamToInstanceAsync<Dictionary<string, object>>(options: null);

        Assert.NotNull(instance);
        Assert.NotEmpty(instance);
        helper.WriteLine(instance.TryGetValueWithKey("value", throwException: true)!.ToString());
    }

    private readonly IHttpClientFactory _httpClientFactory =
        ServiceCollectionUtility.GetHttpClientFactory(serviceCollection: null);
}
