using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Diagnostics;
using Songhay.Extensions;
using Songhay.Models;
using Songhay.Tests.Activities;
using System;
using System.Diagnostics;
using System.IO;

namespace Songhay.Tests.Models
{
    [TestClass]
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

        static readonly TraceSource traceSource;

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ShouldGetActivityFromDefaultName()
        {
            using (var listener = new TextWriterTraceListener(Console.Out))
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
                Assert.IsNotNull(activity);

                activity.Start(getter.Args);

                listener.Flush();
            }
        }

        [TestMethod]
        public void ShouldHandleEmptyArgments()
        {
            try
            {
                var getter = new MyActivitiesGetter(new string[] { });
            }
            catch (ArgumentException ex)
            {
                this.TestContext.WriteLine($"The expected exception: {ex.Message}");
            }
        }

        [TestMethod]
        public void ShouldHandleNullArgments()
        {
            try
            {
                var getter = new MyActivitiesGetter(null);
            }
            catch (ArgumentNullException ex)
            {
                this.TestContext.WriteLine($"The expected exception: {ex.Message}");
            }
        }

        [TestMethod]
        public void ShouldShowHelpDisplayText()
        {
            using (var listener = new TextWriterTraceListener(Console.Out))
            {
                traceSource.Listeners.Add(listener);

                var args = new[]
                {
                    nameof(GetHelloWorldActivity),
                    ProgramArgs.Help
                };

                var getter = new MyActivitiesGetter(args);
                var activity = getter.GetActivity();
                Assert.IsNotNull(activity);

                if (getter.Args.IsHelpRequest())
                    this.TestContext.WriteLine(activity.DisplayHelp(getter.Args));

                activity.Start(getter.Args);

                listener.Flush();
            }
        }
    }
}
