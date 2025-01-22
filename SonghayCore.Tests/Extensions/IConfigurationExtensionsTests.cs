using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Songhay.Models;

namespace Songhay.Tests.Extensions;

// ReSharper disable once InconsistentNaming
public class IConfigurationExtensionsTests(ITestOutputHelper helper)
{
    [Theory]
    [InlineData(ConsoleArgsScalars.DryRun, true, true, $"{ConsoleArgsScalars.DryRun}=true")]
    [InlineData(ConsoleArgsScalars.DryRun, true, false, $"{ConsoleArgsScalars.DryRun}=false")]
    [InlineData("--my-arg", false, true, "--my-arg", "my-value")]
    [InlineData("--my-other-arg", false, false, "--my-other-arg")]
    [InlineData("--my-other-arg", true, false, "--my-other-arg")] //see https://github.com/BryanWilhite/SonghayCore/issues/177
    public void HasKey_Test(string? key, bool isCommandLineSwitch, bool expected, params string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder().AddCommandLine(args).Build();

        var actual = configuration.HasKey(key, isCommandLineSwitch);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(ConsoleArgsScalars.DryRun, "true")]
    [InlineData($"{ConsoleArgsScalars.DryRun}=true")]
    public async Task ShouldReadCommandLineArgsForDryRunWithHost(params string[] args)
    {
        IConfiguration? configuration = null; 
        IHostBuilder builder = Host.CreateDefaultBuilder(args);
        
        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            configuration = configBuilder.Build();
        });

        using IHost host = builder.Build();
        try
        {
            host.Start();

            var actual = configuration.IsDryRun();
            Assert.True(actual);
        }
        finally
        {
            await host.StopAsync();
        }
    }

    [Theory]
    [InlineData(ConsoleArgsScalars.DryRun, "true")]
    [InlineData($"{ConsoleArgsScalars.DryRun}=true")]
    public void ShouldReadCommandLineArgsForDryRunWithoutHost(params string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder().AddCommandLine(args).Build();
        var actual = configuration.IsDryRun();
        Assert.True(actual);
    }

    [Theory]
    [InlineData("../../../content/json", "configuration.json")]
    public void ReadSettings_Test(string basePath, string configFile)
    {
        basePath = ProgramAssemblyUtility
            .GetPathFromAssembly(GetType().Assembly, basePath);

        string[] args =
        [
            ConsoleArgsScalars.BaseDirectoryRequired, ConsoleArgsScalars.FlagSpacer,
            ConsoleArgsScalars.BaseDirectory, basePath,
            ConsoleArgsScalars.SettingsFile, configFile,
        ];

        IConfiguration configuration = new ConfigurationBuilder().AddCommandLine(args).Build();

        Assert.Equal(configuration.GetCommandLineArgValue(ConsoleArgsScalars.BaseDirectory), basePath);
        Assert.Equal(configuration.GetCommandLineArgValue(ConsoleArgsScalars.SettingsFile), configFile);

        var json = File.ReadAllText(configuration.GetSettingsFilePath());
        Assert.False(string.IsNullOrWhiteSpace(json));

        helper.WriteLine(json);
    }

    [Theory]
    [InlineData("../../../content/json", "input.json")]
    [InlineData(null, "../../../content/json/input.json")]
    [InlineData(null, @"{ ""arg1"": 1, ""arg2"": 2, ""arg3"": 3 }")]
    public void ReadStringInput_Test(string? basePath, string input)
    {
        if (string.IsNullOrWhiteSpace(basePath))
        {
            if (input.EndsWith(".json"))
                input = ProgramAssemblyUtility
                    .GetPathFromAssembly(GetType().Assembly, input);
        }
        else
        {
            basePath = ProgramAssemblyUtility
                .GetPathFromAssembly(GetType().Assembly, basePath);
        }

        string[] args = input.EndsWith(".json") ?
            [
                ConsoleArgsScalars.BaseDirectoryRequired, ConsoleArgsScalars.FlagSpacer,
                ConsoleArgsScalars.BaseDirectory, $"{basePath}",
                ConsoleArgsScalars.InputFile, input,
            ]
            :
            [
                ConsoleArgsScalars.BaseDirectoryRequired, ConsoleArgsScalars.FlagSpacer,
                ConsoleArgsScalars.BaseDirectory, $"{basePath}",
                ConsoleArgsScalars.InputString, input,
            ];

        IConfiguration configuration = new ConfigurationBuilder().AddCommandLine(args).Build();

        input = configuration.ReadStringInput().ToReferenceTypeValueOrThrow();

        Assert.False(string.IsNullOrWhiteSpace(input));

        helper.WriteLine(input);
    }

    [Fact]
    public void WithDefaultHelpText_Test()
    {
        IConfiguration configuration = new ConfigurationBuilder().AddCommandLine([]).Build();
        var helpText = configuration.WithDefaultHelpText().ToHelpDisplayText();
        Assert.False(string.IsNullOrWhiteSpace(helpText));
        helper.WriteLine(helpText);
    }

    [Theory]
    [InlineData("../../../content/json", "output.json")]
    [InlineData(null, "../../../content/json/output.json")]
    public void WriteOutputToFile_Test(string? basePath, string outputFile)
    {
        if (string.IsNullOrWhiteSpace(basePath))
        {
            outputFile = ProgramAssemblyUtility
                .GetPathFromAssembly(GetType().Assembly, outputFile);
        }
        else
        {
            basePath = ProgramAssemblyUtility
                .GetPathFromAssembly(GetType().Assembly, basePath);
        }

        string[] args = string.IsNullOrWhiteSpace(basePath) ?
                [
                    ConsoleArgsScalars.OutputFile, outputFile,
                ]
                :
                [
                    ConsoleArgsScalars.BaseDirectoryRequired, ConsoleArgsScalars.FlagSpacer,
                    ConsoleArgsScalars.BaseDirectory, $"{basePath}",
                    ConsoleArgsScalars.OutputFile, outputFile,
                    ConsoleArgsScalars.OutputUnderBasePath, ConsoleArgsScalars.FlagSpacer,
                ]
            ;

        IConfiguration configuration = new ConfigurationBuilder().AddCommandLine(args).Build();

        var anon = new { modificationDate = DateTime.Now };

        configuration.WriteOutputToFile(JsonSerializer.Serialize(anon));

        var actual = configuration.GetOutputPath().ToReferenceTypeValueOrThrow();

        helper.WriteLine($"output path: {actual}");

        helper.WriteLine($"file content: {File.ReadAllText(actual)}");
    }
}
