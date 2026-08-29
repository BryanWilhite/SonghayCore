using System.Net;

using Amazon.S3;
using Amazon.S3.Model;

using Songhay.Models;
using SonghayCore.S3;

namespace Songhay.Tests;

public class AmazonS3UtilityTests
{
    public AmazonS3UtilityTests(ITestOutputHelper testOutputHelper)
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
    [InlineData("Wasabi", "studio-public-region", "songhay/feedly-from-string.opml")]
    public async Task ShouldDeleteS3Object(string setKey, string bucketMetaKey, string bucketKey)
    {
        const bool shouldSkip = false;

        Skip.If(shouldSkip);

        //arrange:
        ILogger logger = _loggerProvider.CreateLogger(nameof(ShouldGetPositiveHeadBucketResponse));

        RestApiMetadata wasabiMeta = _programMetadata.RestApiMetadataSet
            .TryGetValueWithKey(setKey).ToReferenceTypeValueOrThrow();

        string? bucketName = null;

        AmazonS3Client s3Client = AmazonS3Utility
            .GetAmazonS3Client(wasabiMeta, bucketMetaKey, nameof(ShouldGetPositiveHeadBucketResponse),
                t =>
                {
                    var (credentialsProfileName, bN, region, uriRoot) = t;

                    bucketName = bN;

                    logger.LogDebug("{Name}: {Value}", nameof(credentialsProfileName), credentialsProfileName);
                    logger.LogDebug("{Name}: {Value}", nameof(region), region);
                    logger.LogDebug("{Name}: {Value}", nameof(bucketName), bucketName);
                    logger.LogDebug("{Name}: {Value}", nameof(uriRoot), uriRoot);
                    
                }, logger)
            .ToReferenceTypeValueOrThrow();

        DeleteObjectRequest request = new()
        {
            BucketName = bucketName,
            Key = bucketKey
        };

        //act:
        var actual = await s3Client.DeleteObjectAsync(request).ConfigureAwait(false);

        //assert:
        Assert.Equal(HttpStatusCode.NoContent, actual.HttpStatusCode);
    }

    [SkippableTheory]
    [ProjectDirectoryData("Wasabi", "studio-public-region", "songhay/studio.rss.xml")]
    public async Task ShouldDownloadFile(DirectoryInfo projectInfo, string setKey, string bucketMetaKey, string bucketKey)
    {
        Skip.If(string.IsNullOrWhiteSpace(SettingsPath));

        //arrange:
        ILogger logger = _loggerProvider.CreateLogger(nameof(ShouldGetPositiveHeadBucketResponse));

        RestApiMetadata wasabiMeta = _programMetadata.RestApiMetadataSet
            .TryGetValueWithKey(setKey).ToReferenceTypeValueOrThrow();

        string? bucketName = null;

        AmazonS3Client s3Client = AmazonS3Utility
            .GetAmazonS3Client(wasabiMeta, bucketMetaKey, nameof(ShouldGetPositiveHeadBucketResponse),
                t =>
                {
                    var (credentialsProfileName, bN, region, uriRoot) = t;

                    bucketName = bN;

                    logger.LogDebug("{Name}: {Value}", nameof(credentialsProfileName), credentialsProfileName);
                    logger.LogDebug("{Name}: {Value}", nameof(region), region);
                    logger.LogDebug("{Name}: {Value}", nameof(bucketName), bucketName);
                    logger.LogDebug("{Name}: {Value}", nameof(uriRoot), uriRoot);
                    
                }, logger)
            .ToReferenceTypeValueOrThrow();

        GetObjectRequest request = new() { BucketName = bucketName, Key = bucketKey };

        //act:
        using GetObjectResponse actual = await s3Client.GetObjectAsync(request).ConfigureAwait(false);

        //assert:
        Assert.Equal(HttpStatusCode.OK, actual.HttpStatusCode);
        Assert.True(actual.ContentLength > 0,
            $"{nameof(GetObjectResponse)}.{nameof(GetObjectResponse.ContentLength)} should be greater than zero!");

        //archive:
        string path = projectInfo.ToCombinedPath($"content/xml/{actual.Key.Split('/').Last()}");
        logger.LogDebug("Writing response to `{Path}`...", path);

        await actual.WriteResponseStreamToFileAsync(path, append: false, cancellationToken: CancellationToken.None);
    }

