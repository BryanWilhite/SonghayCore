using System.Text.Json;
using Songhay.Tests.Orderers;

namespace Songhay.Tests;

public class AzureBlobStorageRestApiUtilityTests : OrderedTestBase
{
    public AzureBlobStorageRestApiUtilityTests(ITestOutputHelper testOutputHelper)
    {
        AssertNoXUnitException();

        _testOutputHelper = testOutputHelper;
    }

    static string? GetConnectionStringFromEnvironmentVariable()
    {
        var path = Environment.GetEnvironmentVariable("SONGHAY_APP_SETTINGS_PATH");

        if (string.IsNullOrWhiteSpace(path)) return null;

        var json = File.ReadAllText(path);
        Assert.False(string.IsNullOrWhiteSpace(json));

        using var jDoc = JsonDocument.Parse(json);
        var actual = jDoc.RootElement
            .GetProperty("ProgramMetadata")
            .GetProperty("CloudStorageSet")
            .GetProperty("SonghayCloudStorage")
            .GetProperty("general-purpose-v1")
            .GetString();

        actual.ThrowWhenNullOrWhiteSpace();

        return actual;
    }

    [SkippableFact, TestOrder(ordinal: 0, reason: "verify environment...")]
    public void ShouldGetConnectionStringFromEnvironmentVariable()
    {
        var actual = GetConnectionStringFromEnvironmentVariable();
        Assert.False(string.IsNullOrWhiteSpace(actual));
    }

    [SkippableTheory, TestOrder(ordinal: 1, reason: "upload `hello.json`...")]
    [InlineData(ContainerName, "hello.json", @"{ ""root"": ""hello!"", ""isGreeting"": true }")]
    public async Task UploadBlobAsync_Test(string containerName, string fileName, string content)
    {
        var connectionString = GetConnectionStringFromEnvironmentVariable();
        Skip.If(string.IsNullOrWhiteSpace(connectionString));
        
        await AzureBlobStorageRestApiUtility.UploadBlobAsync(connectionString, containerName, fileName, content);
    }

    [SkippableTheory, TestOrder(ordinal: 2, reason: "list container...")]
    [InlineData(ContainerName)]
    public async Task ListContainerAsync_Test(string containerName)
    {
        var connectionString = GetConnectionStringFromEnvironmentVariable();
        Skip.If(string.IsNullOrWhiteSpace(connectionString));

        var actual = await AzureBlobStorageRestApiUtility.ListContainerAsync(connectionString, containerName);
        Assert.False(string.IsNullOrWhiteSpace(actual));
        _testOutputHelper.WriteLine(actual);
    }

    [SkippableTheory, TestOrder(ordinal: 3, reason: "delete `hello.json`...")]
    [InlineData(ContainerName, "hello.json")]
    public async Task DeleteBlobAsync_Test(string containerName, string fileName)
    {
        var connectionString = GetConnectionStringFromEnvironmentVariable();
        Skip.If(string.IsNullOrWhiteSpace(connectionString));

        await AzureBlobStorageRestApiUtility.DeleteBlobAsync(connectionString, containerName, fileName);
    }

    [SkippableTheory, TestOrder(ordinal: 4, reason: "list container after delete...")]
    [InlineData(ContainerName)]
    public async Task ListContainerAsync_2_Test(string containerName)
    {
        var connectionString = GetConnectionStringFromEnvironmentVariable();
        Skip.If(string.IsNullOrWhiteSpace(connectionString));

        var actual = await AzureBlobStorageRestApiUtility.ListContainerAsync(connectionString, containerName);
        Assert.False(string.IsNullOrWhiteSpace(actual));
        _testOutputHelper.WriteLine(actual);
    }

    [SkippableTheory, TestOrder(ordinal: 5, reason: "download `foo-two.txt`...")]
    [InlineData(ContainerName, "foo-two.txt")]
    public async Task DownloadBlobToStringAsync_Test(string containerName, string fileName)
    {
        var connectionString = GetConnectionStringFromEnvironmentVariable();
        Skip.If(string.IsNullOrWhiteSpace(connectionString));

        var actual = await AzureBlobStorageRestApiUtility.DownloadBlobToStringAsync(connectionString, containerName, fileName);
        Assert.False(string.IsNullOrWhiteSpace(actual));
    }

    const string ContainerName = "integration-test-container";

    readonly ITestOutputHelper _testOutputHelper;
}
