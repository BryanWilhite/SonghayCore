using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace Songhay.Extensions;

public static partial class HttpRequestMessageExtensions
{
    /// <summary>
    /// Derives the <see cref="AuthenticationHeaderValue"/>
    /// from the <see cref="HttpRequestMessage"/>.
    /// </summary>
    /// <param name="request">the <see cref="HttpRequestMessage"/></param>
    /// <param name="storageAccountName">the Azure Storage account name</param>
    /// <param name="storageAccountKey">the Azure Storage account shared key</param>
    /// <param name="eTag">entity tag for Web cache validation</param>
    /// <param name="md5">The MD5 (message-digest algorithm) hash</param>
    /// <returns></returns>
    /// <remarks>
    /// There are two Authorization Header schemes supported: SharedKey and SharedKeyLite. This member supports only one of them: SharedKey.
    /// For more detail, see “Specifying the Authorization header”
    /// [ https://docs.microsoft.com/en-us/rest/api/storageservices/authorize-with-shared-key#specifying-the-authorization-header ]
    /// 
    /// See also: “Authorize requests to Azure Storage”
    /// [ https://docs.microsoft.com/en-us/rest/api/storageservices/authorize-requests-to-azure-storage ]
    ///
    /// See also: https://github.com/Azure-Samples/storage-dotnet-rest-api-with-auth/tree/master
    ///
    /// Provide the md5 and it will check and make sure it matches the requested blob's md5.
    /// If it doesn't match, it won't return a value.
    ///
    /// Provide an eTag, and it will only make changes to a blob if the current eTag matches,
    /// to ensure you don't overwrite someone else's changes.
    /// </remarks>
    public static AuthenticationHeaderValue ToAzureStorageAuthorizationHeader(this HttpRequestMessage? request,
        string? storageAccountName, string? storageAccountKey, string? eTag, string? md5)
    {
        ArgumentNullException.ThrowIfNull(request);
        storageAccountKey.ThrowWhenNullOrWhiteSpace();

        var signatureBytes = request.ToAzureStorageSignature(storageAccountName, eTag, md5);

        var sha256 = new HMACSHA256(Convert.FromBase64String(storageAccountKey));

        const string scheme = "SharedKey";
        var parameter = $"{storageAccountName}:{Convert.ToBase64String(sha256.ComputeHash(signatureBytes))}";

        var value = new AuthenticationHeaderValue(scheme, parameter);

        return value;
    }

    /// <summary>
    /// Returns headers, starting with <c>x-ms-</c>,
    /// in a canonical format.
    /// </summary>
    /// <param name="request">the <see cref="HttpRequestMessage"/></param>
    /// <returns></returns>
    /// <remarks>
    /// See https://github.com/Azure-Samples/storage-dotnet-rest-api-with-auth/tree/master
    ///
    /// See https://docs.microsoft.com/en-us/rest/api/storageservices/authorize-requests-to-azure-storage
    ///
    /// See https://docs.microsoft.com/en-us/rest/api/storageservices/authorize-with-shared-key
    ///
    /// See http://en.wikipedia.org/wiki/Canonicalization
    /// </remarks>
    public static string ToAzureStorageCanonicalizedHeaders(this HttpRequestMessage? request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var xMsHeaders = request.Headers
            .Where(pair => pair.Key.StartsWith("x-ms-", StringComparison.OrdinalIgnoreCase))
            .OrderBy(pair => pair.Key)
            .Select(pair => new {Key = pair.Key.ToLowerInvariant(), pair.Value});

        var sb = new StringBuilder();

        foreach (var pair in xMsHeaders)
        {
            var innerBuilder = new StringBuilder(pair.Key);
            char separator = ':';

            foreach (string headerValues in pair.Value)
            {
                string trimmedValue = headerValues.TrimStart().Replace("\r\n", String.Empty);
                innerBuilder.Append(separator).Append(trimmedValue);

                // Set this to a comma; this will only be used 
                //   if there are multiple values for one of the headers.
                separator = ',';
            }

            sb.Append(innerBuilder.ToString()).Append("\n");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Derives the raw representation of the message signature
    /// from the <see cref="HttpRequestMessage"/>.`
    /// </summary>
    /// <param name="request">the <see cref="HttpRequestMessage"/></param>
    /// <param name="storageAccountName">The name of the storage account to use.</param>
    /// <param name="eTag">entity tag for Web cache validation</param>
    /// <param name="md5">The MD5 (message-digest algorithm) hash</param>
    /// <returns></returns>
    /// <remarks>
    /// See https://github.com/Azure-Samples/storage-dotnet-rest-api-with-auth/tree/master
    ///
    /// See https://docs.microsoft.com/en-us/rest/api/storageservices/authorize-requests-to-azure-storage
    ///
    /// See https://docs.microsoft.com/en-us/rest/api/storageservices/authorize-with-shared-key
    /// </remarks>
    public static byte[] ToAzureStorageSignature(this HttpRequestMessage? request, string? storageAccountName,
        string? eTag, string? md5)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Content == null)
            throw new NullReferenceException($"{nameof(request)}.{nameof(request.Content)}");
        if (string.IsNullOrWhiteSpace(eTag)) eTag = string.Empty;
        if (string.IsNullOrWhiteSpace(md5)) md5 = string.Empty;

        var contentLength = string.Empty;

        HttpMethod method = request.Method;

        if (method == HttpMethod.Put)
        {
            try
            {
                contentLength = request.Content.Headers.ContentLength.ToString();
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(
                    $"The expected Content Headers are not here. Was {nameof(HttpRequestMessage)}.{nameof(HttpRequestMessage.Content)} set?",
                    ex);
            }
        }

        string canonicalizedHeaders = request.ToAzureStorageCanonicalizedHeaders();
        string location = request.RequestUri.ToAzureStorageCanonicalizedResourceLocation(storageAccountName);

        var messageSignature =
            $"{method}\n\n\n{contentLength}\n{md5}\n\n\n\n{eTag}\n\n\n\n{canonicalizedHeaders}{location}";

        byte[] signatureBytes = Encoding.UTF8.GetBytes(messageSignature);

        return signatureBytes;
    }

