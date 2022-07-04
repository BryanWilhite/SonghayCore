namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="HttpRequestMessage"/>
    /// </summary>
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Gets a <see cref="string"/> from the derived <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="request">The request.</param>
        public static async Task<string> GetContentAsync(this HttpRequestMessage request)
        {
            return await request.GetContentAsync(responseMessageAction: null, optionalClientGetter: null);
        }

        /// <summary>
        /// Gets a <see cref="string" /> from the derived <see cref="HttpResponseMessage" />.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="responseMessageAction">The response message action.</param>
        /// <returns></returns>
        public static async Task<string> GetContentAsync(this HttpRequestMessage request, Action<HttpResponseMessage> responseMessageAction)
        {
            return await request.GetContentAsync(responseMessageAction, optionalClientGetter: null);
        }

        /// <summary>
        /// Gets a <see cref="string" /> from the derived <see cref="HttpResponseMessage" />.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="responseMessageAction">The response message action.</param>
        /// <param name="optionalClientGetter">The optional client getter.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">request</exception>
        public static async Task<string> GetContentAsync(this HttpRequestMessage request,
            Action<HttpResponseMessage>? responseMessageAction,
            Func<HttpClient>? optionalClientGetter)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var client = (optionalClientGetter == null) ? GetHttpClient() : optionalClientGetter.Invoke();

            string? content = null;

            using var response = await client
                .SendAsync(request)
                .ConfigureAwait(continueOnCapturedContext: false);

            responseMessageAction?.Invoke(response);
            content = await response.Content
                .ReadAsStringAsync()
                .ConfigureAwait(continueOnCapturedContext: false);

            return content;

        }

        /// <summary>
        /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
        /// </summary>
        /// <param name="request"></param>
        public static async Task<HttpResponseMessage> SendAsync(this HttpRequestMessage? request)
        {
            return await request.SendAsync(requestMessageAction: null,
                optionalClientGetter: null,
                HttpCompletionOption.ResponseContentRead);
        }

        /// <summary>
        /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage?)" />
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="requestMessageAction">The request message action.</param>
        public static async Task<HttpResponseMessage> SendAsync(this HttpRequestMessage? request, Action<HttpRequestMessage?> requestMessageAction)
        {
            return await request.SendAsync(requestMessageAction,
                optionalClientGetter: null,
                HttpCompletionOption.ResponseContentRead);
        }

        /// <summary>
        /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage?)" />
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="requestMessageAction">The request message action.</param>
        /// <param name="optionalClientGetter">The optional client getter.</param>
        /// <param name="completionOption"> the <see cref="HttpCompletionOption"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// uri
        /// or
        /// mediaType
        /// </exception>
        public static async Task<HttpResponseMessage> SendAsync(this HttpRequestMessage? request,
            Action<HttpRequestMessage>? requestMessageAction,
            Func<HttpClient>? optionalClientGetter,
            HttpCompletionOption completionOption)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            requestMessageAction?.Invoke(request);

            var client = (optionalClientGetter == null) ? GetHttpClient() : optionalClientGetter.Invoke();

            var response = await client
                .SendAsync(request, completionOption)
                .ConfigureAwait(continueOnCapturedContext: false);

            return response;
        }

        static HttpClient GetHttpClient() => HttpClientLazy.Value;

        static readonly Lazy<HttpClient> HttpClientLazy =
            new(() => new HttpClient(), LazyThreadSafetyMode.PublicationOnly);
    }
}
