namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="RestApiMetadata"/>.
/// </summary>
public static class RestApiMetadataExtensions
{
    /// <summary>
    /// Converts the specified <see cref="RestApiMetadata" />
    /// to the data required by <see cref="AzureKeyVaultRestApiUtility.GetAccessTokenAsync"/>.
    /// </summary>
    /// <param name="meta">The meta.</param>
    /// <remarks>
    /// As of this writing, this member should return data of the form:
    ///
    /// <code>
    /// "grant_type": "client_credentials",
    /// "scope": "https://vault.azure.net/.default",
    /// "client_id": "active-directory-registration-app-id",
    /// "client_secret": "active-directory-registration-app-secret"
    /// </code>
    ///
    /// where <c>client_id</c> is the value of the <c>appId</c> property
    /// in the output of <c>az ad app list</c>;
    /// <c>client_secret</c> is the secret exposed under the App registration in the Azure Portal.
    /// </remarks>
    public static Dictionary<string, string> ToAzureActiveDirectoryAccessTokenData(this RestApiMetadata? meta)
    {
        ArgumentNullException.ThrowIfNull(meta);

        var pairs = meta.ClaimsSet.Where(i => i.Key != "tenantOrDirectoryId");
        var accessData = new Dictionary<string, string>(pairs);

        return accessData;
    }

    /// <summary>
    /// Converts the specified <see cref="RestApiMetadata" />
    /// to the <see cref="Uri"/> required by <see cref="AzureKeyVaultRestApiUtility.GetAccessTokenAsync"/>.
    /// </summary>
    /// <remarks>
    /// This member should return a <see cref="Uri"/> of the form:
    ///
    /// <code>
    /// https://login.microsoftonline.com/{tenantOrDirectoryId}/oauth2/v2.0/token
    /// </code>
    ///
    /// where <c>tenantOrDirectoryId</c> is the value of “Directory (tenant) ID”
    /// under the App registration in the Azure Portal.
    /// </remarks>
    public static Uri ToAzureActiveDirectoryAccessTokenUri(this RestApiMetadata? meta)
    {
        ArgumentNullException.ThrowIfNull(meta);

        var tenantOrDirectoryId = meta.ClaimsSet.TryGetValueWithKey("tenantOrDirectoryId");
        var uri = meta.ToUri("UriPathTemplateForToken", tenantOrDirectoryId).ToReferenceTypeValueOrThrow();

        return uri;
    }

    /// <summary>
    /// Converts the specified <see cref="RestApiMetadata" />
    /// to the <see cref="Uri"/> required by <see cref="AzureKeyVaultRestApiUtility.GetSecretAsync"/>.
    /// </summary>
    /// <remarks>
    /// This member should return a <see cref="Uri"/> of the form:
    ///
    /// <code>
    /// https://{vaultName}.vault.azure.net/secrets/{secretName}?api-version=2016-10-01
    /// </code>
    ///
    /// where <c>vaultName</c> is the name of the Azure Key Vault;
    /// <c>secretName</c> is the name of the secret in the vault.
    /// </remarks>
    public static Uri ToAzureKeyVaultSecretUri(this RestApiMetadata? meta)
    {
        ArgumentNullException.ThrowIfNull(meta);

        var tenantOrDirectoryId = meta.ClaimsSet.TryGetValueWithKey("tenantOrDirectoryId");
        var uri = meta.ToUri("UriPathTemplateForToken", tenantOrDirectoryId).ToReferenceTypeValueOrThrow();

        return uri;
    }

    /// <summary>
    /// Converts the specified <see cref="RestApiMetadata" /> to a <see cref="Uri"/>
    /// based on the specified URI Template.
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
