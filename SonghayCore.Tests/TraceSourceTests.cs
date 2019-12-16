using Microsoft.Extensions.Configuration;
using Songhay.Diagnostics;
using Songhay.Extensions;
using Songhay.Models;
using System;
using System.Diagnostics;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests
{
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

            traceSource = TraceSources.Instance.GetConfiguredTraceSource().WithSourceLevels();
            nullTraceSource = TraceSources.Instance.GetConfiguredTraceSource(configuration, "wha?").WithSourceLevels();
            otherTraceSource = TraceSources.Instance.GetConfiguredTraceSource(configuration, "other.TraceSourceName").WithSourceLevels();
        }

        static readonly TraceSource traceSource;
        static readonly TraceSource nullTraceSource;
        static readonly TraceSource otherTraceSource;

        public TraceSourceTests(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
        }

        [Fact]
        public void ShouldHaveConfiguredTraceSources()
        {
            Assert.NotNull(traceSource);
            Assert.NotNull(otherTraceSource);

            using (var writer = new StringWriter())
            using (var listener = new TextWriterTraceListener(writer))
            {
                traceSource.Listeners.Add(listener);
                otherTraceSource.Listeners.Add(listener);

                traceSource?.WriteLine("info!");
                otherTraceSource?.WriteLine("other info!");

                traceSource.TraceVerbose("verbose!");
                otherTraceSource.TraceVerbose("other verbose!");

                traceSource.TraceError("warn!");
                otherTraceSource.TraceError("other warn!");

                traceSource.TraceError("err!");
                otherTraceSource.TraceError("other err!");

                listener.Flush();
                this._testOutputHelper.WriteLine(writer.ToString());
            }
        }

        [Fact]
        public void ShouldNotHaveConfiguredTraceSource()
        {
            Assert.Null(nullTraceSource);

            nullTraceSource?.WriteLine("info!");
            nullTraceSource.TraceVerbose("info!");
            nullTraceSource.TraceWarning("info!");
            nullTraceSource.TraceError("info!");
        }

        readonly ITestOutputHelper _testOutputHelper;
    }
}
