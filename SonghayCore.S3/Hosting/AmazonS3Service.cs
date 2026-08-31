using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Songhay.S3.Models;

namespace Songhay.S3.Hosting;

/// <summary>
/// The domain-specific service for <see cref="Songhay.S3.Activities"/>.
/// </summary>
/// <param name="hostApplicationLifetime">the <see cref="IHostApplicationLifetime"/></param>
/// <param name="configuration">the <see cref="IConfiguration"/></param>
/// <param name="amazonS3ActivityGroup">the abstraction that groups <see cref="Songhay.S3.Activities"/> input and invocation</param>
/// <param name="logger">the <see cref="ILogger"/></param>
/// <remarks>
/// This class is intended for collecting input from <see cref="IConfiguration"/>.
/// To enter input directly, use <see cref="AmazonS3ActivityGroup.InvokeActivityAsync"/>.
/// </remarks>
public class AmazonS3Service(IHostApplicationLifetime hostApplicationLifetime, IConfiguration configuration, IActivityKeyedTaskGroup amazonS3ActivityGroup, ILogger<AmazonS3Service> logger) : BackgroundService
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

        try
        {
            string? setKey = configuration.GetCommandLineArgValue(ArgSetKey);
            if (string.IsNullOrWhiteSpace(setKey))
            {
                logger.LogInformation("{S}", GetHelpText());

                return;
            }

            string? bucketMetaKey = configuration.GetCommandLineArgValue(ArgBucketMetaKey);
            if (string.IsNullOrWhiteSpace(bucketMetaKey))
            {
                logger.LogInformation("{S}", GetHelpText());

                return;
            }

            string? bucketKey = configuration.GetCommandLineArgValue(ArgBucketKey);
            string? content = configuration.ReadStringInput();
            string? contentMimeType = configuration.GetCommandLineArgValue(ArgBucketS3ObjectMimetype);

            string? activitySetKey = configuration.GetCommandLineArgValue(ConsoleArgsScalars.ActivityName);
            if (string.IsNullOrWhiteSpace(activitySetKey))
            {
                logger.LogInformation("{S}", GetHelpText());

                return;
            }

            string? output = await amazonS3ActivityGroup.InvokeActivityAsync(activitySetKey, setKey, bucketMetaKey, bucketKey, content, contentMimeType);

            string? path = configuration.GetOutputPath();

            if (!string.IsNullOrWhiteSpace(path))
            {
                logger.LogInformation("Writing output to `{Path}`...", path);

                await File.WriteAllTextAsync(path, output, stoppingToken);
            }
        }
        finally
        {
            hostApplicationLifetime.StopApplication();
        }
    }

    private string? GetHelpText()
    {
        if (!string.IsNullOrWhiteSpace(_cachedHelpText)) return _cachedHelpText;

        configuration.AddHelpDisplayText(ArgSetKey, "a key in the conventional `ProgramMetadata.RestApiMetadataSet` dictionary");
        configuration.AddHelpDisplayText(ArgBucketMetaKey, "a key in the conventional `RestApiMetadata.ClaimsSet` dictionary");
        configuration.AddHelpDisplayText(ArgBucketKey, "the value of `S3Object.Key`");
        configuration.AddHelpDisplayText(ArgBucketS3ObjectMimetype, "the value of `PutObjectRequest.ContentType` or its equivalent");

        _cachedHelpText = configuration.WithDefaultHelpText().ToHelpDisplayText();

        return _cachedHelpText;
    }

    private string? _cachedHelpText;

    private const string ArgSetKey = "--set-key";
    private const string ArgBucketMetaKey = "--bucket-meta-key";
    private const string ArgBucketKey = "--bucket-key";
    private const string ArgBucketS3ObjectMimetype = "--bucket-object-mime-type";
}
