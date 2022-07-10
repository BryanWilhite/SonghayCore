using Songhay.Diagnostics;
using Songhay.Extensions;
using Songhay.Models;
using System.Diagnostics;
using Songhay.Abstractions;

namespace Songhay.Tests.Activities;

public class GetHelloWorldReportActivity : IActivity
{
    static GetHelloWorldReportActivity() => TraceSource = TraceSources.Instance.GetTraceSourceFromConfiguredName().WithSourceLevels();
    static readonly TraceSource? TraceSource;

    public string DisplayHelp(ProgramArgs? args)
    {
        throw new System.NotImplementedException();
    }

    public void Start(ProgramArgs? args)
    {
        TraceSource.WriteLine("Sorry, but the Hello Worlds reports are not yet available :(");
    }
}
