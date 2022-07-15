namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="RestApiMetadata"/>.
/// </summary>
public static class RestApiMetadataExtensions
{
    /// <summary>
    /// To the URI.
    /// </summary>
    /// <param name="meta">The meta.</param>
    /// <param name="uriTemplateKey">The URI template key.</param>
    /// <param name="bindByPositionValues">The bind by position values.</param>
    public static Uri? ToUri(this RestApiMetadata? meta, string? uriTemplateKey, params string?[] bindByPositionValues)
    {
        if (meta == null || meta.ApiBase == null) return null;

        bindByPositionValues.ThrowWhenNullOrEmpty();

        if (meta.UriTemplates.Keys.All(i => i != uriTemplateKey))
            throw new FormatException("The expected REST API metadata URI template key is not here.");

        const string forwardSlash = "/";
        var uriBase = meta.ApiBase.OriginalString.EndsWith(forwardSlash)
            ? string.Concat(meta.ApiBase.OriginalString, meta.UriTemplates[uriTemplateKey!])
            : string.Concat(meta.ApiBase.OriginalString, forwardSlash, meta.UriTemplates[uriTemplateKey!]);

        var uriTemplate = new UriTemplate(uriBase);
        var uri = uriTemplate.BindByPosition(bindByPositionValues!);

        return uri;
    }
}