    /// <summary>
    /// Returns <see cref="HttpRequestMessage"/>
    /// with conventional headers for <see cref="ByteArrayContent"/>
    /// for Azure Storage.
    /// </summary>
    /// <param name="request">the <see cref="HttpRequestMessage"/></param>
    /// <param name="blobName">the Azure Storage Blob name</param>
    /// <param name="content">the Azure Storage Blob content</param>
    /// <returns></returns>
    public static HttpRequestMessage WithAzureStorageBlockBlobContent(this HttpRequestMessage? request,
        string? blobName, string? content)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(blobName);
        ArgumentNullException.ThrowIfNull(content);

        byte[] bytes = Encoding.UTF8.GetBytes(content);

        request.Content = new ByteArrayContent(bytes);

        request.Headers.Add("x-ms-blob-content-disposition", $@"attachment; filename=""{blobName}""");
        request.Headers.Add("x-ms-blob-type", "BlockBlob");
        request.Headers.Add("x-ms-meta-m1", "v1");
        request.Headers.Add("x-ms-meta-m2", "v2");

        return request;
    }

    /// <summary>
    /// Returns <see cref="HttpRequestMessage"/>
    /// with conventional headers for Azure Storage.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="requestMoment"></param>
    /// <param name="serviceVersion"></param>
    /// <param name="storageAccountName"></param>
    /// <param name="storageAccountKey"></param>
    /// <returns></returns>
    public static HttpRequestMessage WithAzureStorageHeaders(this HttpRequestMessage? request,
        DateTime requestMoment, string? serviceVersion, string? storageAccountName, string? storageAccountKey)
    {
        return request.WithAzureStorageHeaders(
            requestMoment,
            serviceVersion,
            storageAccountName,
            storageAccountKey,
            eTag: null,
            md5: null
        );
    }

    /// <summary>
    /// Returns <see cref="HttpRequestMessage"/> with the minimum headers
    /// required for Azure Storage.
    /// </summary>
    /// <param name="request">the <see cref="HttpRequestMessage"/></param>
    /// <param name="requestMoment">the moment of the request</param>
    /// <param name="serviceVersion"></param>
    /// <param name="storageAccountName">the Azure Storage account name</param>
    /// <param name="storageAccountKey">the Azure Storage account shared key</param>
    /// <param name="eTag">entity tag for Web cache validation</param>
    /// <param name="md5">The MD5 (message-digest algorithm) hash</param>
    /// <returns></returns>
    /// <remarks>
    /// See https://github.com/Azure-Samples/storage-dotnet-rest-api-with-auth/tree/master
    ///
    /// See https://docs.microsoft.com/en-us/rest/api/storageservices/authorize-requests-to-azure-storage
    ///
    /// See https://docs.microsoft.com/en-us/rest/api/storageservices/authorize-with-shared-key
    ///
    /// Provide the md5 and it will check and make sure it matches the requested blob's md5. If it doesn't match, it won't return a value.
    /// 
    /// Provide an eTag, and it will only make changes to a blob if the current eTag matches, to ensure you don't overwrite someone else's changes.
    /// </remarks>
    public static HttpRequestMessage WithAzureStorageHeaders(this HttpRequestMessage? request,
        DateTime requestMoment, string? serviceVersion, string? storageAccountName, string? storageAccountKey,
        string? eTag, string? md5)
    {
        ArgumentNullException.ThrowIfNull(request);

        request.Headers.Add("x-ms-date", requestMoment.ToString("R", CultureInfo.InvariantCulture));
        request.Headers.Add("x-ms-version", serviceVersion);

        request.Headers.Authorization =
            request.ToAzureStorageAuthorizationHeader(
                storageAccountName,
                storageAccountKey,
                eTag, md5);

        return request;
    }
}
