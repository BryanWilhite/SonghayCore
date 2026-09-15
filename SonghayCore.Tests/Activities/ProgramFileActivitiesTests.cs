using System.Net;
using Songhay.Activities;
using Songhay.Models;

namespace Songhay.Tests.Activities;

public class ProgramFileActivitiesTests(ITestOutputHelper testOutputHelper)
{
    [SkippableTheory]
    [ProjectDirectoryData("storage-mirror", "one/new.json")]
    public async Task ShouldDeleteFile(DirectoryInfo projectInfo, string? setKey, string? bucketKeyOrPrefix)
    {
        const bool shouldSkip = true;

        Skip.If(shouldSkip);

        //arrange:
        ILogger<ProgramFileDeleteActivity> logger = _xUnitLoggerProvider
            .GenerateLogger<ProgramFileDeleteActivity>().ToReferenceTypeValueOrThrow();
        string path = projectInfo.ToCombinedPath(setKey);
        StorageActivityInput input = new(path, null, bucketKeyOrPrefix);
        ProgramFileDeleteActivity activity = new(logger);

        //act:
        var actual = await activity.StartAsync(input, CancellationToken.None);

        //assert:
        Assert.NotNull(actual);
        Assert.Equal(HttpStatusCode.NoContent, actual.HttpStatusCode);
    }

    [Theory]
    [ProjectDirectoryData("storage-mirror")]
    public async Task ShouldListStorageMirror(DirectoryInfo projectInfo, string? setKey)
    {
        //arrange:
        ILogger<ProgramFileListActivity> logger = _xUnitLoggerProvider
            .GenerateLogger<ProgramFileListActivity>().ToReferenceTypeValueOrThrow();
        string path = projectInfo.ToCombinedPath(setKey);
        StorageActivityInput input = new(path, null, null);
        ProgramFileListActivity activity = new(logger);

        //act:
        var actual = await activity.StartAsync(input, CancellationToken.None);

        //assert:
        Assert.NotNull(actual);
        Assert.Equal(HttpStatusCode.OK, actual.HttpStatusCode);
        Assert.NotEmpty(actual.Content ?? []);

        string json = JsonSerializer.Serialize(actual.Content, JsonSerializerOptionsUtility.JsonSerializerOptionsForIndentation);
        testOutputHelper.WriteLine(json);
    }

    [Theory]
    [ProjectDirectoryData("storage-mirror", "four")]
    public async Task ShouldListStorageMirrorWithFilter(DirectoryInfo projectInfo, string? setKey, string? bucketKeyOrPrefix)
    {
        //arrange:
        ILogger<ProgramFileListActivity> logger = _xUnitLoggerProvider
            .GenerateLogger<ProgramFileListActivity>().ToReferenceTypeValueOrThrow();
        string path = projectInfo.ToCombinedPath(setKey);
        StorageActivityInput input = new(path, null, bucketKeyOrPrefix);
        ProgramFileListActivity activity = new(logger);

        //act:
        var actual = await activity.StartAsync(input, CancellationToken.None);

        //assert:
        Assert.NotNull(actual);
        Assert.Equal(HttpStatusCode.OK, actual.HttpStatusCode);
        Assert.NotEmpty(actual.Content ?? []);

        string json = JsonSerializer.Serialize(actual.Content, JsonSerializerOptionsUtility.JsonSerializerOptionsForIndentation);
        testOutputHelper.WriteLine(json);
    }

    [Theory]
    [ProjectDirectoryData("storage-mirror", "zed")]
    public async Task ShouldListStorageMirrorWithFilterEverything(DirectoryInfo projectInfo, string? setKey, string? bucketKeyOrPrefix)
    {
        //arrange:
        ILogger<ProgramFileListActivity> logger = _xUnitLoggerProvider
            .GenerateLogger<ProgramFileListActivity>().ToReferenceTypeValueOrThrow();
        string path = projectInfo.ToCombinedPath(setKey);
        StorageActivityInput input = new(path, null, bucketKeyOrPrefix);
        ProgramFileListActivity activity = new(logger);

        //act:
        var actual = await activity.StartAsync(input, CancellationToken.None);

        //assert:
        Assert.NotNull(actual);
        Assert.Equal(HttpStatusCode.NotFound, actual.HttpStatusCode);
        Assert.Empty(actual.Content!);

        string json = JsonSerializer.Serialize(actual.Content, JsonSerializerOptionsUtility.JsonSerializerOptionsForIndentation);
        testOutputHelper.WriteLine(json);
    }

    [Theory]
    [ProjectDirectoryData("storage-not-mirror")]
    public async Task ShouldNotListStorageMirror(DirectoryInfo projectInfo, string? setKey)
    {
        //arrange:
        ILogger<ProgramFileListActivity> logger = _xUnitLoggerProvider
            .GenerateLogger<ProgramFileListActivity>().ToReferenceTypeValueOrThrow();
        string path = projectInfo.ToCombinedPath(setKey);
        StorageActivityInput input = new(path, null, null);
        ProgramFileListActivity activity = new(logger);

        //act:
        var actual = await activity.StartAsync(input, CancellationToken.None);

        //assert:
        Assert.NotNull(actual);
        Assert.Equal(HttpStatusCode.NotFound, actual.HttpStatusCode);
        Assert.Empty(actual.Content!);
    }

    [Theory]
    [ProjectDirectoryData("storage-mirror", "one/new.json", "{\"new\": 42 }")]
    public async Task ShouldSaveFile(DirectoryInfo projectInfo, string? setKey, string? bucketKeyOrPrefix, string? content)
    {
        //arrange:
        ILogger<ProgramFileSaveActivity> logger = _xUnitLoggerProvider
            .GenerateLogger<ProgramFileSaveActivity>().ToReferenceTypeValueOrThrow();
        string path = projectInfo.ToCombinedPath(setKey);
        StorageActivityInput<string?> input = new(path, null, bucketKeyOrPrefix, content, null);
        ProgramFileSaveActivity activity = new(logger);

        //act:
        var actual = await activity.StartAsync(input, CancellationToken.None);

        //assert:
        Assert.NotNull(actual);
        Assert.Equal(HttpStatusCode.OK, actual.HttpStatusCode);
    }

    private readonly XUnitLoggerProvider _xUnitLoggerProvider = new(testOutputHelper);
}
