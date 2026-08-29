using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Songhay.S3.Abstractions;

namespace Songhay.S3.Hosting;

/// <summary>
/// The domain-specific service for <see cref="Songhay.S3.Activities"/>.
/// </summary>
/// <remarks>
/// This class is intended for collecting input from <see cref="IConfiguration"/>.
/// To enter input directly, use an instance
/// of <see cref="IAmazonS3ActivityGroup"/>.
/// </remarks>
public class AmazonS3Service(IConfiguration configuration, IAmazonS3ActivityGroup amazonS3ActivityGroup, ILogger<AmazonS3Service> logger) : BackgroundService
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="stoppingToken"></param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ILoggerUtility.AsInstanceOrNullLogger(logger);

        logger.LogInformation("{ActivityName} starting...", nameof(AmazonS3Service));

        string? setKey = configuration.GetCommandLineArgValue(ArgSetKey);
        setKey.ThrowWhenNullOrWhiteSpace();

        string? bucketMetaKey = configuration.GetCommandLineArgValue(ArgBucketMetaKey);
        bucketMetaKey.ThrowWhenNullOrWhiteSpace();

        string? bucketKey = configuration.GetCommandLineArgValue(ArgBucketKey);
        string? content = configuration.ReadStringInput();
        string? contentMimeType = configuration.GetCommandLineArgValue(ArgBucketS3ObjectMimetype);

        string? activitySetKey = configuration.GetCommandLineArgValue(ConsoleArgsScalars.ActivityName);
        activitySetKey.ThrowWhenNullOrWhiteSpace();

        string? output = await amazonS3ActivityGroup.InvokeActivityAsync(activitySetKey, setKey, bucketMetaKey, bucketKey, content, contentMimeType);

        string? path = configuration.GetOutputPath();

        if (!string.IsNullOrWhiteSpace(path))
        {
            logger.LogInformation("Writing output to `{Path}`...", path);

            await File.WriteAllTextAsync(path, output, stoppingToken);
        }
    }

    internal const string ArgSetKey = "--set-key";
    internal const string ArgBucketMetaKey = "--bucket-meta-key";
    internal const string ArgBucketKey = "--bucket-key";
    internal const string ArgBucketS3ObjectMimetype = "--bucket-object-mime-type";
}
