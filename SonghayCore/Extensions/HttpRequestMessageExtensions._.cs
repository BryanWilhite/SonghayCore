namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="HttpRequestMessage"/>
/// </summary>
public static partial class HttpRequestMessageExtensions
{
    /// <summary>
    /// Gets a <see cref="string"/> from the derived <see cref="HttpResponseMessage"/>.
    /// </summary>
    /// <param name="request">The request.</param>
    public static async Task<string> GetContentAsync(this HttpRequestMessage? request) =>
        await request.GetContentAsync(responseMessageAction: null, optionalClientGetter: null);

    /// <summary>
    /// Gets a <see cref="string" /> from the derived <see cref="HttpResponseMessage" />.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="responseMessageAction">The response message action.</param>
    public static async Task<string> GetContentAsync(this HttpRequestMessage? request,
        Action<HttpResponseMessage>? responseMessageAction) =>
        await request.GetContentAsync(responseMessageAction, optionalClientGetter: null);

    /// <summary>
    /// Gets a <see cref="string" /> from the derived <see cref="HttpResponseMessage" />.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="responseMessageAction">The response message action.</param>
    /// <param name="optionalClientGetter">The optional client getter.</param>
    public static async Task<string> GetContentAsync(this HttpRequestMessage? request,
        Action<HttpResponseMessage>? responseMessageAction,
        Func<HttpClient>? optionalClientGetter)
    {
        ArgumentNullException.ThrowIfNull(request);

        var client = (optionalClientGetter == null) ? GetHttpClient() : optionalClientGetter.Invoke();

        using var response = await client
            .SendAsync(request)
            .ConfigureAwait(continueOnCapturedContext: false);

        responseMessageAction?.Invoke(response);

        var content = await response.Content
            .ReadAsStringAsync()
            .ConfigureAwait(continueOnCapturedContext: false);

        return content;
    }

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// </summary>
    /// <param name="request"></param>
    public static async Task<HttpResponseMessage> SendAsync(this HttpRequestMessage? request) =>
        await request.SendAsync(requestMessageAction: null,
            optionalClientGetter: null,
            HttpCompletionOption.ResponseContentRead);

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="requestMessageAction">The request message action.</param>
    public static async Task<HttpResponseMessage> SendAsync(this HttpRequestMessage? request,
        Action<HttpRequestMessage>? requestMessageAction) =>
        await request.SendAsync(requestMessageAction,
            optionalClientGetter: null,
            HttpCompletionOption.ResponseContentRead);

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="requestMessageAction">The request message action.</param>
    /// <param name="optionalClientGetter">The optional client getter.</param>
    /// <param name="completionOption"> the <see cref="HttpCompletionOption"/>.</param>
    public static async Task<HttpResponseMessage> SendAsync(this HttpRequestMessage? request,
        Action<HttpRequestMessage>? requestMessageAction,
        Func<HttpClient>? optionalClientGetter,
        HttpCompletionOption completionOption)
    {
        ArgumentNullException.ThrowIfNull(request);

        requestMessageAction?.Invoke(request);

        var client = (optionalClientGetter == null) ? GetHttpClient() : optionalClientGetter.Invoke();

        var response = await client
            .SendAsync(request, completionOption)
            .ConfigureAwait(continueOnCapturedContext: false);

        return response;
    }

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// with the specified request body, <see cref="Encoding.UTF8"/>
    /// and <see cref="MimeTypes.ApplicationJson"/>.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="requestBody">The request body.</param>
    public static async Task<HttpResponseMessage> SendBodyAsync(this HttpRequestMessage? request,
        string? requestBody) => await request.SendBodyAsync(requestBody, Encoding.UTF8, MimeTypes.ApplicationJson);

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// with the specified request body.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="requestBody">The request body.</param>
    /// <param name="encoding">The encoding.</param>
    /// <param name="mediaType">Type of the media.</param>
    public static async Task<HttpResponseMessage> SendBodyAsync(this HttpRequestMessage? request,
        string? requestBody, Encoding encoding, string mediaType) => await request.SendBodyAsync(requestBody,
        encoding, mediaType, requestMessageAction: null, optionalClientGetter: null);

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// with the specified request body.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="requestBody">The request body.</param>
    /// <param name="encoding">The encoding.</param>
    /// <param name="mediaType">Type of the media.</param>
    /// <param name="requestMessageAction">The request message action.</param>
    public static async Task<HttpResponseMessage> SendBodyAsync(this HttpRequestMessage? request,
        string? requestBody, Encoding encoding, string mediaType,
        Action<HttpRequestMessage> requestMessageAction) => await request.SendBodyAsync(requestBody, encoding,
        mediaType, requestMessageAction, optionalClientGetter: null);

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// with the specified request body.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="requestBody">The request body.</param>
    /// <param name="encoding">The encoding.</param>
    /// <param name="mediaType">Type of the media.</param>
    /// <param name="requestMessageAction">The request message action.</param>
    /// <param name="optionalClientGetter">The optional client getter.</param>
    public static async Task<HttpResponseMessage> SendBodyAsync(this HttpRequestMessage? request,
        string? requestBody, Encoding? encoding, string? mediaType,
        Action<HttpRequestMessage>? requestMessageAction,
        Func<HttpClient>? optionalClientGetter)
    {
        ArgumentNullException.ThrowIfNull(request);
        requestBody.ThrowWhenNullOrWhiteSpace();
        mediaType.ThrowWhenNullOrWhiteSpace();

        request.Content = new StringContent(requestBody, encoding, mediaType);

        requestMessageAction?.Invoke(request);

        var client = (optionalClientGetter == null) ? GetHttpClient() : optionalClientGetter.Invoke();

        var response = await client
            .SendAsync(request)
            .ConfigureAwait(continueOnCapturedContext: false);

        return response;
    }

    static HttpClient GetHttpClient() => HttpClientLazy.Value;

    static readonly Lazy<HttpClient> HttpClientLazy = new(() => new HttpClient(), LazyThreadSafetyMode.PublicationOnly);
}
