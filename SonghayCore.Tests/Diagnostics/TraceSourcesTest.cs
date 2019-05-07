#if NETSTANDARD

using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Diagnostics;
using Songhay.Models;
using System;
using System.Diagnostics;
using System.IO;

namespace Songhay.Tests.Diagnostics
{
    [TestClass]
    public class TraceSourcesTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ShouldGetConfiguredTraceSourceName()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("./appsettings.json");

            var configuration = builder.Build();
            var name = configuration[DeploymentEnvironment.DefaultTraceSourceNameConfigurationKey];
            Assert.IsFalse(string.IsNullOrEmpty(name), "The expected configuration trace source name is not here.");
            this.TestContext.WriteLine($"configuration trace source name: {name}");

            TraceSources.ConfiguredTraceSourceName = name;
            Assert.IsTrue(TraceSources.IsConfiguredTraceSourceNameLoaded, $"The expected {nameof(TraceSources)} state is not here.");

            var traceSource = TraceSources.Instance.GetTraceSourceFromConfiguredName();

            using (var listener = new TextWriterTraceListener(Console.Out))
            {
                traceSource.Listeners.Add(listener);

                this.TestContext.WriteLine($"instantiating {nameof(MyClass)}...");
                var mine = new MyClass();
                Assert.IsTrue(mine.GetConfiguredTraceSourceName() == name, "The expected configured configuration trace source name is not here.");
            }
        }
    }
}

#endif