    [SkippableTheory]
    [InlineData("Wasabi", "studio-public-region", "songhay/studio.rss.xml")]
    public async Task ShouldDownloadString(string setKey, string bucketMetaKey, string bucketKey)
    {
        Skip.If(string.IsNullOrWhiteSpace(SettingsPath));

        //arrange:
        ILogger logger = _loggerProvider.CreateLogger(nameof(ShouldGetPositiveHeadBucketResponse));

        RestApiMetadata wasabiMeta = _programMetadata.RestApiMetadataSet
            .TryGetValueWithKey(setKey).ToReferenceTypeValueOrThrow();

        string? bucketName = null;

        AmazonS3Client s3Client = AmazonS3Utility
            .GetAmazonS3Client(wasabiMeta, bucketMetaKey, nameof(ShouldGetPositiveHeadBucketResponse),
                t =>
                {
                    var (credentialsProfileName, bN, region, uriRoot) = t;

                    bucketName = bN;

                    logger.LogDebug("{Name}: {Value}", nameof(credentialsProfileName), credentialsProfileName);
                    logger.LogDebug("{Name}: {Value}", nameof(region), region);
                    logger.LogDebug("{Name}: {Value}", nameof(bucketName), bucketName);
                    logger.LogDebug("{Name}: {Value}", nameof(uriRoot), uriRoot);
                    
                }, logger)
            .ToReferenceTypeValueOrThrow();

        GetObjectRequest request = new() { BucketName = bucketName, Key = bucketKey };

        //act:
        using GetObjectResponse actual = await s3Client.GetObjectAsync(request).ConfigureAwait(false);

        //assert:
        Assert.Equal(HttpStatusCode.OK, actual.HttpStatusCode);
        Assert.True(actual.ContentLength > 0,
            $"{nameof(GetObjectResponse)}.{nameof(GetObjectResponse.ContentLength)} should be greater than zero!");

        logger.LogInformation("Downloaded: {S}", await actual.ResponseStream.ReadStreamAsStringAsync());
    }

    [SkippableTheory]
    [InlineData("Wasabi", "studio-public-region")]
    public async Task ShouldGetPositiveHeadBucketResponse(string setKey, string bucketMetaKey)
    {
        Skip.If(string.IsNullOrWhiteSpace(SettingsPath));

        //arrange:
        ILogger logger = _loggerProvider.CreateLogger(nameof(ShouldGetPositiveHeadBucketResponse));

        RestApiMetadata wasabiMeta = _programMetadata.RestApiMetadataSet
            .TryGetValueWithKey(setKey).ToReferenceTypeValueOrThrow();

        string? bucketName = null;

        AmazonS3Client s3Client = AmazonS3Utility
            .GetAmazonS3Client(wasabiMeta, bucketMetaKey, nameof(ShouldGetPositiveHeadBucketResponse),
                t =>
                {
                    var (credentialsProfileName, bN, region, uriRoot) = t;

                    bucketName = bN;

                    logger.LogDebug("{Name}: {Value}", nameof(credentialsProfileName), credentialsProfileName);
                    logger.LogDebug("{Name}: {Value}", nameof(region), region);
                    logger.LogDebug("{Name}: {Value}", nameof(bucketName), bucketName);
                    logger.LogDebug("{Name}: {Value}", nameof(uriRoot), uriRoot);
                    
                }, logger)
            .ToReferenceTypeValueOrThrow();

        HeadBucketRequest request = new() { BucketName = bucketName };

        //act:
        HeadBucketResponse actual = await s3Client.HeadBucketAsync(request).ConfigureAwait(false);

        //assert:
        Assert.Equal(HttpStatusCode.OK, actual.HttpStatusCode);
    }

