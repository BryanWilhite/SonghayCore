#if NETSTANDARD

using Newtonsoft.Json.Linq;
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
        /// Returns <c>true</c> when <see cref="HttpResponseMessage"/>
        /// is <see cref="HttpStatusCode.Moved"/>, <see cref="HttpStatusCode.MovedPermanently"/>
        /// or <see cref="HttpStatusCode.Redirect"/>.
        /// </summary>
        /// <param name="response">The response.</param>
        public static bool IsMovedOrRedirected(this HttpResponseMessage response) =>
            response.StatusCode == HttpStatusCode.Moved ||
            response.StatusCode == HttpStatusCode.MovedPermanently ||
            response.StatusCode == HttpStatusCode.Redirect;

        /// <summary>
        /// Converts the specified <see cref="HttpResponseMessage"/>
        /// to <see cref="JContainer"/>
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static async Task<JContainer> ToJContainerAsync(this HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content)) return null;

            return JToken.Parse(content) as JContainer;
        }
    }
}

#endif
