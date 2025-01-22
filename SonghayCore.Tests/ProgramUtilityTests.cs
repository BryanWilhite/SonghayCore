using Microsoft.Extensions.Configuration;
using Songhay.Diagnostics;
using Songhay.Models;
using Songhay.Tests.Diagnostics;

namespace Songhay.Tests;

public class ProgramUtilityTests(ITestOutputHelper testOutputHelper)
{
    [Theory]
    [InlineData("foo", 0, 0, "foo")]
    [InlineData("foo", 1, 2, "  foo")]
    [InlineData("foo", 4, 1, "    foo")]
    public void GetConsoleIndentation_Test(string input, int numberOfSpaces, int indentationLevel, string expectedOutput)
    {
        string indentation = ProgramUtility.GetConsoleIndentation(numberOfSpaces, indentationLevel);
        string actual = $"{indentation}{input}";
        Assert.Equal(expectedOutput, actual);
    }

    [Fact]
    public void InitializeTraceSource_Test()
    {
        IConfigurationRoot configuration = ProgramUtility.LoadConfiguration(Directory.GetCurrentDirectory());

        string? name = configuration[DeploymentEnvironment.DefaultTraceSourceNameConfigurationKey];
        Assert.False(string.IsNullOrWhiteSpace(name), "The expected configuration trace source name is not here.");
        testOutputHelper.WriteLine($"configuration trace source name: {name}");

        TraceSources.ConfiguredTraceSourceName = name;
        Assert.True(TraceSources.IsConfiguredTraceSourceNameLoaded, $"The expected {nameof(TraceSources)} state is not here.");

        using var writer = new StringWriter();
        using var listener = new TextWriterTraceListener(writer);
        ProgramUtility.InitializeTraceSource(listener);

        testOutputHelper.WriteLine($"instantiating {nameof(MyClass)}...");
        var mine = new MyClass();
        Assert.True(mine.GetConfiguredTraceSourceName() == name, "The expected configured configuration trace source name is not here.");

        listener.Flush();
        testOutputHelper.WriteLine(writer.ToString());
    }
}