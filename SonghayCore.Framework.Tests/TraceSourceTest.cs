using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Diagnostics;
using Songhay.Extensions;
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
        }

        [TestMethod]
        public void ShouldNotHaveConfiguredTraceSource()
        {
            Assert.IsNull(nullTraceSource);
        }
    }
}
