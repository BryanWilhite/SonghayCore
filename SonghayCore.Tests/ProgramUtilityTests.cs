using Songhay.Diagnostics;
using Songhay.Models;
using Songhay.Tests.Diagnostics;

namespace Songhay.Tests;

public class ProgramUtilityTests
{
    public ProgramUtilityTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void InitializeTraceSource_Test()
    {
        var configuration = ProgramUtility.LoadConfiguration(Directory.GetCurrentDirectory());

        var name = configuration[DeploymentEnvironment.DefaultTraceSourceNameConfigurationKey];
        Assert.False(string.IsNullOrWhiteSpace(name), "The expected configuration trace source name is not here.");
        _testOutputHelper.WriteLine($"configuration trace source name: {name}");

        TraceSources.ConfiguredTraceSourceName = name;
        Assert.True(TraceSources.IsConfiguredTraceSourceNameLoaded, $"The expected {nameof(TraceSources)} state is not here.");

        using (var writer = new StringWriter())
        using (var listener = new TextWriterTraceListener(writer))
        {
            ProgramUtility.InitializeTraceSource(listener);

            _testOutputHelper.WriteLine($"instantiating {nameof(MyClass)}...");
            var mine = new MyClass();
            Assert.True(mine.GetConfiguredTraceSourceName() == name, "The expected configured configuration trace source name is not here.");

            listener.Flush();
            _testOutputHelper.WriteLine(writer.ToString());
        }
    }

    readonly ITestOutputHelper _testOutputHelper;
}