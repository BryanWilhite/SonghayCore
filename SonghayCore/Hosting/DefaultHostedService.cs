using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Songhay.Hosting;

/// <summary>
/// Defines the conventional, default Hosted Service
/// </summary>
public class DefaultHostedService: IHostedService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultHostedService"/> class.
    /// </summary>
    /// <param name="hostApplicationLifetime">the <see cref="IHostApplicationLifetime"/></param>
    /// <param name="logger">the <see cref="ILogger"/></param>
    public DefaultHostedService(IHostApplicationLifetime hostApplicationLifetime, ILogger<DefaultHostedService> logger)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _logger = logger;
    }

    /// <summary>
    /// Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _hostApplicationLifetime.ApplicationStarted.Register(() =>
        {
            _logger.LogWarning("Warning: fallback to {Name}! Stopping app...", nameof(DefaultHostedService));
            _exitCode = 0;
            _hostApplicationLifetime.StopApplication();
        });

        return Task.CompletedTask;
    }

    /// <summary>
    /// Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogWarning("Stopping `{Name}`...", nameof(DefaultHostedService));

        Environment.ExitCode = _exitCode.GetValueOrDefault(-1);
        // FUNKYKB: exit code may be null should use enter Ctrl+c/SIGTERM.

        return Task.CompletedTask;
    }

    readonly IHostApplicationLifetime _hostApplicationLifetime;
    readonly ILogger<DefaultHostedService> _logger;

    int? _exitCode;
}
