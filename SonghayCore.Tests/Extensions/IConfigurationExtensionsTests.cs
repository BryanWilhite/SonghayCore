using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Songhay.Models;

namespace Songhay.Tests.Extensions;

// ReSharper disable once InconsistentNaming
public class IConfigurationExtensionsTests
{
    public IConfigurationExtensionsTests(ITestOutputHelper helper)
    {
        _testOutputHelper = helper;
    }

    [Theory]
    [InlineData("../../../json", "configuration.json")]
    public void ShouldLoadSettings(string basePath, string configFile)
    {
        basePath = ProgramAssemblyUtility
            .GetPathFromAssembly(GetType().Assembly, basePath);

        _testOutputHelper.WriteLine($"{nameof(basePath)}: `{basePath}`");
        _testOutputHelper.WriteLine($"{nameof(configFile)}: `{configFile}`");

        string[] args = {
            ConsoleArgsScalars.BaseDirectoryRequired, ConsoleArgsScalars.FlagSpacer,
            ConsoleArgsScalars.BaseDirectory, basePath,
            ConsoleArgsScalars.SettingsFile, configFile,
        };

        IConfiguration? configuration = null;
        IHostBuilder builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureAppConfiguration((hostingContext, configBuilder) =>
        {
            configuration = hostingContext.Configuration;

            string path = configuration.GetSettingsFilePath();

            configBuilder.AddJsonFile(path, optional: false);
        });

        IHost _ = builder.Build();
        // FUNKYKB: `IHost.Run` cannot be called here
        // because it will await forever
        // and any JSON files will not load until it is called.

        Assert.NotNull(configuration);
        IReadOnlyCollection<string> keys = configuration.ToKeys();
        Assert.NotEmpty(keys);

        Assert.Equal(basePath, configuration.GetBasePathValue());
        Assert.Equal(configFile, Path.GetFileName(configuration.GetSettingsFilePath()));

        string json = configuration.ReadSettings();
        Assert.False(string.IsNullOrWhiteSpace(json));

        _testOutputHelper.WriteLine(json);
    }

    readonly ITestOutputHelper _testOutputHelper;
}
