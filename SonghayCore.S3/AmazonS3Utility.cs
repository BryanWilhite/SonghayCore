using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;

using Songhay.S3.Extensions;

namespace Songhay.S3;

/// <summary>
/// Shared routines for <see cref="Amazon.S3"/>
/// </summary>
public static class AmazonS3Utility
{
    /// <summary>
    /// Collects the <see cref="S3Object"/> responses
    /// from paginating through a bucker based
    /// on the specified <see cref="ListObjectsV2Request.MaxKeys"/>.
    /// </summary>
    /// <param name="s3Client">the <see cref="AmazonS3Client"/></param>
    /// <param name="request">the <see cref="ListObjectsV2Request"/></param>
    /// <param name="logger">the <see cref="ILogger"/></param>
    public static async Task<IReadOnlyCollection<S3Object>> CollectS3ObjectsFromPaginationAsync(AmazonS3Client? s3Client, ListObjectsV2Request? request, ILogger logger)
    {
        List<S3Object> allS3Objects = [];

        if (s3Client == null)
        {
            logger.LogErrorForMissingData<AmazonS3Client>();

            return allS3Objects;
        }

        if (request == null)
        {
            logger.LogErrorForMissingData<ListObjectsV2Request>();

            return allS3Objects;
        }

        ListObjectsV2Response response;
        do
        {
            response = await s3Client.ListObjectsV2Async(request);

            allS3Objects.AddRange(response.S3Objects);

            request.ContinuationToken = response.NextContinuationToken;

        } while (response.IsTruncated ?? false);

        return allS3Objects;
    }

    /// <summary>
    /// Returns an instance of <see cref="AmazonS3Client"/>,
    /// trying <see cref="GetAmazonS3ClientWithCredentialsChain(RestApiMetadata?, string?, string?, Action{ValueTuple{string?, string?, string?, string?}}?, ILogger)"/> then <see cref="GetAmazonS3ClientWithoutCredentialsChain(RestApiMetadata?, string?, string?, EnvironmentVariableTarget?, Action{ValueTuple{bool, string?, string?, string?}}?, ILogger)"/>,
    /// or <c>null</c> when there is an error
    /// detailed by the <see cref="ILogger"/>
    /// </summary>
    /// <param name="restApiMetadata"></param>
    /// <param name="bucketMetaKey"></param>
    /// <param name="clientAppId"></param>
    /// <param name="environmentVariableTarget"></param>
    /// <param name="s3BucketName">returns the name of the S3 bucket derived from <see cref="RestApiMetadata"/></param>
    /// <param name="logger"></param>
    public static AmazonS3Client? GetAmazonS3Client(
        RestApiMetadata? restApiMetadata,
        string? bucketMetaKey,
        string? clientAppId,
        EnvironmentVariableTarget? environmentVariableTarget,
        out string? s3BucketName,
        ILogger logger)
    {
        s3BucketName = null;

        string? bucketName = null;

        AmazonS3Client? s3Client = AmazonS3Utility
            .GetAmazonS3ClientWithCredentialsChain(
                restApiMetadata,
                bucketMetaKey,
                clientAppId,
                t =>
                {
                    var (credentialsProfileName, bN, region, uriRoot) = t;

                    bucketName = bN;

                    logger.LogDebug("{Name}: {Value}", nameof(credentialsProfileName), credentialsProfileName);
                    logger.LogDebug("{Name}: {Value}", nameof(region), region);
                    logger.LogDebug("{Name}: {Value}", nameof(bucketName), bucketName);
                    logger.LogDebug("{Name}: {Value}", nameof(uriRoot), uriRoot);

                }, logger);

        s3BucketName = bucketName;

        if (s3Client != null)
        {
            return s3Client;
        }

        s3Client = AmazonS3Utility
            .GetAmazonS3ClientWithoutCredentialsChain(
                restApiMetadata,
                bucketMetaKey,
                clientAppId,
                environmentVariableTarget,
                t =>
                {
                    var (areAnySecretsMissing, bN, region, uriRoot) = t;

                    bucketName = bN;

                    logger.LogDebug("{Name}: {Value}", nameof(areAnySecretsMissing), areAnySecretsMissing);
                    logger.LogDebug("{Name}: {Value}", nameof(region), region);
                    logger.LogDebug("{Name}: {Value}", nameof(bucketName), bucketName);
                    logger.LogDebug("{Name}: {Value}", nameof(uriRoot), uriRoot);
                    
                }, logger);

        s3BucketName = bucketName;

        return s3Client;
    }

