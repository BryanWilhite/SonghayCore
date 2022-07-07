using Songhay.Diagnostics;
using Songhay.Extensions;
using System.Diagnostics;

namespace Songhay.Tests.Diagnostics;

public class MyClass
{
    static MyClass()
    {
        traceSource = TraceSources.Instance.GetTraceSourceFromConfiguredName().WithSourceLevels().EnsureTraceSource();
        traceSource.WriteLine($"static constructor: {nameof(MyClass)}");
    }

    static readonly TraceSource traceSource;

    public MyClass()
    {
        traceSource.WriteLine($"{nameof(MyClass)} constructed.");
    }

    public string GetConfiguredTraceSourceName() => traceSource.Name;
}