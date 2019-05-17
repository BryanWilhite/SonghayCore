using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Diagnostics;
using Songhay.Extensions;
using System;
using System.Diagnostics;

namespace SonghayCore.Framework.Tests
{
    [TestClass]
    public class TraceSourceTest
    {
        static TraceSourceTest()
        {
            traceSource = TraceSources.Instance.GetConfiguredTraceSource();
            nullTraceSource = TraceSources.Instance.GetConfiguredTraceSource("wha?");
            otherTraceSource = TraceSources.Instance.GetConfiguredTraceSource("other.TraceSourceName");
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
