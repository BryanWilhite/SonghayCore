using Microsoft.Extensions.Configuration;
using Songhay.Diagnostics;
using Songhay.Models;

namespace Songhay.Tests.Diagnostics;

public class TraceSourcesTests
{
    public TraceSourcesTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void GetConfiguredTraceSourceName_Test()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("./appsettings.json");

        var configuration = builder.Build();
        var name = configuration[DeploymentEnvironment.DefaultTraceSourceNameConfigurationKey];
        Assert.False(string.IsNullOrWhiteSpace(name), "The expected configuration trace source name is not here.");
        _testOutputHelper.WriteLine($"configuration trace source name: {name}");

        TraceSources.ConfiguredTraceSourceName = name;
        Assert.True(TraceSources.IsConfiguredTraceSourceNameLoaded, $"The expected {nameof(TraceSources)} state is not here.");

        var traceSource = TraceSources.Instance.GetTraceSourceFromConfiguredName();

        using (var writer = new StringWriter())
        using (var listener = new TextWriterTraceListener(writer))
        {
            traceSource.ToReferenceTypeValueOrThrow().Listeners.Add(listener);

            _testOutputHelper.WriteLine($"instantiating {nameof(MyClass)}...");
            var mine = new MyClass();
            Assert.True(mine.GetConfiguredTraceSourceName() == name, "The expected configured configuration trace source name is not here.");

            listener.Flush();
            _testOutputHelper.WriteLine(writer.ToString());
        }
    }

    readonly ITestOutputHelper _testOutputHelper;
}
