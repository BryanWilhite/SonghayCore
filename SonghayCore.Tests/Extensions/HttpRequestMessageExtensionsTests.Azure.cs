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

    static string GetLocation(string accountName, string containerName, string blobName) =>
        $"https://{accountName}.blob.core.windows.net/{containerName}/{blobName}";

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

    static async Task<string> DownloadFileToStringAsync(string connectionString, string containerName, string fileName)
    {
        var metadata = GetCloudStorageMetadata(connectionString);
        var location = GetLocation(metadata.accountName, containerName, fileName);

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

    [Fact]
    public void ShouldGetConnectionStringFromEnvironmentVariable()
    {
        var actual = GetConnectionStringFromEnvironmentVariable();
        Assert.False(string.IsNullOrWhiteSpace(actual));
    }

    [Theory]
    [InlineData("integration-test-container", "foo-two.txt")]
    public async Task DownloadFileToStringAsync_Test(string containerName, string fileName)
    {
        var connectionString = GetConnectionStringFromEnvironmentVariable();
        var actual = await DownloadFileToStringAsync(connectionString, containerName, fileName);
        Assert.False(string.IsNullOrWhiteSpace(actual));
    }
}