#if NETSTANDARD

using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<string> GetServerResponseAsync(this HttpRequestMessage request)
        {
            if (request == null) throw new NullReferenceException($"The expected {nameof(HttpRequestMessage)} is not here.");

            var response = await GetHttpClient().SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            return content;

        }

        /// <summary>
        /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestMessageAction">The request message action.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">uri - The expected URI is not here.
        /// or
        /// mediaType - The expected request body media type is not here.</exception>
        public static async Task<HttpResponseMessage> SendAsync(this HttpRequestMessage request, Action<HttpRequestMessage> requestMessageAction)
        {
            if (request == null) throw new NullReferenceException($"The expected {nameof(HttpRequestMessage)} is not here.");

            requestMessageAction?.Invoke(request);

            var response = await GetHttpClient().SendAsync(request);
            return response;
        }

        /// <summary>
        /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
        /// with the specified request body.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestBody">The request body.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="mediaType">Type of the media.</param>
        /// <param name="requestMessageAction">The request message action.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">requestBody - The expected request body is not here.</exception>
        public static async Task<HttpResponseMessage> SendBodyAsync(this HttpRequestMessage request, string requestBody, Encoding encoding, string mediaType, Action<HttpRequestMessage> requestMessageAction)
        {
            if (request == null) throw new NullReferenceException($"The expected {nameof(HttpRequestMessage)} is not here.");
            if (string.IsNullOrEmpty(requestBody)) throw new ArgumentNullException(nameof(requestBody), "The expected request body is not here.");
            if (string.IsNullOrEmpty(mediaType)) throw new ArgumentNullException(nameof(mediaType), "The expected request body media type is not here.");

            request.Content = new StringContent(requestBody, encoding, mediaType);

            requestMessageAction?.Invoke(request);

            var response = await GetHttpClient().SendAsync(request);
            return response;
        }

        private static HttpClient GetHttpClient() => HttpClientLazy.Value;

        private static readonly Lazy<HttpClient> HttpClientLazy =
            new Lazy<HttpClient>(() => new HttpClient(), LazyThreadSafetyMode.PublicationOnly);
    }
}

#endif
