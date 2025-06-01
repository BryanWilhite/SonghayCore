namespace Songhay.Net;

/// <summary>
/// The default implementation of <see cref="IApiRequestStrategy"/>
/// for Azure Storage BLOBs.
/// </summary>
/// <remarks>
/// This implementation may seem like overkill but day-job experience informs me
/// that many C# developers are not comfortable with <see cref="Func{TResult}"/>.
/// So this class serves as a wrapper for this more functional approach.
/// </remarks>
public class AzureBlobApiRequestStrategy(
    string? accountName,
    string? accountKey,
    string? containerName,
    string? apiVersion) : IApiRequestStrategy
{

    /// <summary>
    /// Returns the conventional ID or tag of this instance.
    /// </summary>
    /// <remarks>
    /// This identifier is intended for asserting that an expected ID is present
    /// which is useful for ensuring that an extension method is operating on the expected instance.
    /// </remarks>
    public string InstanceTag { get; } = containerName.ToReferenceTypeValueOrThrow();

    /// <summary>
    /// Returns an <see cref="HttpRequestMessage"/> needed to call the API
    /// </summary>
    public HttpRequestMessage GenerateHttpRequestMessage<TData>(TData? requestData)
    {
        string? blobName = requestData as string;

        blobName.ThrowWhenNullOrWhiteSpace();

        accountName.ThrowWhenNullOrWhiteSpace();
        accountKey.ThrowWhenNullOrWhiteSpace();
        containerName.ThrowWhenNullOrWhiteSpace();

        string location = GetLocation(accountName, containerName, blobName);

        return new HttpRequestMessage(HttpMethod.Get, location)
            .WithAzureStorageHeaders(
                DateTime.UtcNow,
                apiVersion,
                accountName,
                accountKey);
    }

    internal static string GetLocation(string accountName, string containerName, string blobName) =>
        $"https://{accountName}.blob.core.windows.net/{containerName}/{blobName}";
}
