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
    public void ReadSettings_Test(string baseDirectory, string configFile)
    {
        baseDirectory = ProgramAssemblyUtility
            .GetPathFromAssembly(GetType().Assembly, baseDirectory);

        _testOutputHelper.WriteLine($"{nameof(baseDirectory)}: `{baseDirectory}`");
        _testOutputHelper.WriteLine($"{nameof(configFile)}: `{configFile}`");

        string[] args = {
            ConsoleArgsScalars.BaseDirectoryRequired, ConsoleArgsScalars.FlagSpacer,
            ConsoleArgsScalars.BaseDirectory, baseDirectory,
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
        _testOutputHelper.WriteLine($"{nameof(keys)}:");
        keys.ForEachInEnumerable(k => _testOutputHelper.WriteLine($"    `{k}`"));

        Assert.Equal(baseDirectory, configuration.GetBasePathValue());
        Assert.Equal(configFile, Path.GetFileName(configuration.GetSettingsFilePath()));

        string json = configuration.ReadSettings();
        Assert.False(string.IsNullOrWhiteSpace(json));

        _testOutputHelper.WriteLine(json);
    }

    [Theory]
    [InlineData("../../../json", "input.json")]
    [InlineData(null, "../../../json/input.json")]
    [InlineData(null, @"{ ""arg1"": 1, ""arg2"": 2, ""arg3"": 3 }")]
    public void ReadStringInput_Test(string? baseDirectory, string? input)
    {
        #region arrange input

        bool isJsonFile = input?.EndsWith(".json") == true;

        if (isJsonFile && string.IsNullOrWhiteSpace(baseDirectory))
        {
            input = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, input);
        }
        else if(!string.IsNullOrWhiteSpace(baseDirectory))
        {
            baseDirectory = ProgramAssemblyUtility.GetPathFromAssembly(GetType().Assembly, baseDirectory);
        }

        #endregion

        #region arrange args

        string?[] args = isJsonFile ?
            new[]
            {
                ConsoleArgsScalars.BaseDirectoryRequired, ConsoleArgsScalars.FlagSpacer,
                ConsoleArgsScalars.BaseDirectory, baseDirectory,
                ConsoleArgsScalars.InputFile, input,
            }
            :
            new[]
            {
                ConsoleArgsScalars.BaseDirectoryRequired, ConsoleArgsScalars.FlagSpacer,
                ConsoleArgsScalars.BaseDirectory, baseDirectory,
                ConsoleArgsScalars.InputString, input,
            };

        #endregion

        IConfiguration? configuration = null;
        IHostBuilder builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureAppConfiguration((hostingContext, _) => configuration = hostingContext.Configuration);

        IHost _ = builder.Build();
        // FUNKYKB: `IHost.Run` cannot be called here
        // because it will await forever
        // and any JSON files will not load until it is called.

        Assert.NotNull(configuration);
        IReadOnlyCollection<string> keys = configuration.ToKeys();
        Assert.NotEmpty(keys);
        _testOutputHelper.WriteLine($"{nameof(keys)}:");
        keys.ForEachInEnumerable(k => _testOutputHelper.WriteLine($"    `{k}`"));

        input = configuration.ReadStringInput();

        Assert.False(string.IsNullOrWhiteSpace(input));

        _testOutputHelper.WriteLine($"{nameof(input)}:");
        _testOutputHelper.WriteLine(input);
    }

    [Fact]
    public void WithDefaultHelpText_Test()
    {
        IConfiguration? configuration = null;
        IHostBuilder builder = Host.CreateDefaultBuilder(Array.Empty<string>());

        builder.ConfigureAppConfiguration((hostingContext, _) =>
        {
            configuration = hostingContext.Configuration;

            Assert.NotNull(configuration);

            configuration.WithDefaultHelpText();
        });

        IHost _ = builder.Build();
        // FUNKYKB: `IHost.Run` cannot be called here
        // because it will await forever
        // and any JSON files will not load until it is called.

        IReadOnlyCollection<string> keys = configuration.ToKeys();
        Assert.NotEmpty(keys);
        _testOutputHelper.WriteLine($"{nameof(keys)}:");
        keys.ForEachInEnumerable(k => _testOutputHelper.WriteLine($"    `{k}`"));

        string? helpText = configuration.ToHelpDisplayText();
        Assert.False(string.IsNullOrWhiteSpace(helpText));
        _testOutputHelper.WriteLine(helpText);
    }

    [Theory]
    [InlineData("../../../json", "output.json")]
    [InlineData(null, "../../../json/output.json")]
    public void WriteOutputToFile_Test(string? baseDirectory, string outputFile)
    {
        #region arrange input

        if (string.IsNullOrWhiteSpace(baseDirectory))
        {
            outputFile = ProgramAssemblyUtility
                .GetPathFromAssembly(GetType().Assembly, outputFile);
        }
        else
        {
            baseDirectory = ProgramAssemblyUtility
                .GetPathFromAssembly(GetType().Assembly, baseDirectory);
        }

        #endregion

        #region arrange args

        string[] args = string.IsNullOrWhiteSpace(baseDirectory) ?
                new[]
                {
                    ConsoleArgsScalars.OutputFile, outputFile,
                }
                :
                new[]
                {
                    ConsoleArgsScalars.BaseDirectoryRequired, ConsoleArgsScalars.FlagSpacer,
                    ConsoleArgsScalars.BaseDirectory, baseDirectory,
                    ConsoleArgsScalars.OutputFile, outputFile,
                    ConsoleArgsScalars.OutputUnderBasePath, ConsoleArgsScalars.FlagSpacer,
                }
            ;

        #endregion

        IConfiguration? configuration = null;
        IHostBuilder builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureAppConfiguration((hostingContext, _) => configuration = hostingContext.Configuration);

        IHost _ = builder.Build();
        // FUNKYKB: `IHost.Run` cannot be called here
        // because it will await forever
        // and any JSON files will not load until it is called.

        Assert.NotNull(configuration);
        IReadOnlyCollection<string> keys = configuration.ToKeys();
        Assert.NotEmpty(keys);
        _testOutputHelper.WriteLine($"{nameof(keys)}:");
        keys.ForEachInEnumerable(k => _testOutputHelper.WriteLine($"    `{k}`"));

        var anon = new { modificationDate = DateTime.Now };

        configuration.WriteOutputToFile(JsonSerializer.Serialize(anon));

        string actual = configuration.GetOutputPath();

        _testOutputHelper.WriteLine($"output path: {actual}");

        _testOutputHelper.WriteLine($"file content: {File.ReadAllText(actual)}");
    }

    readonly ITestOutputHelper _testOutputHelper;
}
