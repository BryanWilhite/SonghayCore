using Songhay.Abstractions;
using Songhay.S3.Activities;
using Songhay.S3.Extensions;
using Songhay.S3.Hosting;

namespace Songhay.S3.Tests.Models;

public class AmazonS3ActivityGroupTests(ITestOutputHelper testOutputHelper)
{
    [SkippableTheory]
    [InlineData("Wasabi", "b-roll-player-video-region", "youtube-channels")]
    public async Task ShouldListBucketObjectsWithPaginationAndFiltering(string setKey, string bucketMetaKey, string? bucketKeyPrefix)
    {
        const bool shouldSkip = false;

        Skip.If(shouldSkip);

        //arrange:
        ILogger logger = _loggerProvider.CreateLogger(nameof(ShouldListBucketObjectsWithPaginationAndFiltering));

        IConfiguration configuration = new ConfigurationBuilder()
            .AddConventionalJsonFile()
            .Build();

        IServiceProvider provider = new ServiceCollection()
            .AddSingleton(configuration)
            .AddLogging(builder => builder.AddProvider(_loggerProvider))
            .AddProgramMetadata(configuration)
            .AddS3HostedService<AmazonS3Service>()
            .BuildServiceProvider();

        IActivityKeyedTaskGroup group = provider.GetRequiredService<IActivityKeyedTaskGroup>();

        //act:
        string? actual = await group.InvokeActivityAsync(nameof(AmazonS3ListBucketObjectsWithPaginationActivity), setKey, bucketMetaKey, bucketKeyPrefix);

        //assert:
        Assert.NotNull(actual);

        logger.LogInformation(actual);
    }

    private readonly XUnitLoggerProvider _loggerProvider = new(testOutputHelper);
}
