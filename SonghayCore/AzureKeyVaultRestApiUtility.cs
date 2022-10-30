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
    /// Calls REST endpoint under <c>login.microsoftonline.com</c>,
    /// using OAuth to authenticate the specified client ID and secret
    /// of the app registered in the Azure Active Directory of your enterprise.
    /// </summary>
    /// <param name="clientId">the value of the <c>appId</c> property in the output of <c>az ad app list</c>.</param>
    /// <param name="clientSecret">the secret exposed under the App registration in the Azure Portal</param>
    /// <param name="tenantOrDirectoryId">the “Directory (tenant) ID” under the App registration in the Azure Portal</param>
    public static async Task<string> GetAccessToken(string? clientId, string? clientSecret, string? tenantOrDirectoryId)
    {
        clientId.ThrowWhenNullOrWhiteSpace();
        clientSecret.ThrowWhenNullOrWhiteSpace();
        tenantOrDirectoryId.ThrowWhenNullOrWhiteSpace();

        const string GrantType = "client_credentials";
        const string Scope = "https://vault.azure.net/.default";

        var data = new Dictionary<string, string>
        {
            { "grant_type", GrantType },
            { "scope", Scope },
            { "client_id", clientId },
            { "client_secret", clientSecret },
        };

        var uriBuilder = new UriBuilder("https://login.microsoftonline.com/");
        uriBuilder.Path = $"{tenantOrDirectoryId}/oauth2/v2.0/token";

        var request = new HttpRequestMessage(HttpMethod.Post, uriBuilder.Uri) {
             Content = new FormUrlEncodedContent(data)
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
    /// <param name="accessToken">the value of the <c>access_token</c> JSON property obtained from <see cref="GetAccessToken" /></param>
    /// <param name="vaultName">the name of the Azure Key Vault</param>
    /// <param name="secretName">the name of the Vault secret</param>
    public static async Task<string> GetSecret(string? accessToken, string? vaultName, string? secretName)
        => await GetSecret(accessToken, vaultName, secretName, "api-version=2016-10-01");

    /// <summary>
    /// Calls REST endpoint under <c>{vaultName}.vault.azure.net</c>
    /// to get a secret from the specified vault.
    /// </summary>
    /// <param name="accessToken">the JSON obtained from <see cref="GetAccessToken" /></param>
    /// <param name="vaultName">the name of the Azure Key Vault</param>
    /// <param name="secretName">the name of the Vault secret</param>
    /// <param name="vaultUriQuery">the query-string portion of the REST-endpoint <see cref="Uri" /></param>
    public static async Task<string> GetSecret(string? accessToken, string? vaultName, string? secretName, string? vaultUriQuery)
    {
        accessToken.ThrowWhenNullOrWhiteSpace();
        vaultName.ThrowWhenNullOrWhiteSpace();
        secretName.ThrowWhenNullOrWhiteSpace();

        var secretUriBuilder = new UriBuilder($"https://{vaultName}.vault.azure.net/");
        secretUriBuilder.Path = $"secrets/{secretName}";
        if(!string.IsNullOrWhiteSpace(vaultUriQuery))secretUriBuilder.Query = vaultUriQuery;

        var accessTokenDocument = JsonDocument.Parse(accessToken);
        var parameter = accessTokenDocument.RootElement.GetProperty("access_token").GetString();

        var request = new HttpRequestMessage(HttpMethod.Get, secretUriBuilder.Uri);
        request.Headers.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter);

        var secretResult = await request.SendAsync();

        secretResult.EnsureSuccessStatusCode();

        var secret = await secretResult.Content.ReadAsStringAsync();

        return secret;
    }
}
