using System.Net.Http;

namespace Songhay.Models;

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
    public const string Delete = "DELETE";

    /// <summary>
    /// HTTP Method <c>GET</c>
    /// </summary>
    public const string Get = "GET";

    /// <summary>
    /// HTTP Method <c>HEAD</c>
    /// </summary>
    public const string Head = "HEAD";

    /// <summary>
    /// HTTP Method <c>OPTIONS</c>
    /// </summary>
    public const string Options = "OPTIONS";

    /// <summary>
    /// HTTP Method <c>POST</c>
    /// </summary>
    public const string Post = "POST";

    /// <summary>
    /// HTTP Method <c>PUT</c>
    /// </summary>
    public const string Put = "PUT";

    /// <summary>
    /// HTTP Method <c>TRACE</c>
    /// </summary>
    public const string Trace = "TRACE";
}
