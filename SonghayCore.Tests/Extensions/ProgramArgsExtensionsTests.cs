using Songhay.Models;

namespace Songhay.Tests.Extensions;

public class ProgramArgsExtensionsTests
{
    public ProgramArgsExtensionsTests(ITestOutputHelper helper)
    {
        _testOutputHelper = helper;
    }

    [Theory]
    [InlineData("../../../json", "configuration.json")]
    public void ShouldLoadSettings(string basePath, string configFile)
    {
        basePath = ProgramAssemblyUtility
            .GetPathFromAssembly(GetType().Assembly, basePath);

        var args = new ProgramArgs(new[]
        {
            ProgramArgs.BasePathRequired,
            ProgramArgs.BasePath, basePath,
            ProgramArgs.SettingsFile, configFile,
        });

        Assert.Equal(args.GetBasePathValue(), basePath);
        Assert.Equal(args.GetSettingsFilePath(), configFile);

        var json = args.GetSettings();
        Assert.False(string.IsNullOrWhiteSpace(json));

        _testOutputHelper.WriteLine(json);
    }

    [Theory]
    [InlineData("../../../json", "input.json")]
    [InlineData(null, "../../../json/input.json")]
    [InlineData(null, @"{ ""arg1"": 1, ""arg2"": 2, ""arg3"": 3 }")]
    public void ShouldLoadInput(string? basePath, string input)
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

        var args = input.EndsWith(".json") ?
            new ProgramArgs(new[]
            {
                ProgramArgs.BasePathRequired,
                ProgramArgs.BasePath, basePath,
                ProgramArgs.InputFile, input,
            })
            :
            new ProgramArgs(new[]
            {
                ProgramArgs.BasePathRequired,
                ProgramArgs.BasePath, basePath,
                ProgramArgs.InputString, input,
            });

        input = args.GetStringInput();

        Assert.False(string.IsNullOrWhiteSpace(input));

        _testOutputHelper.WriteLine(input);
    }

    [Fact]
    public void WithDefaultHelpText_Test()
    {
        var args = new ProgramArgs(new [] { string.Empty });
        var helpText = args.WithDefaultHelpText().ToHelpDisplayText();
        Assert.False(string.IsNullOrWhiteSpace(helpText));
        _testOutputHelper.WriteLine(helpText);
    }

    [Theory]
    [InlineData("../../../json", "output.json")]
    [InlineData(null, "../../../json/output.json")]
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

        var args = string.IsNullOrWhiteSpace(basePath) ?
                new ProgramArgs(new[]
                {
                    ProgramArgs.OutputFile, outputFile,
                })
                :
                new ProgramArgs(new[]
                {
                    ProgramArgs.BasePathRequired,
                    ProgramArgs.BasePath, basePath,
                    ProgramArgs.OutputFile, outputFile,
                    ProgramArgs.OutputUnderBasePath,
                })
            ;

        var anon = new { modificationDate = DateTime.Now };

        args.WriteOutputToFile(JsonSerializer.Serialize(anon));

        var actual = args.GetOutputPath().ToReferenceTypeValueOrThrow();

        _testOutputHelper.WriteLine($"output path: {actual}");

        _testOutputHelper.WriteLine($"file content: {File.ReadAllText(actual)}");
    }

    readonly ITestOutputHelper _testOutputHelper;
}