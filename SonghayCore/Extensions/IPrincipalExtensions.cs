using System.Security.Principal;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="IPrincipal"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IPrincipalExtensions
{
    /// <summary>
    /// Converts <see cref="IPrincipal"/> to a <see cref="string"/> representation of the user’s name.
    /// </summary>
    /// <param name="principal">The principal.</param>
    /// <param name="defaultUserName">Default name of the user.</param>
    /// <returns></returns>
    public static string ToUserName(this IPrincipal? principal, string defaultUserName)
    {
        var userName = principal?.Identity?.Name;

        return userName ?? defaultUserName;
    }
}
