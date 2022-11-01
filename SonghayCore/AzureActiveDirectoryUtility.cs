namespace Songhay;

/// <summary>
/// Shared routines for Azure Active Directory conventions.
/// </summary>
/// <remarks>
/// These utilities are expecting a JSON <see cref="string"/> of the form:
/// <code>
/// {
///    "ActiveDirectoryAccess": {
///        "ApiBase": "https://login.microsoftonline.com/",
///        "ClaimsSet": {
///            "grant_type": "client_credentials",
///            "scope": "https://vault.azure.net/.default",
///            "client_id": "active-directory-registration-app-id",
///            "client_secret": "active-directory-registration-app-secret",
///            "tenantOrDirectoryId": "active-directory-registration-app-directory-id"
///        },
///        "UriTemplates": {
///            "UriPathTemplateForToken": "{tenantOrDirectoryId}/oauth2/v2.0/token"
///        }
///    },
///    "AzureKeyVault": {
///        "ApiBase": "https://your-secrets.vault.azure.net/",
///        "ClaimsSet": {
///            "queryPairForApiVersion": "api-version=2016-10-01",
///            "secretNameForMySecret": "my-secret"
///        },
///        "UriTemplates": {
///            "UriPathTemplateForSecret": "secrets/{secretName}"
///        }
///    }
///}
/// </code>
///
/// For security reasons, a very small precaution, this class will not hold any fields.
/// </remarks>
public class AzureActiveDirectoryUtility
{
    /// <summary>
    /// Returns the <see cref="RestApiMetadata"/> corresponding
    /// to the conventional property name <c>"ActiveDirectoryAccess"</c>.
    /// </summary>
    /// <param name="json">JSON of the format shown in the remarks of this class definition.</param>
    public static RestApiMetadata GetActiveDirectoryAccessMetadata(string? json)
    {
        json.ThrowWhenNullOrWhiteSpace();

        var directoryServiceMetadata = JsonSerializer.Deserialize<Dictionary<string, RestApiMetadata>>(json);
        var meta = directoryServiceMetadata
            .TryGetValueWithKey("ActiveDirectoryAccess")
            .ToReferenceTypeValueOrThrow();

        return meta;
    }

    /// <summary>
    /// Returns the <see cref="RestApiMetadata"/> corresponding
    /// to the conventional property name <c>"ActiveDirectoryAccess"</c>.
    /// </summary>
    /// <param name="json">JSON of the format shown in the remarks of this class definition.</param>
    public static RestApiMetadata GetAzureKeyVaultMetadata(string? json)
    {
        json.ThrowWhenNullOrWhiteSpace();

        var directoryServiceMetadata = JsonSerializer.Deserialize<Dictionary<string, RestApiMetadata>>(json);
        var meta = directoryServiceMetadata
            .TryGetValueWithKey("AzureKeyVault")
            .ToReferenceTypeValueOrThrow();

        return meta;
    }
}
