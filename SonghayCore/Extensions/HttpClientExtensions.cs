﻿namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="HttpClient"/>
/// </summary>
public static class HttpClientExtensions
{
    /// <summary>
    /// Sends a <see cref="HttpMethod.Delete"/>
    /// <see cref="HttpRequestMessage"/>.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="uri">The URI.</param>
    /// <param name="requestMessageAction">The request message action.</param>
    public static async Task<HttpResponseMessage?> DeleteAsync(this HttpClient? client, Uri? uri,
        Action<HttpRequestMessage>? requestMessageAction) =>
        await client.SendAsync(uri, HttpMethod.Delete, requestMessageAction);

    /// <summary>
    /// Downloads resource at URI to the specified path.
    /// </summary>
    /// <param name="client">The request.</param>
    /// <param name="uri">The URI.</param>
    /// <param name="path">The path.</param>
    /// <param name="requestMessageAction">The request message action.</param>
    public static async Task DownloadToFileAsync(this HttpClient? client, Uri? uri, string path,
        Action<HttpRequestMessage>? requestMessageAction)
    {
        if (client == null) return;

        var buffer = new byte[32768];
        int bytesRead;

        using var request = new HttpRequestMessage(HttpMethod.Get, uri);

        requestMessageAction?.Invoke(request);

        using var response = await client
            .SendAsync(request)
            .ConfigureAwait(continueOnCapturedContext: false);

        await using var stream = await response.Content
            .ReadAsStreamAsync()
            .ConfigureAwait(continueOnCapturedContext: false);

        await using var fs = File.Create(path);

        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            fs.Write(buffer, 0, bytesRead);
        }
    }

    /// <summary>
    /// Downloads resource at URI to the specified path.
    /// </summary>
    /// <param name="client">The request.</param>
    /// <param name="uri">The URI.</param>
    /// <param name="path">The path.</param>
    public static async Task DownloadToFileAsync(this HttpClient? client, Uri? uri, string path) =>
        await client.DownloadToFileAsync(uri, path, requestMessageAction: null);

    /// <summary>
    /// Downloads resource at URI to <see cref="string" />.
    /// </summary>
    /// <param name="client">The request.</param>
    /// <param name="uri">The URI.</param>
    public static async Task<string?> DownloadToStringAsync(this HttpClient? client, Uri? uri)
    {
        if (client == null) return null;

        var content = await client
            .GetStringAsync(uri)
            .ConfigureAwait(continueOnCapturedContext: false);

        return content;
    }

    /// <summary>
    /// Downloads resource at URI to <see cref="string" />.
    /// </summary>
    /// <param name="client">The request.</param>
    /// <param name="uri">The URI.</param>
    /// <param name="requestMessageAction">The request message action.</param>
    public static async Task<string?> DownloadToStringAsync(this HttpClient? client, Uri? uri,
        Action<HttpRequestMessage>? requestMessageAction)
    {
        if (client == null) return null;

        using var request = new HttpRequestMessage(HttpMethod.Get, uri);

        requestMessageAction?.Invoke(request);

        string? content;

        using var response = await client
            .SendAsync(request)
            .ConfigureAwait(continueOnCapturedContext: false);

        content = await response.Content
            .ReadAsStringAsync()
            .ConfigureAwait(continueOnCapturedContext: false);

        return content;
    }

    /// <summary>
    /// Sends a <see cref="HttpMethod.Get"/>
    /// <see cref="HttpRequestMessage"/>.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="uri">The URI.</param>
    /// <param name="requestMessageAction">The request message action.</param>
    public static async Task<HttpResponseMessage?> GetAsync(this HttpClient? client, Uri? uri,
        Action<HttpRequestMessage>? requestMessageAction) =>
        await client.SendAsync(uri, HttpMethod.Get, requestMessageAction);

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// with the specified <see cref="MimeTypes.ApplicationFormUrlEncoded" />
    /// request body using <see cref="HttpMethod.Post" />
    /// and <see cref="Encoding.UTF8" />.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="uri">The URI.</param>
    /// <param name="formData">The form data.</param>
    public static async Task<HttpResponseMessage?> PostFormAsync(this HttpClient? client, Uri? uri,
        Hashtable formData) => await client.PostFormAsync(uri, formData, requestMessageAction: null);

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// with the specified <see cref="MimeTypes.ApplicationFormUrlEncoded" />
    /// request body using <see cref="HttpMethod.Post" />
    /// and <see cref="Encoding.UTF8" />.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="uri">The URI.</param>
    /// <param name="formData">The form data.</param>
    /// <param name="requestMessageAction">The request message action.</param>
    public static async Task<HttpResponseMessage?> PostFormAsync(this HttpClient? client, Uri? uri,
        Hashtable formData, Action<HttpRequestMessage>? requestMessageAction)
    {
        string GetPostData(Hashtable postData)
        {
            var sb = new StringBuilder();
            var s = sb.ToString();

            foreach (DictionaryEntry entry in postData)
            {
                s = (string.IsNullOrWhiteSpace(s))
                    ? string.Format(CultureInfo.InvariantCulture, "{0}={1}", entry.Key, entry.Value)
                    : string.Format(CultureInfo.InvariantCulture, "&{0}={1}", entry.Key, entry.Value);
                sb.Append(s);
            }

            return sb.ToString();
        }

        var postParams = GetPostData(formData);

        return await client.SendBodyAsync(uri, HttpMethod.Post, postParams, Encoding.UTF8,
            MimeTypes.ApplicationFormUrlEncoded, requestMessageAction: null);
    }

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// with the specified <see cref="MimeTypes.ApplicationJson"/>
    /// request body using <see cref="HttpMethod.Post" />
    /// and <see cref="Encoding.UTF8"/>.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="uri">The URI.</param>
    /// <param name="requestBody">The request body.</param>
    public static async Task<HttpResponseMessage?>
        PostJsonAsync(this HttpClient? client, Uri? uri, string requestBody) => await client.SendBodyAsync(uri,
        HttpMethod.Post, requestBody, Encoding.UTF8, MimeTypes.ApplicationJson, requestMessageAction: null);

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// with the specified <see cref="MimeTypes.ApplicationJson" />
    /// request body using <see cref="HttpMethod.Post" />
    /// and <see cref="Encoding.UTF8" />.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="uri">The URI.</param>
    /// <param name="requestBody">The request body.</param>
    /// <param name="requestMessageAction">The request message action.</param>
    public static async Task<HttpResponseMessage?> PostJsonAsync(this HttpClient? client, Uri? uri,
        string requestBody, Action<HttpRequestMessage>? requestMessageAction) => await client.SendBodyAsync(uri,
        HttpMethod.Post, requestBody, Encoding.UTF8, MimeTypes.ApplicationJson, requestMessageAction);

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// with the specified <see cref="MimeTypes.ApplicationXml"/>
    /// request body using <see cref="HttpMethod.Post" />
    /// and <see cref="Encoding.UTF8"/>.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="uri">The URI.</param>
    /// <param name="requestBody">The request body.</param>
    public static async Task<HttpResponseMessage?>
        PostXmlAsync(this HttpClient? client, Uri? uri, string requestBody) => await client.SendBodyAsync(uri,
        HttpMethod.Post, requestBody, Encoding.UTF8, MimeTypes.ApplicationXml, requestMessageAction: null);

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// with the specified <see cref="MimeTypes.ApplicationXml" />
    /// request body using <see cref="HttpMethod.Post" />
    /// and <see cref="Encoding.UTF8" />.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="uri">The URI.</param>
    /// <param name="requestBody">The request body.</param>
    /// <param name="requestMessageAction">The request message action.</param>
    public static async Task<HttpResponseMessage?> PostXmlAsync(this HttpClient? client, Uri? uri,
        string requestBody, Action<HttpRequestMessage>? requestMessageAction) => await client.SendBodyAsync(uri,
        HttpMethod.Post, requestBody, Encoding.UTF8, MimeTypes.ApplicationXml, requestMessageAction);

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// with the specified <see cref="MimeTypes.ApplicationJson"/>
    /// request body using <see cref="HttpMethod.Put" />
    /// and <see cref="Encoding.UTF8"/>.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="uri">The URI.</param>
    /// <param name="requestBody">The request body.</param>
    public static async Task<HttpResponseMessage?>
        PutJsonAsync(this HttpClient? client, Uri? uri, string requestBody) => await client.SendBodyAsync(uri,
        HttpMethod.Put, requestBody, Encoding.UTF8, MimeTypes.ApplicationJson, requestMessageAction: null);

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// with the specified <see cref="MimeTypes.ApplicationJson" />
    /// request body using <see cref="HttpMethod.Put" />
    /// and <see cref="Encoding.UTF8" />.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="uri">The URI.</param>
    /// <param name="requestBody">The request body.</param>
    /// <param name="requestMessageAction">The request message action.</param>
    public static async Task<HttpResponseMessage?> PutJsonAsync(this HttpClient? client, Uri? uri,
        string requestBody, Action<HttpRequestMessage>? requestMessageAction) => await client.SendBodyAsync(uri,
        HttpMethod.Put, requestBody, Encoding.UTF8, MimeTypes.ApplicationJson, requestMessageAction);

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// with the specified <see cref="MimeTypes.ApplicationXml"/>
    /// request body using <see cref="HttpMethod.Put" />
    /// and <see cref="Encoding.UTF8"/>.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="uri">The URI.</param>
    /// <param name="requestBody">The request body.</param>
    public static async Task<HttpResponseMessage?>
        PutXmlAsync(this HttpClient? client, Uri? uri, string requestBody) => await client.SendBodyAsync(uri,
        HttpMethod.Put, requestBody, Encoding.UTF8, MimeTypes.ApplicationXml, requestMessageAction: null);

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// with the specified <see cref="MimeTypes.ApplicationXml" />
    /// request body using <see cref="HttpMethod.Put" />
    /// and <see cref="Encoding.UTF8" />.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="uri">The URI.</param>
    /// <param name="requestBody">The request body.</param>
    /// <param name="requestMessageAction">The request message action.</param>
    public static async Task<HttpResponseMessage?> PutXmlAsync(this HttpClient? client, Uri? uri,
        string requestBody, Action<HttpRequestMessage>? requestMessageAction) => await client.SendBodyAsync(uri,
        HttpMethod.Put, requestBody, Encoding.UTF8, MimeTypes.ApplicationXml, requestMessageAction);

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="uri">The URI.</param>
    /// <param name="method">The method.</param>
    /// <param name="requestMessageAction">The request message action.</param>
    public static async Task<HttpResponseMessage?> SendAsync(this HttpClient? client, Uri? uri, HttpMethod method,
        Action<HttpRequestMessage>? requestMessageAction)
    {
        if (client == null) return null;
        ArgumentNullException.ThrowIfNull(uri);

        using var request = new HttpRequestMessage(method, uri);

        requestMessageAction?.Invoke(request);

        var response = await client
            .SendAsync(request)
            .ConfigureAwait(continueOnCapturedContext: false);

        return response;
    }

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// with the specified request body.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="uri">The URI.</param>
    /// <param name="method">The method.</param>
    /// <param name="requestBody">The request body.</param>
    /// <param name="encoding">The encoding.</param>
    /// <param name="mediaType">Type of the media.</param>
    /// <param name="requestMessageAction">The request message action.</param>
    public static async Task<HttpResponseMessage?> SendBodyAsync(this HttpClient? client, Uri? uri, HttpMethod method,
        string? requestBody, Encoding encoding, string? mediaType, Action<HttpRequestMessage>? requestMessageAction)
    {
        if (client == null) return null;

        ArgumentNullException.ThrowIfNull(uri);
        requestBody.ThrowWhenNullOrWhiteSpace();
        mediaType.ThrowWhenNullOrWhiteSpace();

        using var request = new HttpRequestMessage(method, uri)
        {
            Content = new StringContent(requestBody, encoding, mediaType)
        };

        requestMessageAction?.Invoke(request);

        var response = await client
            .SendAsync(request)
            .ConfigureAwait(continueOnCapturedContext: false);

        return response;
    }
}
