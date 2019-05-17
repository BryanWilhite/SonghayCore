using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Diagnostics;
using Songhay.Extensions;
using Songhay.Models;
using System;
using System.Diagnostics;
using System.IO;

namespace Songhay.Tests
{
    [TestClass]
    public class TraceSourceTest
    {
        static TraceSourceTest()
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

        [TestMethod]
        public void ShouldHaveConfiguredTraceSources()
        {
            Assert.IsNotNull(traceSource);
            Assert.IsNotNull(otherTraceSource);

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

        [TestMethod]
        public void ShouldNotHaveConfiguredTraceSource()
        {
            Assert.IsNull(nullTraceSource);

            nullTraceSource?.TraceInformation("info!");
            nullTraceSource.TraceVerbose("info!");
            nullTraceSource.TraceWarning("info!");
            nullTraceSource.TraceError("info!");
        }
    }
}
