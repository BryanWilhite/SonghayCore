using System.Net;

using Amazon.S3;
using Amazon.S3.Model;

using Songhay.Models;
using SonghayCore.S3;
using SonghayCore.S3.Extensions;

namespace Songhay.Tests;

public class S3UtilityTests
{
    public S3UtilityTests(ITestOutputHelper testOutputHelper)
    {
        _loggerProvider = new XUnitLoggerProvider(testOutputHelper);

        if(string.IsNullOrWhiteSpace(SettingsPath))
        {
            _programMetadata = new ProgramMetadata();

            return;
        }

        string json = File.ReadAllText(SettingsPath);

        using var jDoc = JsonDocument.Parse(json);

        string metaJson = jDoc.RootElement
            .GetProperty(nameof(ProgramMetadata))
            .ToString();

        _programMetadata = JsonSerializer
            .Deserialize<ProgramMetadata>(metaJson)
            .ToReferenceTypeValueOrThrow();
    }

    [SkippableTheory]
    [InlineData("Wasabi", "studio-public-region")]
    public async Task ShouldGetPositiveHeadBucketResponse(string setKey, string bucketMetaKey)
    {
        Skip.If(string.IsNullOrWhiteSpace(SettingsPath));

        //arrange:
        bool arrangeCompleted = true;
        ILogger logger = _loggerProvider.CreateLogger(nameof(ShouldGetPositiveHeadBucketResponse));

        RestApiMetadata wasabiMeta = _programMetadata.RestApiMetadataSet
            .TryGetValueWithKey(setKey).ToReferenceTypeValueOrThrow();

        var (credentialsProfileName, bucketName, region, uriRoot) = wasabiMeta.ToS3Tuple(bucketMetaKey, logger);

        logger.LogDebug("{Name}: {Value}", nameof(region), region);
        logger.LogDebug("{Name}: {Value}", nameof(bucketName), bucketName);
        logger.LogDebug("{Name}: {Value}", nameof(uriRoot), uriRoot);

        AmazonS3Client s3Client = S3Utility
            .GetAmazonS3Client(credentialsProfileName, uriRoot, nameof(ShouldGetPositiveHeadBucketResponse), logger)
            .ToReferenceTypeValueOrThrow();

        HeadBucketRequest request = new() { BucketName = bucketName };

        //act:
        HeadBucketResponse actual = await s3Client.HeadBucketAsync(request).ConfigureAwait(false);

        //assert:
        Assert.True(arrangeCompleted);
        Assert.Equal(HttpStatusCode.OK, actual.HttpStatusCode);
    }

    private static readonly string? SettingsPath = Environment.GetEnvironmentVariable("SONGHAY_APP_SETTINGS_PATH");

    private readonly ProgramMetadata _programMetadata;
    private readonly XUnitLoggerProvider _loggerProvider;

}