namespace Songhay.Tests.Extensions;

public class HttpClientExtensionsTests
{
    public HttpClientExtensionsTests(ITestOutputHelper helper)
    {
        _testOutputHelper = helper;
        _basePath = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, "../../../");
    }

    [Theory]
    [InlineData(
        @"Extensions\HttpClientExtensionsTests.ShouldDownloadToFileAsync.txt",
        "https://github.com/BryanWilhite/SonghayCore/blob/master/README.md")]
    public async Task DownloadToFileAsync_Test(string file, string uri)
    {
        file = ProgramFileUtility.GetCombinedPath(_basePath, file);
        Assert.True(File.Exists(file), "The expected target download file is not here.");

        await GetHttpClient().DownloadToFileAsync(new Uri(uri, UriKind.Absolute), file);
    }

    [Theory]
    [InlineData(
        @"Extensions\HttpClientExtensionsTests.ShouldDownloadToFileAsync.txt",
        "https://github.com/BryanWilhite/SonghayCore/blob/master/README.md")]
    public async Task DownloadToStringAsync_Test(string file, string uri)
    {
        file = ProgramFileUtility.GetCombinedPath(_basePath, file);
        Assert.True(File.Exists(file), "The expected target download file is not here.");

        var content = await GetHttpClient().DownloadToStringAsync(new Uri(uri, UriKind.Absolute));
        Assert.False(string.IsNullOrWhiteSpace(content), "The expected response is not here.");
        _testOutputHelper.WriteLine(content);
    }

    [Theory]
    [InlineData(
        @"Extensions\HttpClientExtensionsTests.ShouldDownloadToFileAsync.txt",
        "https://github.com/BryanWilhite/SonghayCore/blob/master/README.md")]
    public async Task DownloadToStringAsync_HttpRequestMessage_Test(string file, string uri)
    {
        file = ProgramFileUtility.GetCombinedPath(_basePath, file);
        Assert.True(File.Exists(file), "The expected target download file is not here.");

        var response = await GetHttpClient().DownloadToStringAsync(new Uri(uri, UriKind.Absolute), requestMessageAction: null);
        Assert.False(string.IsNullOrWhiteSpace(response), "The expected response is not here.");
        _testOutputHelper.WriteLine(response);
    }

    static HttpClient GetHttpClient() => HttpClientLazy.Value;

    static readonly Lazy<HttpClient> HttpClientLazy =
        new Lazy<HttpClient>(() => new HttpClient(), LazyThreadSafetyMode.PublicationOnly);

    readonly ITestOutputHelper _testOutputHelper;
    readonly string _basePath;
}