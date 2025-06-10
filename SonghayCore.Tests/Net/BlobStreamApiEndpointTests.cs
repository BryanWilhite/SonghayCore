using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Polly;
using Songhay.Models;
using Songhay.Net;

namespace Songhay.Tests.Net;

public class BlobStreamApiEndpointTests(ITestOutputHelper helper)
{
    /// <remarks>
    /// This test is a combination of:
    ///
    /// - troubleshooting/verifying file names in BLOB storage
    /// - testing <see cref="HttpRequestMessageExtensions.WithAzureStorageHeaders(System.Net.Http.HttpRequestMessage?,System.DateTime,string?,string?,string?)"/>
    /// </remarks>
    [Theory, Trait(TestScalars.XunitCategory, TestScalars.XunitCategoryIntegrationTest)]
    [ProjectDirectoryData("studio-public", "songhay_icon.png")]
    [ProjectDirectoryData("studio-public", "mp3/test 01 - with spaces and hyphen.mp3")]
    [ProjectDirectoryData("studio-public", "mp3/test01 - with Spaces and hyphen.mp3")]
    public async Task ShouldCopyToFileStreamAsync(DirectoryInfo projectDirectoryInfo, string containerName, string blobName)
    {
        //arrange:
        const string playerYouTubeApiMetaKey = "PlayerYouTube";
        const string playerYouTubeApiMetaYtContainerKey = "b-roll-video-container";
        const string claimStorageSetContainer = "storage-set-container";
        const string claimStorageSetEndpointKey = "storage-set-endpoint-key";

        ProgramMetadata programMetadata = GetProgramMetadata();

        var (accountName, accountKey, _, apiVersion) = GetCloudStorageMetadata(
                                                            programMetadata,
                                                            playerYouTubeApiMetaKey,
                                                            claimStorageSetContainer,
                                                            claimStorageSetEndpointKey);

        string extension = blobName.Split('.').Last();
        string outputPath = projectDirectoryInfo
            .FindDirectory("content")
            .FindDirectory(extension)
            .FindDirectory(nameof(BlobStreamApiEndpointTests))
            .FindDirectory(nameof(ShouldCopyToFileStreamAsync))
            .ToCombinedPath(blobName.Split('/').Last());

        helper.WriteLine($"streaming from {containerName} to `{outputPath}`...");

        AzureBlobApiRequestStrategy requestStrategy = new(
            accountName,
            accountKey,
            containerName,
            apiVersion,
            playerYouTubeApiMetaYtContainerKey);

        BlobStreamApiEndpoint endpoint = new(ResiliencePipeline.Empty, "default");

        //act:
        await endpoint.DownloadStreamAsync(requestStrategy, blobName.Replace(" ", "%20"),
            stream => ProgramFileUtility.CopyToFileStream(stream, outputPath));
    }

    private static (string accountName, string accountKey, string containerName, string apiVersion)
        GetCloudStorageMetadata(ProgramMetadata? meta,
            string? restApiMetadataSetKey, string? claimStorageSetContainer,
            string? claimStorageSetEndpointKey)
    {
        RestApiMetadata restApiMetadata = meta.ToRestApiMetadata(restApiMetadataSetKey);

        string containerNameKey = restApiMetadata.ClaimsSet
            .TryGetValueWithKey(claimStorageSetContainer, throwException: true)
            .ToReferenceTypeValueOrThrow();

        string containerName = restApiMetadata.ClaimsSet
            .TryGetValueWithKey(containerNameKey, throwException: true)
            .ToReferenceTypeValueOrThrow();

        string connectionStringKey = restApiMetadata.ClaimsSet
            .TryGetValueWithKey(claimStorageSetEndpointKey, throwException: true)
            .ToReferenceTypeValueOrThrow();

        string connectionString = restApiMetadata.ClaimsSet
            .TryGetValueWithKey(connectionStringKey, throwException: true)
            .ToReferenceTypeValueOrThrow();

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new NullReferenceException("The expected connection string is not here.");
        }

        DbConnectionStringBuilder builder = new() { ConnectionString = connectionString };

        string[] values = ((string[])["AccountName", "AccountKey"]).Select(key =>
        {
            if (!builder.ContainsKey(key))
            {
                throw new NullReferenceException($"The expected connection string key, `{key}`, is not here.");
            }

            string? value = builder[key] as string;

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new NullReferenceException($"The expected connection string value for key, `{key}`, is not here.");
            }

            return value;

        }).ToArray();

        (string accountName, string accountKey, string containerName, string apiVersion) storageMetadata;

        storageMetadata.accountName = values[0];
        storageMetadata.accountKey = values[1];
        storageMetadata.containerName = containerName;
        storageMetadata.apiVersion = "2019-12-12";

        return storageMetadata;
    }

    private static ProgramMetadata GetProgramMetadata()
    {
        var builder = new ConfigurationBuilder().AddConventionalJsonFile();

        var programMetadata = builder.Build().BindNewInstance<ProgramMetadata>();
        programMetadata.EnsureProgramMetadata();

        return programMetadata;
    }
}
