using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Songhay.Abstractions;
using Songhay.Activities;
using Songhay.Models;

namespace Songhay.Tests.Models;

public class ProgramFileActivityGroupTests(ITestOutputHelper testOutputHelper)
{
    [SkippableTheory]
    [ProjectDirectoryData("s3-buckets/wasabi/b-roll-player-video", "youtube-channels")]
    public async Task ShouldListBucketObjectsWithPaginationAndFiltering(DirectoryInfo projectInfo, string setKey, string? bucketKeyPrefix)
    {
        //arrange:
        setKey = projectInfo.Parent!.Parent.ToCombinedPath(setKey);

        Skip.IfNot(Directory.Exists(setKey), "This test requires the Songhay Studio environment.");

        IServiceProvider provider = new ServiceCollection()
            .AddLogging(builder => builder.AddProvider(_loggerProvider))
            .AddActivityKeyedTaskGroup<ProgramFileActivityGroup>()
            .BuildServiceProvider();

        IActivityKeyedTaskGroup<EndpointResult> group = provider.GetRequiredService<IActivityKeyedTaskGroup<EndpointResult>>();

        //act:
        EndpointResult result = await group.InvokeActivityAsync(
            nameof(ProgramFileListActivity),
            CancellationToken.None,
            setKey, null, bucketKeyPrefix);

        EndpointContentResult<IReadOnlyCollection<StorageObject>> actual = (result as EndpointContentResult<IReadOnlyCollection<StorageObject>>).ToReferenceTypeValueOrThrow();

        //assert:
        Assert.Equal(HttpStatusCode.OK, actual.HttpStatusCode);
        Assert.NotEmpty(actual.Content!);
        Assert.All(actual.Content!, so => testOutputHelper.WriteLine(so.ToString()));
    }

    private readonly XUnitLoggerProvider _loggerProvider = new(testOutputHelper);
}
