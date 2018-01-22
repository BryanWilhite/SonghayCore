using Songhay.Diagnostics;
using Songhay.Extensions;
using System.Diagnostics;

namespace Songhay.Tests.Diagnostics
{
    public class MyClass
    {
        static MyClass()
        {
            traceSource = TraceSources.Instance.GetTraceSourceFromConfiguredName().WithAllSourceLevels().EnsureTraceSource();
            traceSource.TraceInformation($"static constructor: {nameof(MyClass)}");
        }

        static readonly TraceSource traceSource;

        public MyClass()
        {
            traceSource.TraceInformation($"{nameof(MyClass)} constructed.");
        }

        public string GetConfiguredTraceSourceName() => traceSource.Name;
    }
}
