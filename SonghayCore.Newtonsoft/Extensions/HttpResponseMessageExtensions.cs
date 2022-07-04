using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
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
        /// Serializes the <see cref="HttpResponseMessage"/>
        /// to the specified <c>TInstance</c>
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="settings">The <see cref="JsonSerializerSettings"/></param>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <returns></returns>
        /// <remarks>
        /// This method uses the Newtsonsoft API to deserialize.
        /// 
        /// Consider using <see cref="HttpCompletionOption.ResponseHeadersRead"/>
        /// with <see cref="HttpClient.SendAsync(HttpRequestMessage, HttpCompletionOption)"/>
        /// for increased performance.
        /// </remarks>
        public static async Task<TInstance> StreamToInstanceAsync<TInstance>(this HttpResponseMessage response, JsonSerializerSettings settings)
        {
            if (response == null) return await Task
                .FromResult(default(TInstance))
                .ConfigureAwait(continueOnCapturedContext: false);

            var stream = await response.Content.ReadAsStreamAsync();

            if (stream == null || stream.CanRead == false) return await Task
                .FromResult(default(TInstance))
                .ConfigureAwait(continueOnCapturedContext: false);

            using (var streamReader = new StreamReader(stream))
            using (var textReader = new JsonTextReader(streamReader))
            {
                var serializer = JsonSerializer.Create(settings);
                var instance = serializer.Deserialize<TInstance>(textReader);

                return instance;
            }
        }

        /// <summary>
        /// Converts the specified <see cref="HttpResponseMessage"/>
        /// to <see cref="JContainer"/>
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        public static async Task<JContainer> ToJContainerAsync(this HttpResponseMessage response)
        {
            if (response == null) return await Task
                .FromResult(default(JContainer))
                .ConfigureAwait(continueOnCapturedContext: false);

            var content = await response.Content
                .ReadAsStringAsync()
                .ConfigureAwait(continueOnCapturedContext: false);

            if (string.IsNullOrWhiteSpace(content)) return null;

            return JToken.Parse(content) as JContainer;
        }
    }
}
