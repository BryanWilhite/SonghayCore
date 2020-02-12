#if NETSTANDARD

using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
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
        public static async Task DownloadByteArrayToFile(this HttpResponseMessage response, FileSystemInfo fileInfo)
        {
            if (fileInfo == null) throw new NullReferenceException($"The expected {nameof(FileSystemInfo)} is not here.");

            await response.DownloadByteArrayToFile(fileInfo.FullName);
        }

        /// <summary>
        /// Downloads <see cref="HttpResponseMessage.Content"/>
        /// from <see cref="byte"/> array to file.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="target">The target.</param>
        public static async Task DownloadByteArrayToFile(this HttpResponseMessage response, string target)
        {
            if (response == null) return;
            var data = await response.Content.ReadAsByteArrayAsync();
            File.WriteAllBytes(target, data);
        }

        /// <summary>
        /// Downloads <see cref="HttpResponseMessage.Content"/>
        /// from <see cref="byte"/> array to file.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="fileInfo">The file information.</param>
        /// <exception cref="NullReferenceException">The expected {nameof(FileSystemInfo)} is not here.</exception>
        public static async Task DownloadStringToFile(this HttpResponseMessage response, FileSystemInfo fileInfo)
        {
            if (fileInfo == null) throw new NullReferenceException($"The expected {nameof(FileSystemInfo)} is not here.");

            await response.DownloadStringToFile(fileInfo.FullName);
        }

        /// <summary>
        /// Downloads <see cref="HttpResponseMessage.Content"/>
        /// from <see cref="byte"/> array to file.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="target">The target.</param>
        public static async Task DownloadStringToFile(this HttpResponseMessage response, string target)
        {
            if (response == null) return;
            var data = await response.Content.ReadAsStringAsync();
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
        /// Converts the specified <see cref="HttpResponseMessage"/>
        /// to <see cref="JContainer"/>
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static async Task<JContainer> ToJContainerAsync(this HttpResponseMessage response)
        {
            if (response == null) return await Task.FromResult(default(JContainer));

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content)) return null;

            return JToken.Parse(content) as JContainer;
        }
    }
}

#endif
