using Songhay.Diagnostics;
using Songhay.Extensions;
using System.Diagnostics;

namespace Songhay.Tests.Diagnostics;

public class MyClass
{
    static MyClass()
    {
        TraceSource = TraceSources.Instance.GetTraceSourceFromConfiguredName().WithSourceLevels().ToValueOrThrow();
        TraceSource.WriteLine($"static constructor: {nameof(MyClass)}");
    }

    static readonly TraceSource TraceSource;

    public MyClass()
    {
        TraceSource.WriteLine($"{nameof(MyClass)} constructed.");
    }

    public string GetConfiguredTraceSourceName() => TraceSource.Name;
}
