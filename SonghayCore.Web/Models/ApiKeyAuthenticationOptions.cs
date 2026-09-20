using Microsoft.AspNetCore.Authentication;

namespace Songhay.Web.Models;

/// <summary>
/// Extends <see cref="AuthenticationSchemeOptions"/>
/// to define concepts for API-key authentication.
/// </summary>
/// <remarks>
/// <para>
/// “Subclassing `AuthenticationSchemeOptions` is the standard pattern.
/// The constants live alongside
/// so I can reference `ApiKeyAuthenticationOptions.DefaultScheme`
/// instead of magic strings.”
/// </para>
/// <para>
/// — Mukesh Murugan, <see cref="https://codewithmukesh.com/blog/api-key-authentication-aspnet-core/"/>
/// </para>
/// </remarks>
public sealed class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    /// <summary>
    /// Specifies the name of the scheme to use by default
    /// when a specific scheme isn't requested
    /// for <see cref="AuthenticationServiceCollectionExtensions.AddAuthentication(IServiceCollection, string)"/>
    /// </summary>
    public const string DefaultScheme = "ApiKey";

    /// <summary>
    /// The conventional HTTP header name for API-key authentication.
    /// </summary>
    /// <remarks>
    /// <para>
    /// “The de-facto header name is X-API-Key.
    /// It is not a formal HTTP standard - the X- prefix
    /// was actually deprecated by RFC 6648 -
    /// but it has become the convention because
    /// of widespread adoption
    /// (AWS API Gateway, GitHub before tokens, most webhook providers).
    /// Some APIs use the Authorization header
    /// with a custom scheme like Authorization: ApiKey <c>{key}</c>.
    /// Both work; pick one and stick with it.”
    /// </para>
    /// <para>
    /// — Mukesh Murugan, <see cref="https://codewithmukesh.com/blog/api-key-authentication-aspnet-core/"/>
    /// </para>
    /// </remarks>
    public const string HeaderName = "X-API-Key";
}
