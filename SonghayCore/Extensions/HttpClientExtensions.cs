using Songhay.Models;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="HttpClient"/>
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Downloads resource at URI to the specified path.
        /// </summary>
        /// <param name="client">The request.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static async Task DownloadToFileAsync(this HttpClient client, Uri uri, string path)
        {
            if (client == null) return;

            var buffer = new byte[32768];
            var bytesRead = 0;
            var fileName = Path.GetFileName(path);

            client.DefaultRequestHeaders.Add("Content-Disposition", string.Format("attachment; filename={0}", fileName));

            var stream = await client.GetStreamAsync(uri);
            try
            {
                using (var fs = File.Create(path))
                {
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fs.Write(buffer, 0, bytesRead);
                    }
                }
            }
            finally
            {
                stream.Close();
            }
        }

        /// <summary>
        /// Downloads resource at URI to <see cref="string" />.
        /// </summary>
        /// <param name="client">The request.</param>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static async Task<string> DownloadToStringAsync(this HttpClient client, Uri uri)
        {
            if (client == null) return null;

            var response = await client.GetStringAsync(uri);

            return response;
        }

        /// <summary>
        /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
        /// with the specified <see cref="MimeTypes.ApplicationFormUrlEncoded" />
        /// request body using <see cref="HttpMethod.Post" />
        /// and <see cref="Encoding.UTF8"/>.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="formData">The form data.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">requestBody - The expected request body is not here.</exception>
        public static async Task<HttpResponseMessage> PostFormAsync(this HttpClient client, Uri uri, Hashtable formData)
        {
            string GetPostData(Hashtable postData)
            {
                var sb = new StringBuilder();
                string s = sb.ToString();

                foreach (DictionaryEntry entry in postData)
                {
                    s = (string.IsNullOrEmpty(s))
                        ? string.Format(CultureInfo.InvariantCulture, "{0}={1}", entry.Key, entry.Value)
                        : string.Format(CultureInfo.InvariantCulture, "&{0}={1}", entry.Key, entry.Value);
                    sb.Append(s);
                }

                return sb.ToString();
            }

            var postParams = GetPostData(formData);

            return await client.SendBodyAsync(uri, HttpMethod.Post, postParams, Encoding.UTF8, MimeTypes.ApplicationFormUrlEncoded);
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
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">requestBody - The expected request body is not here.</exception>
        public static async Task<HttpResponseMessage> PostJsonAsync(this HttpClient client, Uri uri, string requestBody)
        {
            return await client.SendBodyAsync(uri, HttpMethod.Post, requestBody, Encoding.UTF8, MimeTypes.ApplicationJson);
        }

        /// <summary>
        /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
        /// with the specified <see cref="MimeTypes.ApplicationXml"/>
        /// request body using <see cref="HttpMethod.Post" />
        /// and <see cref="Encoding.UTF8"/>.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="requestBody">The request body.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">requestBody - The expected request body is not here.</exception>
        public static async Task<HttpResponseMessage> PostXmlAsync(this HttpClient client, Uri uri, string requestBody)
        {
            return await client.SendBodyAsync(uri, HttpMethod.Post, requestBody, Encoding.UTF8, MimeTypes.ApplicationXml);
        }

        /// <summary>
        /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
        /// with the specified <see cref="MimeTypes.ApplicationJson"/>
        /// request body using <see cref="HttpMethod.Put" />
        /// and <see cref="Encoding.UTF8"/>.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="requestBody">The request body.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">requestBody - The expected request body is not here.</exception>
        public static async Task<HttpResponseMessage> PutJsonAsync(this HttpClient client, Uri uri, string requestBody)
        {
            return await client.SendBodyAsync(uri, HttpMethod.Put, requestBody, Encoding.UTF8, MimeTypes.ApplicationJson);
        }

        /// <summary>
        /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
        /// with the specified <see cref="MimeTypes.ApplicationXml"/>
        /// request body using <see cref="HttpMethod.Put" />
        /// and <see cref="Encoding.UTF8"/>.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="requestBody">The request body.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">requestBody - The expected request body is not here.</exception>
        public static async Task<HttpResponseMessage> PutXmlAsync(this HttpClient client, Uri uri, string requestBody)
        {
            return await client.SendBodyAsync(uri, HttpMethod.Put, requestBody, Encoding.UTF8, MimeTypes.ApplicationXml);
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
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">requestBody - The expected request body is not here.</exception>
        public static async Task<HttpResponseMessage> SendBodyAsync(this HttpClient client, Uri uri, HttpMethod method, string requestBody, Encoding encoding, string mediaType)
        {
            if (client == null) return null;
            if (uri == null) throw new ArgumentNullException(nameof(uri), "The expected URI is not here.");
            if (string.IsNullOrEmpty(requestBody)) throw new ArgumentNullException(nameof(requestBody), "The expected request body is not here.");
            if (string.IsNullOrEmpty(mediaType)) throw new ArgumentNullException(nameof(mediaType), "The expected request body media type is not here.");

            var request = new HttpRequestMessage(method, uri)
            {
                Content = new StringContent(requestBody, encoding, mediaType)
            };

            var response = await client.SendAsync(request);

            return response;
        }
    }
}
