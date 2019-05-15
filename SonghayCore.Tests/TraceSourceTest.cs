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

            traceSource = TraceSources.Instance.GetConfiguredTraceSource();
            nullTraceSource = TraceSources.Instance.GetConfiguredTraceSource(configuration, "wha?");
            otherTraceSource = TraceSources.Instance.GetConfiguredTraceSource(configuration, "other.TraceSourceName");
        }

        static readonly TraceSource traceSource;
        static readonly TraceSource nullTraceSource;
        static readonly TraceSource otherTraceSource;

        [TestMethod]
        public void ShouldHaveConfiguredTraceSources()
        {
            Assert.IsNotNull(traceSource);
            Assert.IsNotNull(otherTraceSource);
        }

        [TestMethod]
        public void ShouldNotHaveConfiguredTraceSource()
        {
            Assert.IsNull(nullTraceSource);
        }
    }
}
