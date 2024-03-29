﻿using System.Web;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="Uri"/>
/// </summary>
public static class UriExtensions
{
    /// <summary>
    /// Determines whether the <see cref="Uri" /> is a file.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <remarks>
    /// Recall that <see cref="Uri.IsFile" /> is another way
    /// of stating that <c>Uri.Schema == Uri.UriSchemeFile</c>
    /// and that <see cref="System.IO"/> members can process URIs.
    /// 
    /// Also note that the only way to truly define a directory
    /// or folder is with a trailing forward/back slash.
    /// </remarks>
    public static bool IsProbablyAFile(this Uri? input) =>
        input != null && (input.IsFile || Path.HasExtension(input.OriginalString));

    /// <summary>
    /// This part of the signature string represents the storage account 
    ///   targeted by the request. Will also include any additional query parameters/values.
    /// For ListContainers, this will return something like this:
    ///   /storageaccountname/\ncomp:list
    /// </summary>
    /// <param name="uri">The URI of the storage service.</param>
    /// <param name="accountName">The storage account name.</param>
    /// <returns><see cref="string" /> representing the canonicalized resource.</returns>
    /// <remarks>
    /// See https://github.com/Azure-Samples/storage-dotnet-rest-api-with-auth/tree/master
    ///
    /// See https://docs.microsoft.com/en-us/rest/api/storageservices/authorize-requests-to-azure-storage
    ///
    /// See https://docs.microsoft.com/en-us/rest/api/storageservices/authorize-with-shared-key
    /// 
    /// See “Shared Key format for 2009-09-19 and later”
    /// [ https://docs.microsoft.com/en-us/rest/api/storageservices/authorize-with-shared-key#shared-key-format-for-2009-09-19-and-later ]
    /// </remarks>
    public static string ToAzureStorageCanonicalizedResourceLocation(this Uri? uri, string? accountName)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ArgumentNullException.ThrowIfNull(accountName);

        // The absolute path is "/" because for we're getting a list of containers.
        StringBuilder sb = new StringBuilder("/").Append(accountName).Append(uri.AbsolutePath);

        // Address.Query is the resource, such as "?comp=list".
        // This ends up with a NameValueCollection with 1 entry having key=comp, value=list.
        // It will have more entries if you have more query parameters.
        NameValueCollection values = HttpUtility.ParseQueryString(uri.Query);

        foreach (var item in values.AllKeys
                     .Where(k => k != null)
                     .Select(k => k!.ToLowerInvariant()).OrderBy(k => k))
        {
            sb.Append('\n').Append(item).Append(':').Append(values[item]);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Converts the <see cref="Uri" /> into a base URI.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>
    /// Returns a <see cref="string"/> like: <c>https://MyServer:8080/</c>
    /// </returns>
    public static string? ToBaseUri(this Uri? input)
    {
        if (input == null) return null;

        var baseLocation = $"{input.GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped)}/";

        return baseLocation;
    }

    /// <summary>
    /// Converts the <see cref="Uri" /> into its file name.
    /// </summary>
    /// <param name="input">The input.</param>
    public static string? ToFileName(this Uri? input) => Path.GetFileName(input?.LocalPath);

    /// <summary>
    /// Converts the specified <see cref="Uri" />
    /// to its ‘expanded’ version.
    /// </summary>
    /// <param name="expandableUri">The expandable <see cref="Uri"/>.</param>
    /// <remarks>
    /// This member will call itself recursively
    /// until <see cref="HttpResponseMessageExtensions.IsMovedOrRedirected"/> returns <c>true</c>
    /// or <see cref="System.Net.Http.Headers.HttpResponseHeaders.Location"/> is null.
    /// </remarks>
    public static async Task<Uri?> ToExpandedUriAsync(this Uri? expandableUri)
    {
        ArgumentNullException.ThrowIfNull(expandableUri);

        var message = new HttpRequestMessage(HttpMethod.Get, expandableUri);

        using var response = await message.SendAsync();

        if (response.Headers.Location == null)
        {
            return message.RequestUri;
        }

        if (response.IsMovedOrRedirected())
        {
            return response.Headers.Location;
        }

        var uri = await response.Headers.Location
            .ToExpandedUriAsync()
            .ConfigureAwait(continueOnCapturedContext: false);

        return uri;
    }

    /// <summary>
    /// Converts the specified <see cref="Uri" />
    /// to its ‘expanded’ version.
    /// </summary>
    /// <param name="expandableUri">The expandable <see cref="Uri"/>.</param>
    public static async Task<KeyValuePair<Uri?, Uri?>> ToExpandedUriPairAsync(this Uri? expandableUri)
    {
        var expandedUri = await expandableUri
            .ToExpandedUriAsync()
            .ConfigureAwait(continueOnCapturedContext: false);

        return new KeyValuePair<Uri?, Uri?>(expandableUri, expandedUri);
    }

    /// <summary>
    /// Converts the <see cref="Uri"/> into a relative URI from query.
    /// </summary>
    /// <param name="input">The input.</param>
    public static Uri? ToRelativeUriFromQuery(this Uri? input)
    {
        if (input == null) return null;

        var query = input.OriginalString.Split('?').Last();

        return string.IsNullOrWhiteSpace(query) ? null : new Uri(query, UriKind.Relative);
    }
}
