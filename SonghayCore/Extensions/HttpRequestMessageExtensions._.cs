namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="HttpRequestMessage"/>
/// </summary>
public static partial class HttpRequestMessageExtensions
{
    /// <summary>
    /// Gets a <see cref="string"/> from the derived <see cref="HttpResponseMessage"/>.
    /// </summary>
    /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
    /// <param name="clientGetter">The required client getter.</param>
    public static async Task<string> GetContentAsync(this HttpRequestMessage? request, Func<HttpClient> clientGetter) =>
        await request.GetContentAsync(clientGetter, responseMessageAction: null);

    /// <summary>
    /// Gets a <see cref="string" /> from the derived <see cref="HttpResponseMessage" />.
    /// </summary>
    /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
    /// <param name="clientGetter">The required client getter.</param>
    /// <param name="responseMessageAction">The response message action.</param>
    public static async Task<string> GetContentAsync(this HttpRequestMessage? request,
        Func<HttpClient> clientGetter, Action<HttpResponseMessage>? responseMessageAction)
    {
        ArgumentNullException.ThrowIfNull(request);

        var client = clientGetter.Invoke();

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
    /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
    /// <param name="clientGetter">The required client getter.</param>
    public static async Task<HttpResponseMessage> SendAsync(this HttpRequestMessage? request, Func<HttpClient> clientGetter) =>
        await request.SendAsync(clientGetter, HttpCompletionOption.ResponseContentRead,
            requestMessageAction: null);

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// </summary>
    /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
    /// <param name="clientGetter">The required client getter.</param>
    /// <param name="requestMessageAction">The request message action.</param>
    public static async Task<HttpResponseMessage> SendAsync(this HttpRequestMessage? request,
        Func<HttpClient> clientGetter, Action<HttpRequestMessage>? requestMessageAction) =>
            await request.SendAsync(clientGetter, HttpCompletionOption.ResponseContentRead, requestMessageAction);

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// </summary>
    /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
    /// <param name="clientGetter">The required client getter.</param>
    /// <param name="completionOption"> the <see cref="HttpCompletionOption"/>.</param>
    /// <param name="requestMessageAction">The request message action.</param>
    public static async Task<HttpResponseMessage> SendAsync(this HttpRequestMessage? request,
        Func<HttpClient> clientGetter,
        HttpCompletionOption completionOption,
        Action<HttpRequestMessage>? requestMessageAction)
    {
        ArgumentNullException.ThrowIfNull(request);

        requestMessageAction?.Invoke(request);

        var client = clientGetter.Invoke();

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
    /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
    /// <param name="requestBody">The request body.</param>
    /// <param name="clientGetter">The required client getter.</param>
    public static async Task<HttpResponseMessage> SendBodyAsync(this HttpRequestMessage? request,
        string? requestBody, Func<HttpClient> clientGetter) =>
            await request.SendBodyAsync(requestBody, Encoding.UTF8, MimeTypes.ApplicationJson, clientGetter);

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// with the specified request body.
    /// </summary>
    /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
    /// <param name="requestBody">The request body.</param>
    /// <param name="encoding">The encoding.</param>
    /// <param name="mediaType">Type of the media.</param>
    /// <param name="clientGetter">The required client getter.</param>
    public static async Task<HttpResponseMessage> SendBodyAsync(this HttpRequestMessage? request,
        string? requestBody, Encoding encoding, string? mediaType, Func<HttpClient> clientGetter) =>
            await request.SendBodyAsync(requestBody, encoding, mediaType, clientGetter, requestMessageAction: null);

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// with the specified request body.
    /// </summary>
    /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
    /// <param name="requestBody">The request body.</param>
    /// <param name="encoding">The encoding.</param>
    /// <param name="mediaType">Type of the media.</param>
    /// <param name="clientGetter">The required client getter.</param>
    /// <param name="requestMessageAction">The request message action.</param>
    public static async Task<HttpResponseMessage> SendBodyAsync(this HttpRequestMessage? request,
        string? requestBody, Encoding? encoding, string? mediaType, Func<HttpClient> clientGetter,
        Action<HttpRequestMessage>? requestMessageAction)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(clientGetter);
        requestBody.ThrowWhenNullOrWhiteSpace();
        mediaType.ThrowWhenNullOrWhiteSpace();

        request.Content = new StringContent(requestBody, encoding, mediaType);

        requestMessageAction?.Invoke(request);

        var client = clientGetter.Invoke();

        var response = await client
            .SendAsync(request)
            .ConfigureAwait(continueOnCapturedContext: false);

        return response;
    }
}
