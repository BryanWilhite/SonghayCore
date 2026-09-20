using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Songhay.Models;
using Songhay.Web.Models;

namespace Songhay.Web.Handlers;

public sealed class ApiKeyAuthenticationHandler(
    IOptionsMonitor<ApiKeyAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    [FromKeyedServices(ApiKeyConstants.DepKeyForRestApiMetadata)] RestApiMetadata restApiMetadata)
    : AuthenticationHandler<ApiKeyAuthenticationOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(
                ApiKeyAuthenticationOptions.HeaderName, out StringValues providedKey))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        string? expectedKey = restApiMetadata.ApiKey;

        if (string.IsNullOrWhiteSpace(expectedKey))
        {
            return Task.FromResult(AuthenticateResult.Fail("API key is not configured."));
        }

        if (IsExpectedKey(expectedKey, providedKey))
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid API key."));
        }

        AuthenticationTicket ticket = GetAuthenticationTicket();

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    private static bool IsExpectedKey(string expectedKey, StringValues providedKey)
    {
        byte[] providedBytes = Encoding.UTF8.GetBytes(providedKey.ToString());
        byte[] expectedBytes = Encoding.UTF8.GetBytes(expectedKey);

        return providedBytes.Length != expectedBytes.Length ||
            !CryptographicOperations.FixedTimeEquals(providedBytes, expectedBytes);
    }

    private AuthenticationTicket GetAuthenticationTicket()
    {
        Claim[] claims =
        [
            new Claim(ClaimTypes.Name, "static-client"),
            new Claim("client_id", "static-client")
        ];

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return ticket;
    }
}
