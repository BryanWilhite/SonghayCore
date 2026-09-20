namespace Songhay.Web.Models;

/// <summary>
/// Constants for API-key authentication
/// </summary>
public static class ApiKeyConstants
{
    /// <summary>
    /// A constant for keyed DI to inject an instance
    /// of <see cref="RestApiMetadata"/>.
    /// </summary>
    public const string DepKeyForRestApiMetadata = "rx-api-key-rest-api-metadata";
}
