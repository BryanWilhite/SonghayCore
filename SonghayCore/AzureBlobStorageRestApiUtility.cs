using System.Data;
using System.Data.Common;

namespace Songhay;

/// <summary>
/// Shared REST routines for Azure BLOB Storage.
/// </summary>
public static class AzureBlobStorageRestApiUtility
{

    /// <summary>
    /// Builds the URI for Azure BLOB Storage.
    /// </summary>
    /// <param name="accountName">Azure BLOB Storage account name</param>
    /// <param name="containerName">Azure BLOB Storage container name</param>
    /// <returns>Returns the <see cref="UriBuilder"/></returns>
    public static UriBuilder GetStorageUriBuilder(string accountName, string containerName) =>
        new($"https://{accountName}.blob.core.windows.net/{containerName}");

    /// <summary>
    /// Gets the conventional Azure BLOB Storage metadata
    /// from the specified connection string.
    /// </summary>
    /// <param name="connectionString">Azure BLOB Storage connection string</param>
    /// <returns>Returns conventional named tuple.</returns>
    /// <exception cref="NullReferenceException"></exception>
    /// <exception cref="DataException"></exception>
    public static (string accountName, string accountKey, string apiVersion) GetCloudStorageMetadata(string connectionString)
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

    /// <summary>
    /// <c>DELETE</c>s the BLOB of the specified ‘file’ name.
    /// </summary>
    /// <param name="connectionString">Azure BLOB Storage connection string</param>
    /// <param name="containerName">Azure BLOB Storage container name</param>
    /// <param name="fileName">BLOB ‘file’ name</param>
    /// <exception cref="HttpRequestException"></exception>
    public static async Task DeleteBlobAsync(string connectionString, string containerName, string fileName)
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

    /// <summary>
    /// <c>GET</c>s the BLOB of the specified ‘file’ name.
    /// </summary>
    /// <param name="connectionString">Azure BLOB Storage connection string</param>
    /// <param name="containerName">Azure BLOB Storage container name</param>
    /// <param name="fileName">BLOB ‘file’ name</param>
    /// <returns>Returns the <see cref="string"/> contents of the file.</returns>
    /// <exception cref="HttpRequestException"></exception>
    public static async Task<string> DownloadBlobToStringAsync(string connectionString, string containerName, string fileName)
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

    /// <summary>
    /// Lists the contents of the specified BLOB container.
    /// </summary>
    /// <param name="connectionString">Azure BLOB Storage connection string</param>
    /// <param name="containerName">Azure BLOB Storage container name</param>
    /// <returns>Returns the contents of the container as an XML<see cref="string"/>.</returns>
    /// <exception cref="HttpRequestException"></exception>
    public static async Task<string> ListContainerAsync(string connectionString, string containerName)
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

    /// <summary>
    /// <c>PUT</c>s the BLOB of the specified ‘file’ name.
    /// </summary>
    /// <param name="connectionString">Azure BLOB Storage connection string</param>
    /// <param name="containerName">Azure BLOB Storage container name</param>
    /// <param name="fileName">BLOB ‘file’ name</param>
    /// <param name="content">The <see cref="string"/> contents of the ‘file.’</param>
    /// <exception cref="HttpRequestException"></exception>
    public static async Task UploadBlobAsync(string connectionString, string containerName, string fileName, string content)
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
}
