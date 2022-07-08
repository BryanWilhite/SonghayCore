using System;
using System.Collections.Generic;
using System.Text;

namespace Songhay.Models;

/// <summary>
/// REST API Metadata
/// </summary>
public class RestApiMetadata
{
    /// <summary>
    /// A convential name representing an API key
    /// to sent in the HTTP headers for authentication.
    /// </summary>
    /// <remarks>
    /// Usually <c>"headerAuthorization": "Authorization"</c>.
    /// 
    /// See https://docs.microsoft.com/en-us/rest/api/azure/#request-header
    /// </remarks>
    public const string ClaimsSetHeaderApiAuthorization = "headerAuthorization";

    /// <summary>
    /// A convential name representing an API key
    /// to sent in the HTTP headers for authentication.
    /// </summary>
    /// <remarks>
    /// Usually <c>"headerContentType": "Content-Type"</c>.
    /// 
    /// See https://docs.microsoft.com/en-us/rest/api/azure/#request-header
    /// </remarks>
    public const string ClaimsSetHeaderApiContentType = "headerContentType";

    /// <summary>
    /// A convential name representing an API key
    /// to sent in the HTTP headers for authentication.
    /// </summary>
    /// <remarks>
    /// Usually <c>"headerKey": "X-ApiKey"</c>.
    /// </remarks>
    public const string ClaimsSetHeaderApiKey = "headerKey";

    /// <summary>
    /// Gets or sets the API base.
    /// </summary>
    /// <value>
    /// The API base.
    /// </value>
    public Uri ApiBase { get; set; }

    /// <summary>
    /// Gets or sets the API key.
    /// </summary>
    /// <value>
    /// The API key.
    /// </value>
    public string ApiKey { get; set; }

    /// <summary>
    /// Gets or sets the claims set.
    /// </summary>
    /// <value>
    /// The claims set.
    /// </value>
    public Dictionary<string, string> ClaimsSet { get; set; }

    /// <summary>
    /// Gets or sets the URI templates.
    /// </summary>
    /// <value>
    /// The URI templates.
    /// </value>
    public Dictionary<string, string> UriTemplates { get; set; }

    /// <summary>
    /// Converts this instance into a <see cref="string"/> representation.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var sb = new StringBuilder();

        if (ApiBase != null) sb.AppendFormat("ApiBase: {0}", ApiBase);
        if (!string.IsNullOrWhiteSpace(ApiKey)) sb.AppendFormat(" ApiKey: {0}", ApiKey);

        if ((ClaimsSet != null) && (ClaimsSet.Count > 0))
        {
            sb.AppendLine($"{nameof(ClaimsSet)}:");
            foreach (var item in ClaimsSet)
            {
                sb.AppendLine($"    {item.Key}:");
                sb.AppendLine($"        {item.Value}");
            }
        }

        if ((UriTemplates != null) && (UriTemplates.Count > 0))
        {
            sb.AppendLine($"{nameof(UriTemplates)}:");
            foreach (var item in UriTemplates)
            {
                sb.AppendLine($"    {item.Key}:");
                sb.AppendLine($"        {item.Value}");
            }
        }

        return (sb.Length > 0) ? sb.ToString().Trim() : base.ToString();
    }
}