using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Songhay.Web.Extensions;

/// <summary>
/// Extensions of <see cref="HealthCheckOptions"/>
/// </summary>
public static class HealthCheckOptionsExtensions
{
    /// <summary>
    /// Returns <see cref="HealthCheckOptions"/>
    /// with <see cref="HealthCheckOptions.AllowCachingResponses"/>
    /// set to <c>true</c>
    /// </summary>
    /// <param name="options">the <see cref="HealthCheckOptions"/></param>
    public static HealthCheckOptions WithClientCachingAllowed(this HealthCheckOptions? options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.AllowCachingResponses = true;

        return options;
    }

}