    [SkippableTheory]
    [InlineData("Wasabi", "studio-public-region")]
    public async Task ShouldListBucketObjects(string setKey, string bucketMetaKey)
    {
        Skip.If(string.IsNullOrWhiteSpace(SettingsPath));

        //arrange:
        ILogger logger = _loggerProvider.CreateLogger(nameof(ShouldGetPositiveHeadBucketResponse));

        RestApiMetadata wasabiMeta = _programMetadata.RestApiMetadataSet
            .TryGetValueWithKey(setKey).ToReferenceTypeValueOrThrow();

        string? bucketName = null;

        AmazonS3Client s3Client = AmazonS3Utility
            .GetAmazonS3Client(wasabiMeta, bucketMetaKey, nameof(ShouldGetPositiveHeadBucketResponse),
                t =>
                {
                    var (credentialsProfileName, bN, region, uriRoot) = t;

                    bucketName = bN;

                    logger.LogDebug("{Name}: {Value}", nameof(credentialsProfileName), credentialsProfileName);
                    logger.LogDebug("{Name}: {Value}", nameof(region), region);
                    logger.LogDebug("{Name}: {Value}", nameof(bucketName), bucketName);
                    logger.LogDebug("{Name}: {Value}", nameof(uriRoot), uriRoot);
                    
                }, logger)
            .ToReferenceTypeValueOrThrow();

        ListObjectsRequest request = new() { BucketName = bucketName };

        //act:
        ListObjectsResponse actual = await s3Client.ListObjectsAsync(request).ConfigureAwait(false);

        //assert:
        Assert.Equal(HttpStatusCode.OK, actual.HttpStatusCode);
        Assert.NotEmpty(actual.S3Objects);
        foreach (S3Object s3Object in actual.S3Objects)
        {
            logger.LogDebug("{ObjectName}.{PropertyName}: {Value}", nameof(S3Object), nameof(S3Object.Key), s3Object.Key);
        }
    }

    [SkippableTheory]
    [InlineData("Wasabi", "b-roll-player-video-region")]
    public async Task ShouldListBucketObjectsWithPagination(string setKey, string bucketMetaKey)
    {
        Skip.If(string.IsNullOrWhiteSpace(SettingsPath));

        //arrange:
        ILogger logger = _loggerProvider.CreateLogger(nameof(ShouldGetPositiveHeadBucketResponse));

        RestApiMetadata wasabiMeta = _programMetadata.RestApiMetadataSet
            .TryGetValueWithKey(setKey).ToReferenceTypeValueOrThrow();

        string? bucketName = null;

        AmazonS3Client s3Client = AmazonS3Utility
            .GetAmazonS3Client(wasabiMeta, bucketMetaKey, nameof(ShouldGetPositiveHeadBucketResponse),
                t =>
                {
                    var (credentialsProfileName, bN, region, uriRoot) = t;

                    bucketName = bN;

                    logger.LogDebug("{Name}: {Value}", nameof(credentialsProfileName), credentialsProfileName);
                    logger.LogDebug("{Name}: {Value}", nameof(region), region);
                    logger.LogDebug("{Name}: {Value}", nameof(bucketName), bucketName);
                    logger.LogDebug("{Name}: {Value}", nameof(uriRoot), uriRoot);
                    
                }, logger)
            .ToReferenceTypeValueOrThrow();

        ListObjectsV2Request request = new()
        {
            BucketName = bucketName,
            Prefix = string.Empty,
            MaxKeys = 10
        };

        //act:
        IReadOnlyCollection<S3Object> actual = await AmazonS3Utility.CollectS3ObjectsFromPaginationAsync(s3Client, request, logger);

        //assert:
        Assert.NotEmpty(actual);

        string json = JsonSerializer.Serialize(actual);

        logger.LogInformation(json);
    }

