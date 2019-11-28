#if NETSTANDARD

using Newtonsoft.Json.Linq;
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
