namespace Songhay.Web.Models;

/// <summary>
/// Constants for ASP.NET Health Check conventions.
/// </summary>
public static class HealthCheckConstants
{
    /// <summary>
    /// Liveness route
    /// </summary>
    public const string LivenessRoute = "health/live";

    /// <summary>
    /// Live
    /// </summary>
    public const string Live = "live";

    /// <summary>
    /// Readiness route
    /// </summary>
    public const string ReadinessRoute = "health/ready";

    /// <summary>
    /// Ready
    /// </summary>
    public const string Ready = "ready";

}