    [SkippableTheory]
    [ProjectDirectoryData("Wasabi", "studio-public-region", "songhay/feedly.opml", "content/xml/feedly.opml")]
    public async Task ShouldUploadFile(DirectoryInfo projectInfo, string setKey, string bucketMetaKey, string bucketKey, string localPath)
    {
        Skip.If(string.IsNullOrWhiteSpace(SettingsPath));

        //arrange:
        ILogger logger = _loggerProvider.CreateLogger(nameof(ShouldGetPositiveHeadBucketResponse));

        RestApiMetadata wasabiMeta = _programMetadata.RestApiMetadataSet
            .TryGetValueWithKey(setKey).ToReferenceTypeValueOrThrow();

        string? bucketName = null;

        AmazonS3Client s3Client = AmazonS3Utility
            .GetAmazonS3Client(wasabiMeta, bucketMetaKey, nameof(ShouldGetPositiveHeadBucketResponse),
                t =>
                {
                    var (credentialsProfileName, bN, region, uriRoot) = t;

                    bucketName = bN;

                    logger.LogDebug("{Name}: {Value}", nameof(credentialsProfileName), credentialsProfileName);
                    logger.LogDebug("{Name}: {Value}", nameof(region), region);
                    logger.LogDebug("{Name}: {Value}", nameof(bucketName), bucketName);
                    logger.LogDebug("{Name}: {Value}", nameof(uriRoot), uriRoot);
                    
                }, logger)
            .ToReferenceTypeValueOrThrow();

        PutObjectRequest request = new()
        {
            BucketName = bucketName,
            Key = bucketKey,
            FilePath = projectInfo.ToCombinedPath(localPath),
            ContentType = MimeTypes.ApplicationXml
        };

        //act:
        PutObjectResponse actual = await s3Client.PutObjectAsync(request).ConfigureAwait(false);

        //assert:
        Assert.Equal(HttpStatusCode.OK, actual.HttpStatusCode);
    }

    [SkippableTheory]
    [ProjectDirectoryData("Wasabi", "studio-public-region", "songhay/feedly-from-string.opml", "content/xml/feedly.opml")]
    public async Task ShouldUploadString(DirectoryInfo projectInfo, string setKey, string bucketMetaKey, string bucketKey, string localPath)
    {
        Skip.If(string.IsNullOrWhiteSpace(SettingsPath));

        //arrange:
        ILogger logger = _loggerProvider.CreateLogger(nameof(ShouldGetPositiveHeadBucketResponse));

        RestApiMetadata wasabiMeta = _programMetadata.RestApiMetadataSet
            .TryGetValueWithKey(setKey).ToReferenceTypeValueOrThrow();

        string? bucketName = null;

        AmazonS3Client s3Client = AmazonS3Utility
            .GetAmazonS3Client(wasabiMeta, bucketMetaKey, nameof(ShouldGetPositiveHeadBucketResponse),
                t =>
                {
                    var (credentialsProfileName, bN, region, uriRoot) = t;

                    bucketName = bN;

                    logger.LogDebug("{Name}: {Value}", nameof(credentialsProfileName), credentialsProfileName);
                    logger.LogDebug("{Name}: {Value}", nameof(region), region);
                    logger.LogDebug("{Name}: {Value}", nameof(bucketName), bucketName);
                    logger.LogDebug("{Name}: {Value}", nameof(uriRoot), uriRoot);
                    
                }, logger)
            .ToReferenceTypeValueOrThrow();

        PutObjectRequest request = new()
        {
            BucketName = bucketName,
            Key = bucketKey,
            ContentBody = await File.ReadAllTextAsync(projectInfo.ToCombinedPath(localPath)),
            ContentType = MimeTypes.ApplicationXml
        };

        //act:
        PutObjectResponse actual = await s3Client.PutObjectAsync(request).ConfigureAwait(false);

        //assert:
        Assert.Equal(HttpStatusCode.OK, actual.HttpStatusCode);
    }

    private static readonly string? SettingsPath = Environment.GetEnvironmentVariable("SONGHAY_APP_SETTINGS_PATH");

    private readonly ProgramMetadata _programMetadata;
    private readonly XUnitLoggerProvider _loggerProvider;
}
