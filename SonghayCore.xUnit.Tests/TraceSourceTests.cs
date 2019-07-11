using Microsoft.Extensions.Configuration;
using Songhay.Diagnostics;
using Songhay.Extensions;
using Songhay.Models;
using System;
using System.Diagnostics;
using System.IO;
using Xunit;

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

        [Fact]
        public void ShouldHaveConfiguredTraceSources()
        {
            Assert.NotNull(traceSource);
            Assert.NotNull(otherTraceSource);

            using (var listener = new TextWriterTraceListener(Console.Out))
            {
                traceSource.Listeners.Add(listener);
                otherTraceSource.Listeners.Add(listener);

                traceSource?.TraceInformation("info!");
                otherTraceSource?.TraceInformation("other info!");

                traceSource.TraceVerbose("verbose!");
                otherTraceSource.TraceVerbose("other verbose!");

                traceSource.TraceError("warn!");
                otherTraceSource.TraceError("other warn!");

                traceSource.TraceError("err!");
                otherTraceSource.TraceError("other err!");

                listener.Flush();
            }
        }

        [Fact]
        public void ShouldNotHaveConfiguredTraceSource()
        {
            Assert.Null(nullTraceSource);

            nullTraceSource?.TraceInformation("info!");
            nullTraceSource.TraceVerbose("info!");
            nullTraceSource.TraceWarning("info!");
            nullTraceSource.TraceError("info!");
        }
    }
}
