using Songhay.Diagnostics;
using Songhay.Models;
using Songhay.Abstractions;

namespace Songhay.Tests.Extensions;

// ReSharper disable once InconsistentNaming
public class IActivityExtensionsTests
{
    static IActivityExtensionsTests()
    {
        var configuration = ProgramUtility.LoadConfiguration(Directory.GetCurrentDirectory());
        TraceSources.ConfiguredTraceSourceName = configuration[DeploymentEnvironment.DefaultTraceSourceNameConfigurationKey];

        TraceSource = TraceSources.Instance.GetConfiguredTraceSource().WithSourceLevels().ToReferenceTypeValueOrThrow();
    }

    static readonly TraceSource TraceSource;

    public IActivityExtensionsTests(ITestOutputHelper helper)
    {
        _testOutputHelper = helper;
    }

    [Fact]
    public void StartActivity_Test()
    {
        var args = new[] { nameof(MyActivity) };
        var activitiesGetter = new MyActivitiesGetter(args);
        var activity = activitiesGetter.GetActivity();
        Assert.NotNull(activity);

        var log = activity.StartActivity(new ProgramArgs(args), TraceSource);
        Assert.False(string.IsNullOrWhiteSpace(log));
        _testOutputHelper.WriteLine(log);
    }

    [Fact]
    public void StartActivityForOutput_Test()
    {
        var args = new[] { nameof(MyActivityWithOutput) };
        var activitiesGetter = new MyActivitiesGetter(args);
        var activity = activitiesGetter.GetActivity(nameof(MyActivityWithOutput));
        Assert.NotNull(activity);

        var output = activity
            .StartActivityForOutput<ProgramArgs, string>(new ProgramArgs(args), TraceSource);
        Assert.NotNull(output);
        Assert.False(string.IsNullOrWhiteSpace(output.Log));
        Assert.EndsWith(output.Output!, output.Log!.TrimEnd());
        _testOutputHelper.WriteLine(output.Log);
    }

    [Theory]
    [ProjectFileData(typeof(IActivityExtensionsTests),
        "../../../json/StartActivity_StreamWriter_Test_output.log")]
    public void StartActivity_StreamWriter_Test(FileInfo outputInfo)
    {
        var args = new[] { nameof(MyActivity) };
        var activitiesGetter = new MyActivitiesGetter(args);
        var activity = activitiesGetter.GetActivity();
        Assert.NotNull(activity);

        var disposable = new StreamWriter(outputInfo.FullName);

        var log = activity.StartActivity(
            new ProgramArgs(args),
            TraceSource,
            () => disposable,
            flushLog: false);

        Assert.Null(log);
        Assert.Throws<ObjectDisposedException>(() => disposable.WriteLine("foo"));

        log = File.ReadAllText(outputInfo.FullName);
        Assert.False(string.IsNullOrWhiteSpace(log));
        _testOutputHelper.WriteLine(log);
    }

    readonly ITestOutputHelper _testOutputHelper;
}

sealed class MyActivitiesGetter : ActivitiesGetter
{
    public MyActivitiesGetter(string[] args) : base(args)
    {
        LoadActivities(new Dictionary<string, Lazy<IActivity?>>
        {
            {
                nameof(MyActivity),
                new Lazy<IActivity?>(() => new MyActivity())
            },
            {
                nameof(MyActivityWithOutput),
                new Lazy<IActivity?>(() => new MyActivityWithOutput())
            },
        });
    }
}

class MyActivity : IActivity
{
    static MyActivity() => TraceSource =
        TraceSources.Instance.GetConfiguredTraceSource().WithSourceLevels().ToReferenceTypeValueOrThrow();
    static readonly TraceSource TraceSource;

    public string DisplayHelp(ProgramArgs? args) => "There is no help.";

    public void Start(ProgramArgs? args) =>
        TraceSource.WriteLine(DoMyOtherRoutine(DoMyRoutine()));

    internal string DoMyOtherRoutine(string input) =>
        $"The other routine is done. [{nameof(input)}: {input}]";

    internal string DoMyRoutine() => "The routine is done.";
}

class MyActivityWithOutput : IActivityWithOutput<ProgramArgs, string>
{
    static MyActivityWithOutput() => TraceSource =
        TraceSources.Instance.GetConfiguredTraceSource().WithSourceLevels().ToReferenceTypeValueOrThrow();
    static readonly TraceSource TraceSource;

    public string DisplayHelp(ProgramArgs? args) => "There is no help.";

    public void Start(ProgramArgs? args) =>
        TraceSource.WriteLine(StartForOutput(args));

    public string StartForOutput(ProgramArgs? args)
    {
        var output = $"output: {DoMyOtherRoutine(DoMyRoutine())}";

        TraceSource.WriteLine(output);

        return output;
    }
    internal string DoMyOtherRoutine(string input) =>
        $"The other routine is done. [{nameof(input)}: {input}]";

    internal string DoMyRoutine() => "The routine is done.";
}
