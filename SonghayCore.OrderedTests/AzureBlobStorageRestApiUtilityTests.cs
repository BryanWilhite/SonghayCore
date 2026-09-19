using System.Text.Json;
using Songhay.Tests.Orderers;

namespace Songhay.Tests;

public class AzureBlobStorageRestApiUtilityTests : OrderedTestBase
{
    public AzureBlobStorageRestApiUtilityTests(ITestOutputHelper testOutputHelper)
    {
        AssertNoXUnitException();

        _testOutputHelper = testOutputHelper;
    }

    static string? GetConnectionStringFromEnvironmentVariable()
    {
        var json = ProgramMetadataUtility.GetJsonForProgramMetadataFromEnvironment();

        if (string.IsNullOrWhiteSpace(json)) return null;

        using var jDoc = JsonDocument.Parse(json);
        var actual = jDoc.RootElement
            .GetProperty("ProgramMetadata")
            .GetProperty("CloudStorageSet")
            .GetProperty("SonghayCloudStorage")
            .GetProperty("general-purpose-v1")
            .GetString();

        actual.ThrowWhenNullOrWhiteSpace();

        return actual;
    }

    [SkippableFact, TestOrder(ordinal: 0, reason: "verify environment...")]
    public void ShouldGetConnectionStringFromEnvironmentVariable()
    {
        var actual = GetConnectionStringFromEnvironmentVariable();
        Assert.False(string.IsNullOrWhiteSpace(actual));
    }

    [SkippableTheory, TestOrder(ordinal: 1, reason: "upload `hello.json`...")]
    [InlineData(ContainerName, "hello.json", @"{ ""root"": ""hello!"", ""isGreeting"": true }")]
    public async Task UploadBlobAsync_Test(string containerName, string fileName, string content)
    {
        var connectionString = GetConnectionStringFromEnvironmentVariable();
        Skip.If(string.IsNullOrWhiteSpace(connectionString));
        
        await AzureBlobStorageRestApiUtility
            .UploadBlobAsync(connectionString, containerName, fileName, content,
                () => _httpClientFactory.CreateClient(nameof(UploadBlobAsync_Test)));
    }

    [SkippableTheory, TestOrder(ordinal: 2, reason: "list container...")]
    [InlineData(ContainerName)]
    public async Task ListContainerAsync_Test(string containerName)
    {
        var connectionString = GetConnectionStringFromEnvironmentVariable();
        Skip.If(string.IsNullOrWhiteSpace(connectionString));

        var actual = await AzureBlobStorageRestApiUtility
            .ListContainerAsync(connectionString, containerName,
                () => _httpClientFactory.CreateClient(nameof(ListContainerAsync_Test)));
        Assert.False(string.IsNullOrWhiteSpace(actual));
        _testOutputHelper.WriteLine(actual);
    }

    [SkippableTheory, TestOrder(ordinal: 3, reason: "delete `hello.json`...")]
    [InlineData(ContainerName, "hello.json")]
    public async Task DeleteBlobAsync_Test(string containerName, string fileName)
    {
        var connectionString = GetConnectionStringFromEnvironmentVariable();
        Skip.If(string.IsNullOrWhiteSpace(connectionString));

        await AzureBlobStorageRestApiUtility
            .DeleteBlobAsync(connectionString, containerName, fileName,
                () => _httpClientFactory.CreateClient(nameof(DeleteBlobAsync_Test)));
    }

    [SkippableTheory, TestOrder(ordinal: 4, reason: "list container after delete...")]
    [InlineData(ContainerName)]
    public async Task ListContainerAsync_2_Test(string containerName)
    {
        var connectionString = GetConnectionStringFromEnvironmentVariable();
        Skip.If(string.IsNullOrWhiteSpace(connectionString));

        var actual = await AzureBlobStorageRestApiUtility
            .ListContainerAsync(connectionString, containerName,
                () => _httpClientFactory.CreateClient(nameof(ListContainerAsync_2_Test)));
        Assert.False(string.IsNullOrWhiteSpace(actual));
        _testOutputHelper.WriteLine(actual);
    }

    [SkippableTheory, TestOrder(ordinal: 5, reason: "download `foo-two.txt`...")]
    [InlineData(ContainerName, "foo-two.txt")]
    public async Task DownloadBlobToStringAsync_Test(string containerName, string fileName)
    {
        var connectionString = GetConnectionStringFromEnvironmentVariable();
        Skip.If(string.IsNullOrWhiteSpace(connectionString));

        var actual = await AzureBlobStorageRestApiUtility
            .DownloadBlobToStringAsync(connectionString, containerName, fileName,
                () => _httpClientFactory.CreateClient(nameof(DownloadBlobToStringAsync_Test)));
        Assert.False(string.IsNullOrWhiteSpace(actual));
    }

    const string ContainerName = "integration-test-container";

    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IHttpClientFactory _httpClientFactory =
        ServiceCollectionUtility.GetHttpClientFactory(serviceCollection: null);
}
