using Songhay.Diagnostics;
using Songhay.Models;

namespace Songhay.Tests.Extensions;

public class TraceSourceExtensionsTests(ITestOutputHelper testOutputHelper)
{
    static TraceSourceExtensionsTests()
    {
        Console.WriteLine("Loading configuration...");
        var configuration = ProgramUtility.LoadConfiguration(Directory.GetCurrentDirectory());
        TraceSources.ConfiguredTraceSourceName = configuration[DeploymentEnvironment.DefaultTraceSourceNameConfigurationKey];

        TraceSource = TraceSources.Instance.GetConfiguredTraceSource().WithSourceLevels().ToReferenceTypeValueOrThrow();
    }

    static readonly TraceSource TraceSource;

    [Fact]
    public async Task ShouldTraceFromMultipleThreads()
    {
        var configuration = ProgramUtility.LoadConfiguration(Directory.GetCurrentDirectory());

        TraceSources.ConfiguredTraceSourceName = configuration[DeploymentEnvironment.DefaultTraceSourceNameConfigurationKey];

        using var writer = new StringWriter();
        using var listener = new TextWriterTraceListener(writer);
        ProgramUtility.InitializeTraceSource(listener);

        var tasks = new[]
        {
            Task.Run(() => TraceSource.WriteLine($"Hello world! [thread: {Thread.CurrentThread.ManagedThreadId}]")),
            Task.Run(() =>
            {
                TraceSource.WriteLine($"Hello world! [thread: {Thread.CurrentThread.ManagedThreadId}]");
                TraceSource.TraceError($"Error! [thread: {Thread.CurrentThread.ManagedThreadId}]");
                TraceSource.TraceVerbose($"Was there an error? [thread: {Thread.CurrentThread.ManagedThreadId}]");
            }),
            Task.Run(() => TraceSource.WriteLine($"Hello world! [thread: {Thread.CurrentThread.ManagedThreadId}]")),
            Task.Run(() => TraceSource.WriteLine($"Hello world! [thread: {Thread.CurrentThread.ManagedThreadId}]")),
        };

        await Task.WhenAll(tasks);

        listener.Flush();
        testOutputHelper.WriteLine(writer.ToString());
    }
}