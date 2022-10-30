using System.Net.Http.Headers;

namespace Songhay;

/// <summary>
/// Shared REST routines for Azure BLOB Storage.
/// </summary>
/// <remarks>
/// See “How To Access Azure Key Vault Secrets Through Rest API Using Postman” by Anupam Maiti
/// [ https://www.c-sharpcorner.com/article/how-to-access-azure-key-vault-secrets-through-rest-api-using-postman/ ]
/// </remarks>
public static class AzureKeyVaultRestApiUtility
{
    /// <summary>
    /// Calls the specified REST endpoint, using OAuth to authenticate the specified access data.
    /// </summary>
    /// <param name="accessUri">the location of the Azure Active Directory endpoint</param>
    /// <param name="accessData">the data required to <c>POST</c> to the specified endpoint</param>
    /// <remarks>
    /// The conventional way to generate the <c>accessUri</c> is via <see cref="RestApiMetadata"/>,
    /// its <see cref="RestApiMetadataExtensions.ToAzureActiveDirectoryAccessTokenUri"/> method.
    ///
    /// The conventional way to generate the <c>accessData</c> is via <see cref="RestApiMetadata"/>,
    /// its <see cref="RestApiMetadataExtensions.ToAzureActiveDirectoryAccessTokenData"/> method.
    /// </remarks>
    public static async Task<string> GetAccessTokenAsync(Uri? accessUri, Dictionary<string, string>? accessData)
    {
        ArgumentNullException.ThrowIfNull(accessUri);
        ArgumentNullException.ThrowIfNull(accessData);

        var request = new HttpRequestMessage(HttpMethod.Post, accessUri) {
             Content = new FormUrlEncodedContent(accessData)
        };
        var result = await request.SendAsync();

        result.EnsureSuccessStatusCode();

        var accessToken = await result.Content.ReadAsStringAsync();

        return accessToken;
    }

    /// <summary>
    /// Calls REST endpoint under <c>{vaultName}.vault.azure.net</c>
    /// to get a secret from the specified vault.
    /// </summary>
    /// <param name="accessToken">the JSON obtained from <see cref="GetAccessTokenAsync" /></param>
    /// <param name="secretUri">the location of the Azure Key Vault endpoint</param>
    /// <remarks>
    /// The conventional way to generate the <c>secretUri</c> is via <see cref="RestApiMetadata"/>,
    /// its <see cref="RestApiMetadataExtensions.ToAzureKeyVaultSecretUri"/> method.
    /// </remarks>
    public static async Task<string> GetSecretAsync(string accessToken, Uri secretUri)
    {
        accessToken.ThrowWhenNullOrWhiteSpace();
        ArgumentNullException.ThrowIfNull(secretUri);

        var parameter = JsonDocument
            .Parse(accessToken)
            .RootElement
            .GetProperty("access_token")
            .GetString();

        var request = new HttpRequestMessage(HttpMethod.Get, secretUri);
        request.Headers.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter);

        var secretResult = await request.SendAsync();

        secretResult.EnsureSuccessStatusCode();

        var secret = await secretResult.Content.ReadAsStringAsync();

        return secret;
    }
}
