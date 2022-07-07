using Microsoft.Extensions.Configuration;
using Songhay.Diagnostics;
using Songhay.Extensions;
using Songhay.Models;
using Songhay.Tests.Activities;
using System;
using System.Diagnostics;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests.Models;

public class ActivitiesGetterTest
{
    static ActivitiesGetterTest()
    {
        Console.WriteLine("Loading configuration...");
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("./appsettings.json");

        Console.WriteLine("Building configuration...");
        var configuration = builder.Build();

        TraceSources.ConfiguredTraceSourceName = configuration[DeploymentEnvironment.DefaultTraceSourceNameConfigurationKey];

        traceSource = TraceSources.Instance.GetConfiguredTraceSource().WithSourceLevels();
    }

    public ActivitiesGetterTest(ITestOutputHelper helper)
    {
        this._testOutputHelper = helper;
    }

    static readonly TraceSource traceSource;

    [Fact]
    public void ShouldGetActivityFromDefaultName()
    {
        using (var writer = new StringWriter())
        using (var listener = new TextWriterTraceListener(writer))
        {
            traceSource.Listeners.Add(listener);

            var args = new[]
            {
                nameof(GetHelloWorldActivity),
                "--world-name",
                "Saturn"
            };

            var getter = new MyActivitiesGetter(args);
            var activity = getter.GetActivity();
            Assert.NotNull(activity);

            activity.Start(getter.Args);

            listener.Flush();
            this._testOutputHelper.WriteLine(writer.ToString());
        }
    }

    [Fact]
    public void ShouldHandleEmptyArgments()
    {
        try
        {
            var getter = new MyActivitiesGetter(new string[] { });
        }
        catch (ArgumentException ex)
        {
            this._testOutputHelper.WriteLine($"The expected exception: {ex.Message}");
        }
    }

    [Fact]
    public void ShouldHandleNullArgments()
    {
        try
        {
            var getter = new MyActivitiesGetter(null);
        }
        catch (ArgumentNullException ex)
        {
            this._testOutputHelper.WriteLine($"The expected exception: {ex.Message}");
        }
    }

    [Fact]
    public void ShouldShowHelpDisplayText()
    {
        using (var writer = new StringWriter())
        using (var listener = new TextWriterTraceListener(writer))
        {
            traceSource.Listeners.Add(listener);

            var args = new[]
            {
                nameof(GetHelloWorldActivity),
                ProgramArgs.Help
            };

            var getter = new MyActivitiesGetter(args);
            var activity = getter.GetActivity();
            Assert.NotNull(activity);

            if (getter.Args.IsHelpRequest())
                this._testOutputHelper.WriteLine(activity.DisplayHelp(getter.Args));

            activity.Start(getter.Args);

            listener.Flush();
            this._testOutputHelper.WriteLine(writer.ToString());
        }
    }

    readonly ITestOutputHelper _testOutputHelper;
}