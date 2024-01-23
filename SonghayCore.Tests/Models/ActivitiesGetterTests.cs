using Microsoft.Extensions.Configuration;
using Songhay.Diagnostics;
using Songhay.Models;
using Songhay.Tests.Activities;

namespace Songhay.Tests.Models;

public class ActivitiesGetterTests
{
    static ActivitiesGetterTests()
    {
        Console.WriteLine("Loading configuration...");
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("./appsettings.json");

        Console.WriteLine("Building configuration...");
        var configuration = builder.Build();

        TraceSources.ConfiguredTraceSourceName = configuration[DeploymentEnvironment.DefaultTraceSourceNameConfigurationKey];

        TraceSource = TraceSources.Instance.GetConfiguredTraceSource().WithSourceLevels();
    }

    public ActivitiesGetterTests(ITestOutputHelper helper)
    {
        _testOutputHelper = helper;
    }

    static readonly TraceSource? TraceSource;

    [Fact]
    public void ShouldGetActivityFromDefaultName()
    {
        using var writer = new StringWriter();
        using var listener = new TextWriterTraceListener(writer);

        TraceSource?.Listeners.Add(listener);

        var args = new[]
        {
            nameof(GetHelloWorldActivity),
            "--world-name",
            "Saturn"
        };

        var getter = new MyActivitiesGetter(args);
        var activity = getter.GetActivity();
        Assert.NotNull(activity);

        activity?.Start(getter.Args);

        listener.Flush();
        _testOutputHelper.WriteLine(writer.ToString());
    }

    [Fact]
    public void ShouldHandleEmptyArguments()
    {
        try
        {
            _ = new MyActivitiesGetter(new string[] { });
        }
        catch (ArgumentException ex)
        {
            _testOutputHelper.WriteLine($"The expected exception: {ex.Message}");
        }
    }

    [Fact]
    public void ShouldHandleNullArguments()
    {
        try
        {
            _ = new MyActivitiesGetter(null!);
        }
        catch (ArgumentNullException ex)
        {
            _testOutputHelper.WriteLine($"The expected exception: {ex.Message}");
        }
    }

    [Fact]
    public void ShouldShowHelpDisplayText()
    {
        using var writer = new StringWriter();
        using var listener = new TextWriterTraceListener(writer);

        TraceSource?.Listeners.Add(listener);

        var args = new[]
        {
            nameof(GetHelloWorldActivity),
            ProgramArgs.Help
        };

        var getter = new MyActivitiesGetter(args);
        var activity = getter.GetActivity();
        Assert.NotNull(activity);

        if (getter.Args.IsHelpRequest())
            _testOutputHelper.WriteLine(activity?.DisplayHelp(getter.Args));

        activity?.Start(getter.Args);

        listener.Flush();
        _testOutputHelper.WriteLine(writer.ToString());
    }

    readonly ITestOutputHelper _testOutputHelper;
}