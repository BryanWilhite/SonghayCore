using Songhay.Models;
using Songhay.Web.Models;

namespace Songhay.Web.Extensions;

/// <summary>
/// Extensions of <see cref="RestApiMetadata"/>
/// for Studio conventions.
/// </summary>
/// <remarks>
/// Ideally, all magic-string activity
/// on all instances of <see cref="RestApiMetadata"/>
/// </remarks>
public static class RestApiMetadataExtensions
{
    /// <summary>
    /// Transforms <see cref="RestApiMetadata"/>
    /// to an instance of <see cref="ApiUriSet"/>
    /// by reading <see cref="RestApiMetadata.ClaimsSet"/>
    /// filtered by the specified prefix.
    /// </summary>
    /// <param name="restApiMetadata">the <see cref="RestApiMetadata"/></param>
    /// <param name="prefix">a conventional prefix</param>
    /// <remarks>
    /// This method is useful for <see cref="UriHealthCheck"/>.
    /// </remarks>
    public static ApiUriSet ToApiUriSetFromClaimSetByPrefix(this RestApiMetadata restApiMetadata, string? prefix)
    {
        if (string.IsNullOrWhiteSpace(prefix))
        {
            return [];
        }

        Dictionary<string, Uri> set =
            restApiMetadata.ClaimsSet
                .Where(kv => kv.Key.StartsWith(prefix))
                .ToDictionary(kv => kv.Key, kv => new Uri(kv.Value, UriKind.Absolute));

        ApiUriSet apiUriSet = new(set);

        return apiUriSet;
    }
}
