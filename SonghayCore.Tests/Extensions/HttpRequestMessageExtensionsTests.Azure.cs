using System.Data;
using System.Data.Common;
using System.Text.Json;

namespace Songhay.Tests.Extensions;

public partial class HttpRequestMessageExtensionsTests
{
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

    static UriBuilder GetStorageUriBuilder(string accountName, string containerName) =>
        new($"https://{accountName}.blob.core.windows.net/{containerName}");

    static (string accountName, string accountKey, string apiVersion) GetCloudStorageMetadata(string connectionString)
    {
        var builder = new DbConnectionStringBuilder
        {
            ConnectionString = connectionString
        };

        var values = new[] { "AccountName", "AccountKey" }.Select(key =>
        {
            if (!builder.ContainsKey(key))
            {
                throw new NullReferenceException($"The expected connection string key, `{key}`, is not here.");
            }

            var value = builder[key] as string;

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new NullReferenceException($"The expected connection string value for key, `{key}`, is not here.");
            }

            return value;

        }).ToArray();

        if (values.Length < 2) throw new DataException("The expected connection string values are not here.");

        (string accountName, string accountKey, string apiVersion) storageMetadata;

        storageMetadata.accountName = values[0];
        storageMetadata.accountKey = values[1];
        storageMetadata.apiVersion = "2019-12-12";

        return storageMetadata;
    }

    static async Task DeleteBlobAsync(string connectionString, string containerName, string fileName)
    {
        var metadata = GetCloudStorageMetadata(connectionString);
        var uriBuilder = GetStorageUriBuilder(metadata.accountName, containerName);
        uriBuilder.Path = $"{uriBuilder.Path}/{fileName}";
        var location = uriBuilder.Uri.OriginalString;

        using var request =
            new HttpRequestMessage(HttpMethod.Delete, location)
                .WithAzureStorageHeaders(
                    DateTime.UtcNow,
                    metadata.apiVersion,
                    metadata.accountName,
                    metadata.accountKey);
        using HttpResponseMessage response = await request.SendAsync();

        if (response.IsSuccessStatusCode) return;

        var message = $"Request for `{location}` returned status `{response.StatusCode}`.";
        throw new HttpRequestException(message);
    }

    static async Task<string> DownloadBlobToStringAsync(string connectionString, string containerName, string fileName)
    {
        var metadata = GetCloudStorageMetadata(connectionString);
        var uriBuilder = GetStorageUriBuilder(metadata.accountName, containerName);
        uriBuilder.Path = $"{uriBuilder.Path}/{fileName}";
        var location = uriBuilder.Uri.OriginalString;

        using var request =
            new HttpRequestMessage(HttpMethod.Get, location)
                .WithAzureStorageHeaders(
                    DateTime.UtcNow,
                    metadata.apiVersion,
                    metadata.accountName,
                    metadata.accountKey);
        using HttpResponseMessage response = await request.SendAsync();
        var s = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode) return s;

        var message = $"Request for `{location}` returned status `{response.StatusCode}`.";
        throw new HttpRequestException(message);
    }

    static async Task<string> ListContainerAsync(string connectionString, string containerName)
    {
        var metadata = GetCloudStorageMetadata(connectionString);
        var uriBuilder = GetStorageUriBuilder(metadata.accountName, containerName);
        uriBuilder.Query = "restype=container&comp=list";
        var location = uriBuilder.Uri.OriginalString;

        using var request =
            new HttpRequestMessage(HttpMethod.Get, location)
                .WithAzureStorageHeaders(
                    DateTime.UtcNow,
                    metadata.apiVersion,
                    metadata.accountName,
                    metadata.accountKey);
        using HttpResponseMessage response = await request.SendAsync();
        var s = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode) return s;

        var message = $"Request for `{location}` returned status `{response.StatusCode}`.";
        throw new HttpRequestException(message);
    }

    static async Task UploadBlobAsync(string connectionString, string containerName, string fileName, string content)
    {
        var metadata = GetCloudStorageMetadata(connectionString);
        var uriBuilder = GetStorageUriBuilder(metadata.accountName, containerName);
        uriBuilder.Path = $"{uriBuilder.Path}/{fileName}";
        var location = uriBuilder.Uri.OriginalString;

        using var request =
            new HttpRequestMessage(HttpMethod.Put, location)
                .WithAzureStorageBlockBlobContent(fileName, content)
                .WithAzureStorageHeaders(
                    DateTime.UtcNow,
                    metadata.apiVersion,
                    metadata.accountName,
                    metadata.accountKey);
        using HttpResponseMessage response = await request.SendAsync();

        if (response.IsSuccessStatusCode) return;

        var message = $"Request for `{location}` returned status `{response.StatusCode}`.";
        throw new HttpRequestException(message);
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
        await DeleteBlobAsync(connectionString, containerName, fileName);
    }

    [Theory(Skip = "these tests require a local environment variable")]
    [InlineData(ContainerName, "foo-two.txt")]
    public async Task DownloadBlobToStringAsync_Test(string containerName, string fileName)
    {
        var connectionString = GetConnectionStringFromEnvironmentVariable();
        var actual = await DownloadBlobToStringAsync(connectionString, containerName, fileName);
        Assert.False(string.IsNullOrWhiteSpace(actual));
    }

    [Theory(Skip = "these tests require a local environment variable")]
    [InlineData(ContainerName)]
    public async Task ListContainerAsync_Test(string containerName)
    {
        var connectionString = GetConnectionStringFromEnvironmentVariable();
        var actual = await ListContainerAsync(connectionString, containerName);
        Assert.False(string.IsNullOrWhiteSpace(actual));
        _testOutputHelper.WriteLine(actual);
    }

    [Theory(Skip = "these tests require a local environment variable")]
    [InlineData(ContainerName, "hello.json", @"{ ""root"": ""hello!"", ""isGreeting"": true }")]
    public async Task UploadBlobAsync_Test(string containerName, string fileName, string content)
    {
        var connectionString = GetConnectionStringFromEnvironmentVariable();
        await UploadBlobAsync(connectionString, containerName, fileName, content);
    }

    const string ContainerName = "integration-test-container";
}
