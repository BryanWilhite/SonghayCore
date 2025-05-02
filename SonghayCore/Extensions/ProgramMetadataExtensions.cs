using System.Net.Http.Headers;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="ProgramMetadata"/>.
/// </summary>
public static class ProgramMetadataExtensions
{
    /// <summary>
    /// Ensures that <see cref="ProgramMetadata.DbmsSet"/>
    /// and <see cref="ProgramMetadata.RestApiMetadataSet"/>
    /// are not null or throws <see cref="NullReferenceException"/>
    /// </summary>
    /// <param name="meta">The <see cref="ProgramMetadata"/>.</param>
    /// <exception cref="NullReferenceException"></exception>
    public static void EnsureProgramMetadata([NotNull] this ProgramMetadata? meta)
    {
        ArgumentNullException.ThrowIfNull(meta);

        if (meta.DbmsSet == null)
            throw new NullReferenceException($"The expected {nameof(ProgramMetadata.DbmsSet)} is not here.");

        if (meta.RestApiMetadataSet == null)
            throw new NullReferenceException($"The expected {nameof(ProgramMetadata.RestApiMetadataSet)} is not here.");
    }

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

    /// <summary>
    /// Converts <see cref="ProgramMetadata" />
    /// to the <see cref="RestApiMetadata"/>
    /// it contains, identified by the specified key.
    /// </summary>
    /// <param name="meta">The <see cref="ProgramMetadata"/>.</param>
    /// <param name="restApiMetadataSetKey">the key of <see cref="ProgramMetadata.RestApiMetadataSet"/></param>
    public static RestApiMetadata ToRestApiMetadata(this ProgramMetadata? meta, string? restApiMetadataSetKey)
    {
        ArgumentNullException.ThrowIfNull(meta);

        if (string.IsNullOrWhiteSpace(restApiMetadataSetKey))
            throw new ArgumentNullException(nameof(restApiMetadataSetKey));

        RestApiMetadata restApiMetadata = meta
            .RestApiMetadataSet
            .TryGetValueWithKey(restApiMetadataSetKey, throwException: true)
            .ToReferenceTypeValueOrThrow();

        return restApiMetadata;
    }
}
