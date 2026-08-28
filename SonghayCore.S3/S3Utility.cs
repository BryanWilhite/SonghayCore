using Microsoft.Extensions.Logging;

using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.S3.Model;

using Songhay.Extensions;
using Songhay.Models;
using SonghayCore.S3.Extensions;

namespace SonghayCore.S3;

/// <summary>
/// Shared routines for <see cref="Amazon.S3"/>
/// </summary>
public static class S3Utility
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
    /// Returns an instance of <see cref="AmazonS3Client"/>
    /// or <c>null</c> when there is an error
    /// detailed by the <see cref="ILogger"/>
    /// </summary>
    /// <param name="restApiMetadata">the conventional <see cref="RestApiMetadata"/></param>
    /// <param name="bucketMetaKey">a key in the <see cref="RestApiMetadata.ClaimsSet"/></param>
    /// <param name="clientAppId">an optional ID to describe the <see cref="AmazonS3Client"/></param>
    /// <param name="restApiMetadataAction">the action that reveals <see cref="RestApiMetadata"/> as a tuple</param>
    /// <param name="logger">the <see cref="ILogger"/></param>
    public static AmazonS3Client? GetAmazonS3Client(RestApiMetadata? restApiMetadata,
        string? bucketMetaKey, string? clientAppId,
        Action<(string? credentialsProfileName, string? bucketName, string? region, string? uriRoot)>? restApiMetadataAction, ILogger logger)
    {

        var (credentialsProfileName, bucketName, region, uriRoot) = restApiMetadata.ToS3Tuple(bucketMetaKey, logger);

        restApiMetadataAction?.Invoke((credentialsProfileName, bucketName, region, uriRoot));

        AmazonS3Client? s3Client = GetAmazonS3Client(credentialsProfileName, uriRoot, clientAppId, logger);

        return s3Client;
    }

    /// <summary>
    /// Returns an instance of <see cref="AmazonS3Client"/>
    /// or <c>null</c> when there is an error
    /// detailed by the <see cref="ILogger"/>
    /// </summary>
    /// <param name="credentialsProfileName">the AWS credentials profile name</param>
    /// <param name="uriRoot">the base URI of the desired S3 bucket</param>
    /// <param name="clientAppId">the value of <see cref="AmazonS3Config.ClientAppId"/></param>
    /// <param name="logger">the <see cref="ILogger"/></param>
    public static AmazonS3Client? GetAmazonS3Client(string? credentialsProfileName, string? uriRoot, string? clientAppId, ILogger logger)
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
}