    /// <summary>
    /// Returns an instance of <see cref="AmazonS3Client"/>
    /// or <c>null</c> when there is an error
    /// detailed by the <see cref="ILogger"/>
    /// </summary>
    /// <param name="restApiMetadata">the conventional <see cref="RestApiMetadata"/></param>
    /// <param name="bucketMetaKey">a key in the <see cref="RestApiMetadata.ClaimsSet"/></param>
    /// <param name="clientAppId">an optional ID to describe the <see cref="AmazonS3Client"/></param>
    /// <param name="restApiMetadataAction">the action that reveals <see cref="RestApiMetadata"/> as a tuple</param>
    /// <param name="logger">the <see cref="ILogger"/></param>
    public static AmazonS3Client? GetAmazonS3ClientWithCredentialsChain(
        RestApiMetadata? restApiMetadata,
        string? bucketMetaKey, string? clientAppId,
        Action<(string? credentialsProfileName, string? bucketName, string? region, string? uriRoot)>? restApiMetadataAction, ILogger logger)
    {

        var (credentialsProfileName, bucketName, region, uriRoot) = restApiMetadata.ToS3Tuple(bucketMetaKey, logger);

        restApiMetadataAction?.Invoke((credentialsProfileName, bucketName, region, uriRoot));

        AmazonS3Client? s3Client = GetAmazonS3ClientWithCredentialsChain(credentialsProfileName, uriRoot, clientAppId, logger);

        return s3Client;
    }

    /// <summary>
    /// Returns an instance of <see cref="AmazonS3Client"/>
    /// or <c>null</c> when there is an error
    /// detailed by the <see cref="ILogger"/>
    /// </summary>
    /// <param name="credentialsProfileName">the AWS credentials profile name</param>
    /// <param name="uriRoot">the base URI of the desired S3 bucket</param>
    /// <param name="clientAppId">the value of <see cref="ClientConfig.ClientAppId"/></param>
    /// <param name="logger">the <see cref="ILogger"/></param>
    public static AmazonS3Client? GetAmazonS3ClientWithCredentialsChain(string? credentialsProfileName, string? uriRoot, string? clientAppId, ILogger logger)
    {
        AmazonS3Config config = new()
        {
            ServiceURL = uriRoot,
            ClientAppId = clientAppId
        };

        CredentialProfileStoreChain chain = new();

        if (!chain.TryGetAWSCredentials(credentialsProfileName, out AWSCredentials awsCredentials))
        {
            logger.LogError("The expected AWS profile, `{Name}`, was not found.", credentialsProfileName);

            return null;
        }

        AmazonS3Client s3Client = new (awsCredentials, config);

        return s3Client;
    }

    /// <summary>
    /// Returns an instance of <see cref="AmazonS3Client"/>
    /// or <c>null</c> when there is an error
    /// detailed by the <see cref="ILogger"/>
    /// </summary>
    /// <param name="restApiMetadata">the conventional <see cref="RestApiMetadata"/></param>
    /// <param name="bucketMetaKey">a key in the <see cref="RestApiMetadata.ClaimsSet"/></param>
    /// <param name="clientAppId">the value of <see cref="ClientConfig.ClientAppId"/></param>
    /// <param name="environmentVariableTarget">
    ///     specifies that <see cref="RestApiMetadata"/> should be written with <see cref="Environment.SetEnvironmentVariable(string, string?, EnvironmentVariableTarget)"/>
    /// </param>
    /// <param name="restApiMetadataAction">the action that reveals <see cref="RestApiMetadata"/> as a tuple</param>
    /// <param name="logger">the <see cref="ILogger"/></param>
    /// <remarks>
    /// There are security risks associated with this method.
    /// See the remarks for <see cref="Songhay.S3.Extensions.RestApiMetadataExtensions.ToS3LessSecureTuple"/>
    /// for details.
    /// </remarks>
    public static AmazonS3Client GetAmazonS3ClientWithoutCredentialsChain(
        RestApiMetadata? restApiMetadata,
        string? bucketMetaKey, string? clientAppId, EnvironmentVariableTarget? environmentVariableTarget,
        Action<(bool areAnySecretsMissing, string? bucketName, string? region, string? uriRoot)>? restApiMetadataAction,
        ILogger logger)
    {

        var (publicKey, privateKey, bucketName, region, uriRoot) = restApiMetadata.ToS3LessSecureTuple(bucketMetaKey, logger);

        bool areAnySecretsMissing = string.IsNullOrWhiteSpace(publicKey) || string.IsNullOrWhiteSpace(privateKey);

        if (environmentVariableTarget != null)
        {
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", publicKey, environmentVariableTarget.Value);
            Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", privateKey, environmentVariableTarget.Value);
            Environment.SetEnvironmentVariable("AWS_DEFAULT_REGION", region, environmentVariableTarget.Value);
            Environment.SetEnvironmentVariable("AWS_ENDPOINT_URL", uriRoot, environmentVariableTarget.Value);
        }

        restApiMetadataAction?.Invoke((areAnySecretsMissing, bucketName, region, uriRoot));

        AmazonS3Config s3Config = new()
        {
            ServiceURL = uriRoot,
            ClientAppId = clientAppId
        };

        AmazonS3Client s3Client = new(s3Config);

        return s3Client;
    }
}
