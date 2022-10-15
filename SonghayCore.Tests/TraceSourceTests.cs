using Microsoft.Extensions.Configuration;
using Songhay.Diagnostics;
using Songhay.Models;

namespace Songhay.Tests;

public class TraceSourceTests
{
    static TraceSourceTests()
    {
        Console.WriteLine("Loading configuration...");
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("./appsettings.json");

        Console.WriteLine("Building configuration...");
        var configuration = builder.Build();

        TraceSources.ConfiguredTraceSourceName = configuration[DeploymentEnvironment.DefaultTraceSourceNameConfigurationKey];

        TraceSource = TraceSources
            .Instance
            .GetConfiguredTraceSource()
            .WithSourceLevels()
            .ToReferenceTypeValueOrThrow();
        NullTraceSource = TraceSources
            .Instance
            .GetConfiguredTraceSource(configuration, "wha?")
            .WithSourceLevels();
        OtherTraceSource = TraceSources
            .Instance
            .GetConfiguredTraceSource(configuration, "other.TraceSourceName")
            .WithSourceLevels()
            .ToReferenceTypeValueOrThrow();
    }

    static readonly TraceSource TraceSource;
    static readonly TraceSource? NullTraceSource;
    static readonly TraceSource OtherTraceSource;

    public TraceSourceTests(ITestOutputHelper helper)
    {
        _testOutputHelper = helper;
    }

    [Fact]
    public void ShouldHaveConfiguredTraceSources()
    {
        Assert.NotNull(TraceSource);
        Assert.NotNull(OtherTraceSource);

        using var writer = new StringWriter();
        using var listener = new TextWriterTraceListener(writer);

        TraceSource.Listeners.Add(listener);
        OtherTraceSource.Listeners.Add(listener);

        TraceSource.WriteLine("info!");
        OtherTraceSource.WriteLine("other info!");

        TraceSource.TraceVerbose("verbose!");
        OtherTraceSource.TraceVerbose("other verbose!");

        TraceSource.TraceError("warn!");
        OtherTraceSource.TraceError("other warn!");

        TraceSource.TraceError("err!");
        OtherTraceSource.TraceError("other err!");

        listener.Flush();
        _testOutputHelper.WriteLine(writer.ToString());
    }

    [Fact]
    public void ShouldNotHaveConfiguredTraceSource()
    {
        Assert.Null(NullTraceSource);

        NullTraceSource.WriteLine("info!");
        NullTraceSource.TraceVerbose("info!");
        NullTraceSource.TraceWarning("info!");
        NullTraceSource.TraceError("info!");
    }

    readonly ITestOutputHelper _testOutputHelper;
}
