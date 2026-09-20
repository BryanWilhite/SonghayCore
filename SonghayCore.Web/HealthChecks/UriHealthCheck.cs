using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

using Songhay.Web.Models;

namespace Songhay.Web.HealthChecks;

/// <summary>
/// Implements <see cref="IHealthCheck"/>
/// to make HEAD requests with the injected <see cref="ApiUriSet"/>.
/// </summary>
/// <param name="httpClientFactory">the <see cref="IHttpClientFactory"/></param>
/// <param name="uriHealthCheckSet">the <see cref="ApiUriSet"/> for the Health Check</param>
public sealed class UriHealthCheck(
    IHttpClientFactory httpClientFactory,
    ApiUriSet uriHealthCheckSet,
    ILogger<UriHealthCheck> logger) : IHealthCheck
{
    public const string Name = "uri-health-check";

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        logger.LogDebug("Loading feeds information...");

        HttpClient httpClientForDownload =
            httpClientFactory.CreateClient(nameof(UriHealthCheck));

        logger.LogDebug("Fetching feeds...");

        StringBuilder sbHealthy = new();
        StringBuilder sbUnhealthy = new();

        try
        {
            foreach (KeyValuePair<string, Uri> feed in uriHealthCheckSet)
            {
                HttpRequestMessage request = new(HttpMethod.Head, feed.Value);

                long startedAt = Stopwatch.GetTimestamp();

                using HttpResponseMessage response = await httpClientForDownload.SendAsync(request, cancellationToken);

                TimeSpan elapsed = Stopwatch.GetElapsedTime(startedAt);

                string message =
                    $"For {feed.Key} ({feed.Value}),"
                    + $"response is {response.StatusCode} ({response.ReasonPhrase})"
                    + $"... elapsed time: {elapsed.TotalMilliseconds:F0} ms"
                    + "Continuing... ";

                if (!response.IsSuccessStatusCode)
                {
                    logger.LogError("{S}", message);

                    sbUnhealthy.Append(message);
                }
                else
                {
                    sbHealthy.Append(message);
                }

            }
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"{nameof(Exception)}: {sbUnhealthy}", ex);
        }

        return sbUnhealthy.Length > 0 ?
            HealthCheckResult.Unhealthy(sbUnhealthy.ToString())
            :
            HealthCheckResult.Healthy(sbHealthy.ToString());
    }
}
