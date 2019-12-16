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

            this._testOutputHelper.WriteLine(activity.StartActivity(new ProgramArgs(args), traceSource));
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
