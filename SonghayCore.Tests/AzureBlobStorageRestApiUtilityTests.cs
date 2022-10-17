using System.Text.Json;

namespace Songhay.Tests;

public class AzureBlobStorageRestApiUtilityTests
{
    public AzureBlobStorageRestApiUtilityTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    static string GetConnectionStringFromEnvironmentVariable()
    {
        var path = Environment.GetEnvironmentVariable("SONGHAY_APP_SETTINGS_PATH");
        path.ThrowWhenNullOrWhiteSpace();

        var json = File.ReadAllText(path);
        Assert.False(string.IsNullOrWhiteSpace(json));

        var jDoc = JsonDocument.Parse(json);
        var actual = jDoc.RootElement
            .GetProperty("ProgramMetadata")
            .GetProperty("CloudStorageSet")
            .GetProperty("SonghayCloudStorage")
            .GetProperty("general-purpose-v1")
            .GetString();
        Assert.False(string.IsNullOrWhiteSpace(actual));

        return actual!;
    }

    [Fact(Skip = "these tests require a local environment variable")]
    public void ShouldGetConnectionStringFromEnvironmentVariable()
    {
        var actual = GetConnectionStringFromEnvironmentVariable();
        Assert.False(string.IsNullOrWhiteSpace(actual));
    }

    [Theory(Skip = "these tests require a local environment variable")]
    [InlineData(ContainerName, "hello.json")]
    public async Task DeleteBlobAsync_Test(string containerName, string fileName)
    {
        var connectionString = GetConnectionStringFromEnvironmentVariable();
        await AzureBlobStorageRestApiUtility.DeleteBlobAsync(connectionString, containerName, fileName);
    }

    [Theory(Skip = "these tests require a local environment variable")]
    [InlineData(ContainerName, "foo-two.txt")]
    public async Task DownloadBlobToStringAsync_Test(string containerName, string fileName)
    {
        var connectionString = GetConnectionStringFromEnvironmentVariable();
        var actual = await AzureBlobStorageRestApiUtility.DownloadBlobToStringAsync(connectionString, containerName, fileName);
        Assert.False(string.IsNullOrWhiteSpace(actual));
    }

    [Theory(Skip = "these tests require a local environment variable")]
    [InlineData(ContainerName)]
    public async Task ListContainerAsync_Test(string containerName)
    {
        var connectionString = GetConnectionStringFromEnvironmentVariable();
        var actual = await AzureBlobStorageRestApiUtility.ListContainerAsync(connectionString, containerName);
        Assert.False(string.IsNullOrWhiteSpace(actual));
        _testOutputHelper.WriteLine(actual);
    }

    [Theory(Skip = "these tests require a local environment variable")]
    [InlineData(ContainerName, "hello.json", @"{ ""root"": ""hello!"", ""isGreeting"": true }")]
    public async Task UploadBlobAsync_Test(string containerName, string fileName, string content)
    {
        var connectionString = GetConnectionStringFromEnvironmentVariable();
        await AzureBlobStorageRestApiUtility.UploadBlobAsync(connectionString, containerName, fileName, content);
    }

    const string ContainerName = "integration-test-container";

    readonly ITestOutputHelper _testOutputHelper;
}
