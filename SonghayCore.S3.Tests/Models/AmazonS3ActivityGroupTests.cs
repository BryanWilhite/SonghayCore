using System.Net;
using Songhay.Abstractions;
using Songhay.Models;
using Songhay.S3.Activities;
using Songhay.S3.Extensions;
using Songhay.S3.Models;

namespace Songhay.S3.Tests.Models;

public class AmazonS3ActivityGroupTests(ITestOutputHelper testOutputHelper)
{
    [SkippableTheory]
    [InlineData("Wasabi1", "b-roll-player-video-region", "youtube-channels")]
    public async Task ShouldListBucketObjectsWithPaginationAndFiltering(string setKey, string bucketMetaKey, string? bucketKeyPrefix)
    {
        const bool shouldSkip = false;

        Skip.If(shouldSkip);

        //arrange:
        IConfiguration configuration = new ConfigurationBuilder()
            .AddConventionalJsonFile()
            .Build();

        IServiceProvider provider = new ServiceCollection()
            .AddSingleton(configuration)
            .AddLogging(builder => builder.AddProvider(_loggerProvider))
            .AddProgramMetadata(configuration)
            .AddActivityKeyedTaskGroup<AmazonS3ActivityGroup>()
            .BuildServiceProvider();

        IActivityKeyedTaskGroup<EndpointResult> group = provider.GetRequiredService<IActivityKeyedTaskGroup<EndpointResult>>();

        //act:
        EndpointResult result = await group.InvokeActivityAsync(
            nameof(AmazonS3ListBucketObjectsWithPaginationActivity),
            CancellationToken.None,
            setKey, bucketMetaKey, bucketKeyPrefix);

        EndpointContentResult<IReadOnlyCollection<StorageObject>> actual = (result as EndpointContentResult<IReadOnlyCollection<StorageObject>>).ToReferenceTypeValueOrThrow();

        //assert:
        Assert.Equal(HttpStatusCode.OK, actual.HttpStatusCode);
        Assert.NotEmpty(actual.Content!);
        Assert.All(actual.Content!, so => testOutputHelper.WriteLine(so.ToString()));
    }

    private readonly XUnitLoggerProvider _loggerProvider = new(testOutputHelper);
}
