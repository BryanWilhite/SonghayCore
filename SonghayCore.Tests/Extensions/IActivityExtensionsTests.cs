using Songhay.Diagnostics;
using Songhay.Extensions;
using Songhay.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests.Extensions
{
    public class IActivityExtensionsTests
    {
        static IActivityExtensionsTests()
        {
            var configuration = ProgramUtility.LoadConfiguration(Directory.GetCurrentDirectory());
            TraceSources.ConfiguredTraceSourceName = configuration[DeploymentEnvironment.DefaultTraceSourceNameConfigurationKey];

            traceSource = TraceSources.Instance.GetConfiguredTraceSource().WithSourceLevels();
        }

        static TraceSource traceSource;

        public IActivityExtensionsTests(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
        }

        [Fact]
        public void StartActivity_Test()
        {
            var args = new[] { nameof(MyActivity) };
            var activitiesGetter = new MyActivitiesGetter(args);
            var activity = activitiesGetter?.GetActivity();
            Assert.NotNull(activity);

            var log = activity.StartActivity(new ProgramArgs(args), traceSource);
            Assert.False(string.IsNullOrWhiteSpace(log));
            this._testOutputHelper.WriteLine(log);
        }

        [Theory]
        [ProjectFileData(typeof(IActivityExtensionsTests),
            "../../../json/StartActivity_StreamWriter_Test_output.log")]
        public void StartActivity_StreamWriter_Test(FileInfo outputInfo)
        {
            var args = new[] { nameof(MyActivity) };
            var activitiesGetter = new MyActivitiesGetter(args);
            var activity = activitiesGetter?.GetActivity();
            Assert.NotNull(activity);

            var disposable = new StreamWriter(outputInfo.FullName);

            var log = activity.StartActivity(
                new ProgramArgs(args),
                traceSource,
                () => disposable,
                flushLog: false);

            Assert.Null(log);
            Assert.Throws<ObjectDisposedException>(() => disposable.WriteLine("foo"));

            log = File.ReadAllText(outputInfo.FullName);
            Assert.False(string.IsNullOrWhiteSpace(log));
            this._testOutputHelper.WriteLine(log);
        }

        ITestOutputHelper _testOutputHelper;
    }

    class MyActivitiesGetter : ActivitiesGetter
    {
        public MyActivitiesGetter(string[] args) : base(args)
        {
            this.LoadActivities(new Dictionary<string, Lazy<IActivity>>
            {
                {
                    nameof(MyActivity),
                    new Lazy<IActivity>(() => new MyActivity())
                },
            });
        }
    }

    class MyActivity : IActivity
    {
        static MyActivity() => traceSource = TraceSources.Instance.GetConfiguredTraceSource().WithSourceLevels();
        static TraceSource traceSource;

        public string DisplayHelp(ProgramArgs args) => "There is no help.";

        public void Start(ProgramArgs args) =>
            traceSource?.WriteLine(this.DoMyOtherRoutine(this.DoMyRoutine()));

        internal string DoMyOtherRoutine(string input) =>
            $"The other routine is done. [{nameof(input)}: {input ?? "[null]"}]";

        internal string DoMyRoutine() => "The routine is done.";
    }
}
