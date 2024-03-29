﻿using System.Net.Http.Headers;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="ProgramMetadata"/>.
/// </summary>
public static class ProgramMetadataExtensions
{
    /// <summary>
    /// Converts <see cref="ProgramMetadata" />
    /// to the conventional <see cref="HttpRequestHeaders"/>.
    /// </summary>
    /// <param name="meta">The <see cref="ProgramMetadata"/>.</param>
    /// <param name="restApiMetadataSetKey">The key for <see cref="ProgramMetadata.RestApiMetadataSet"/>.</param>
    public static Dictionary<string, string> ToConventionalHeaders(this ProgramMetadata? meta,
        string restApiMetadataSetKey)
    {
        ArgumentNullException.ThrowIfNull(meta);

        var genWebApiMeta = meta.RestApiMetadataSet.TryGetValueWithKey(restApiMetadataSetKey);
        if (genWebApiMeta == null) throw new NullReferenceException(nameof(genWebApiMeta));

        var headers = new Dictionary<string, string>
        {
            {
                genWebApiMeta.ClaimsSet.TryGetValueWithKey(RestApiMetadata.ClaimsSetHeaderApiKey).ToReferenceTypeValueOrThrow(),
                genWebApiMeta.ApiKey ??
                throw new NullReferenceException(
                    $"The expected {nameof(RestApiMetadata.ClaimsSetHeaderApiKey)} is not here.")
            }
        };

        return headers;
    }
}
