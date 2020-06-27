#if NETSTANDARD

using Songhay.Models;
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
        /// <param name="request">The request.</param>
        public static async Task<string> GetServerResponseAsync(this HttpRequestMessage request)
        {
            return await request.GetServerResponseAsync(responseMessageAction: null, optionalClientGetter: null);
        }

        /// <summary>
        /// Gets a <see cref="string" /> from the derived <see cref="HttpResponseMessage" />.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="responseMessageAction">The response message action.</param>
        /// <returns></returns>
        public static async Task<string> GetServerResponseAsync(this HttpRequestMessage request, Action<HttpResponseMessage> responseMessageAction)
        {
            return await request.GetServerResponseAsync(responseMessageAction, optionalClientGetter: null);
        }

        /// <summary>
        /// Gets a <see cref="string" /> from the derived <see cref="HttpResponseMessage" />.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="responseMessageAction">The response message action.</param>
        /// <param name="optionalClientGetter">The optional client getter.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">request</exception>
        public static async Task<string> GetServerResponseAsync(this HttpRequestMessage request,
            Action<HttpResponseMessage> responseMessageAction,
            Func<HttpClient> optionalClientGetter)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var client = (optionalClientGetter == null) ? GetHttpClient() : optionalClientGetter.Invoke();

            var response = await client.SendAsync(request);

            responseMessageAction?.Invoke(response);

            var content = await response.Content.ReadAsStringAsync();
            return content;

        }

        /// <summary>
        /// Gets any <see cref="Timeout"/> in <see cref="HttpRequestMessage.Properties"/>.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">request</exception>
        public static TimeSpan? GetTimeout(this HttpRequestMessage request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.Properties.TryGetValue(TIMEOUT_PROPERTY_KEY, out var value) && value is TimeSpan timeout)
            {
                return timeout;
            }

            return null;
        }

        /// <summary>
        /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
        /// </summary>
        /// <param name="request"></param>
        public static async Task<HttpResponseMessage> SendAsync(this HttpRequestMessage request)
        {
            return await request.SendAsync(requestMessageAction: null, optionalClientGetter: null);
        }

        /// <summary>
        /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="requestMessageAction">The request message action.</param>
        public static async Task<HttpResponseMessage> SendAsync(this HttpRequestMessage request, Action<HttpRequestMessage> requestMessageAction)
        {
            return await request.SendAsync(requestMessageAction, optionalClientGetter: null);
        }

        /// <summary>
        /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="requestMessageAction">The request message action.</param>
        /// <param name="optionalClientGetter">The optional client getter.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">uri - The expected URI is not here.
        /// or
        /// mediaType - The expected request body media type is not here.</exception>
        public static async Task<HttpResponseMessage> SendAsync(this HttpRequestMessage request,
            Action<HttpRequestMessage> requestMessageAction,
            Func<HttpClient> optionalClientGetter)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            requestMessageAction?.Invoke(request);

            var client = (optionalClientGetter == null) ? GetHttpClient() : optionalClientGetter.Invoke();

            var response = await client.SendAsync(request);

            return response;
        }

        /// <summary>
        /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
        /// with the specified request body, <see cref="Encoding.UTF8"/>
        /// and <see cref="MimeTypes.ApplicationJson"/>.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="requestBody">The request body.</param>
        public static async Task<HttpResponseMessage> SendBodyAsync(this HttpRequestMessage request, string requestBody)
        {
            return await request.SendBodyAsync(requestBody, Encoding.UTF8, MimeTypes.ApplicationJson);
        }

        /// <summary>
        /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
        /// with the specified request body.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="requestBody">The request body.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="mediaType">Type of the media.</param>
        public static async Task<HttpResponseMessage> SendBodyAsync(this HttpRequestMessage request, string requestBody, Encoding encoding, string mediaType)
        {
            return await request.SendBodyAsync(requestBody, encoding, mediaType, requestMessageAction: null, optionalClientGetter: null);
        }

        /// <summary>
        /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
        /// with the specified request body.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="requestBody">The request body.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="mediaType">Type of the media.</param>
        /// <param name="requestMessageAction">The request message action.</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> SendBodyAsync(this HttpRequestMessage request, string requestBody, Encoding encoding, string mediaType, Action<HttpRequestMessage> requestMessageAction)
        {
            return await request.SendBodyAsync(requestBody, encoding, mediaType, requestMessageAction, optionalClientGetter: null);
        }

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
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">requestBody - The expected request body is not here.</exception>
        public static async Task<HttpResponseMessage> SendBodyAsync(this HttpRequestMessage request,
            string requestBody, Encoding encoding, string mediaType,
            Action<HttpRequestMessage> requestMessageAction,
            Func<HttpClient> optionalClientGetter)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(requestBody)) throw new ArgumentNullException(nameof(requestBody), "The expected request body is not here.");
            if (string.IsNullOrWhiteSpace(mediaType)) throw new ArgumentNullException(nameof(mediaType), "The expected request body media type is not here.");

            request.Content = new StringContent(requestBody, encoding, mediaType);

            requestMessageAction?.Invoke(request);

            var client = (optionalClientGetter == null) ? GetHttpClient() : optionalClientGetter.Invoke();

            var response = await client.SendAsync(request);
            return response;
        }

        /// <summary>
        /// Sets the <see cref="Timeout" /> in <see cref="HttpRequestMessage.Properties" />.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="timeout">The timeout.</param>
        /// <exception cref="ArgumentNullException">request</exception>
        public static void SetTimeout(this HttpRequestMessage request, TimeSpan? timeout)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            request.Properties[TIMEOUT_PROPERTY_KEY] = timeout;
        }

        private const string TIMEOUT_PROPERTY_KEY = "RequestTimeout";

        private static HttpClient GetHttpClient() => HttpClientLazy.Value;

        private static readonly Lazy<HttpClient> HttpClientLazy =
            new Lazy<HttpClient>(() => new HttpClient(), LazyThreadSafetyMode.PublicationOnly);
    }
}

#endif
