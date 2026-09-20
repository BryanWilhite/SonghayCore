using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Songhay.Web;

/// <summary>
/// Shared routines for ASP.NET Health Checks
/// </summary>
public static class HealthCheckUtility
{
    /// <summary>
    /// Returns <see cref="HealthCheckOptions"/>
    /// that prevent any checks from running.
    /// </summary>
    /// <remarks>
    /// These options are useful for a liveness check.
    /// A 200 returned just means the process can serve a request.
    /// </remarks>
    public static HealthCheckOptions GetHealthCheckOptionsForZeroChecks() => new()
        {
            Predicate = _ => false
        };

    /// <summary>
    /// Returns <see cref="HealthCheckOptions"/>
    /// that prevent any checks from running.
    /// </summary>
    /// <param name="predicate">the value of <see cref="HealthCheckOptions.Predicate"/></param>
    public static HealthCheckOptions GetHealthCheckOptionsWithFiltering(Func<HealthCheckRegistration, bool>? predicate) => new()
        {
                Predicate = predicate
        };
}
