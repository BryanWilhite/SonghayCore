using Songhay.Diagnostics;
using Songhay.Extensions;
using Songhay.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests.Extensions;

public class TraceSourceExtensionsTests
{
    static TraceSourceExtensionsTests()
    {
        Console.WriteLine("Loading configuration...");
        var configuration = ProgramUtility.LoadConfiguration(Directory.GetCurrentDirectory());
        TraceSources.ConfiguredTraceSourceName = configuration[DeploymentEnvironment.DefaultTraceSourceNameConfigurationKey];

        traceSource = TraceSources.Instance.GetConfiguredTraceSource().WithSourceLevels();
    }

    static TraceSource traceSource;

    public TraceSourceExtensionsTests(ITestOutputHelper testOutputHelper)
    {
        this._testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void ShouldTraceFromMultipleThreads()
    {
        var configuration = ProgramUtility.LoadConfiguration(Directory.GetCurrentDirectory());

        TraceSources.ConfiguredTraceSourceName = configuration[DeploymentEnvironment.DefaultTraceSourceNameConfigurationKey];

        using (var writer = new StringWriter())
        using (var listener = new TextWriterTraceListener(writer))
        {
            ProgramUtility.InitializeTraceSource(listener);

            var tasks = new[]
            {
                Task.Run(() => traceSource.WriteLine($"Hello world! [thread: {Thread.CurrentThread.ManagedThreadId}]")),
                Task.Run(() =>
                {
                    traceSource.WriteLine($"Hello world! [thread: {Thread.CurrentThread.ManagedThreadId}]");
                    traceSource.TraceError($"Error! [thread: {Thread.CurrentThread.ManagedThreadId}]");
                    traceSource.TraceVerbose($"Was there an error? [thread: {Thread.CurrentThread.ManagedThreadId}]");
                }),
                Task.Run(() => traceSource.WriteLine($"Hello world! [thread: {Thread.CurrentThread.ManagedThreadId}]")),
                Task.Run(() => traceSource.WriteLine($"Hello world! [thread: {Thread.CurrentThread.ManagedThreadId}]")),
            };

            Task.WaitAll(tasks);

            listener.Flush();
            this._testOutputHelper.WriteLine(writer.ToString());
        }
    }

    readonly ITestOutputHelper _testOutputHelper;
}