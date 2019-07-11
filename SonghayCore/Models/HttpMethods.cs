#if NETSTANDARD
using System.Net.Http;
#endif

namespace Songhay.Models
{
    /// <summary>
    /// Centralizes <see cref="HttpMethod"/> members as strings.
    /// </summary>
    /// <remarks>
    /// Reference: “HTTP request methods” [https://developer.mozilla.org/en-US/docs/Web/HTTP/Methods]
    /// </remarks>
    public static class HttpMethods
    {
        /// <summary>
        /// HTTP Method <c>DELETE</c>
        /// </summary>
        public const string DELETE = "DELETE";

        /// <summary>
        /// HTTP Method <c>GET</c>
        /// </summary>
        public const string GET = "GET";

        /// <summary>
        /// HTTP Method <c>HEAD</c>
        /// </summary>
        public const string HEAD = "HEAD";

        /// <summary>
        /// HTTP Method <c>OPTIONS</c>
        /// </summary>
        public const string OPTIONS = "OPTIONS";

        /// <summary>
        /// HTTP Method <c>POST</c>
        /// </summary>
        public const string POST = "POST";

        /// <summary>
        /// HTTP Method <c>PUT</c>
        /// </summary>
        public const string PUT = "PUT";

        /// <summary>
        /// HTTP Method <c>TRACE</c>
        /// </summary>
        public const string TRACE = "TRACE";
    }
}
