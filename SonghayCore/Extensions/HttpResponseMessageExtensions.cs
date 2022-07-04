using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="HttpResponseMessage"/>.
    /// </summary>
    public static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// Downloads <see cref="HttpResponseMessage.Content"/>
        /// from <see cref="byte"/> array to file.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="fileInfo">The file information.</param>
        /// <exception cref="NullReferenceException">The expected {nameof(FileSystemInfo)} is not here.</exception>
        public static async Task DownloadByteArrayToFileAsync(this HttpResponseMessage response, FileSystemInfo fileInfo)
        {
            if (fileInfo == null) throw new NullReferenceException($"The expected {nameof(FileSystemInfo)} is not here.");

            await response.DownloadByteArrayToFileAsync(fileInfo.FullName);
        }

        /// <summary>
        /// Downloads <see cref="HttpResponseMessage.Content"/>
        /// from <see cref="byte"/> array to file.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="target">The target.</param>
        public static async Task DownloadByteArrayToFileAsync(this HttpResponseMessage response, string target)
        {
            if (response == null) return;

            var data = await response.Content
                .ReadAsByteArrayAsync()
                .ConfigureAwait(continueOnCapturedContext: false);

            File.WriteAllBytes(target, data);
        }

        /// <summary>
        /// Downloads <see cref="HttpResponseMessage.Content"/>
        /// from <see cref="byte"/> array to file.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="fileInfo">The file information.</param>
        /// <exception cref="NullReferenceException">The expected {nameof(FileSystemInfo)} is not here.</exception>
        public static async Task DownloadStringToFileAsync(this HttpResponseMessage response, FileSystemInfo fileInfo)
        {
            if (fileInfo == null) throw new ArgumentNullException(nameof(fileInfo));

            await response.DownloadStringToFileAsync(fileInfo.FullName);
        }

        /// <summary>
        /// Downloads <see cref="HttpResponseMessage.Content"/>
        /// from <see cref="byte"/> array to file.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="target">The target.</param>
        public static async Task DownloadStringToFileAsync(this HttpResponseMessage response, string target)
        {
            if (response == null) return;

            var data = await response.Content
                .ReadAsStringAsync()
                .ConfigureAwait(continueOnCapturedContext: false);

            File.WriteAllText(target, data);
        }

        /// <summary>
        /// Returns <c>true</c> when <see cref="HttpResponseMessage"/>
        /// is <see cref="HttpStatusCode.Moved"/>, <see cref="HttpStatusCode.MovedPermanently"/>
        /// or <see cref="HttpStatusCode.Redirect"/>.
        /// </summary>
        /// <param name="response">The response.</param>
        public static bool IsMovedOrRedirected(this HttpResponseMessage response) =>
            response?.StatusCode == HttpStatusCode.Moved ||
            response?.StatusCode == HttpStatusCode.MovedPermanently ||
            response?.StatusCode == HttpStatusCode.Redirect;

        /// <summary>
        /// Serializes the <see cref="HttpResponseMessage"/>
        /// to the specified <c>TInstance</c>
        /// </summary>
        /// <param name="response">The response.</param>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <returns></returns>
        /// <remarks>
        /// This method uses the Microsoft API to deserialize.
        /// </remarks>
        public static async Task<TInstance> StreamToInstanceAsync<TInstance>(this HttpResponseMessage response)
        {
            return await response.StreamToInstanceAsync<TInstance>(options: null);
        }

        /// <summary>
        /// Serializes the <see cref="HttpResponseMessage"/>
        /// to the specified <c>TInstance</c>
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="options">The <see cref="JsonSerializerOptions"/></param>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <returns></returns>
        /// <remarks>
        /// This method uses the Microsoft API to deserialize.
        /// </remarks>
        public static async Task<TInstance> StreamToInstanceAsync<TInstance>(this HttpResponseMessage response, JsonSerializerOptions options)
        {
            if (response == null) return await Task
                .FromResult(default(TInstance))
                .ConfigureAwait(continueOnCapturedContext: false);

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                if (stream == null || stream.CanRead == false) return await Task
                    .FromResult(default(TInstance))
                    .ConfigureAwait(continueOnCapturedContext: false);

                using (var streamReader = new StreamReader(stream))
                {
                    var instance = await JsonSerializer
                        .DeserializeAsync<TInstance>(stream, options);

                    return instance;
                }
            }
        }
    }
}
