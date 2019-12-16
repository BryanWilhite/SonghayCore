using Songhay.Diagnostics;
using Songhay.Extensions;
using Songhay.Models;
using System.Diagnostics;

namespace Songhay.Tests.Activities
{
    public class GetHelloWorldReportActivity : IActivity
    {
        static GetHelloWorldReportActivity() => traceSource = TraceSources.Instance.GetTraceSourceFromConfiguredName().WithSourceLevels();
        static readonly TraceSource traceSource;

        public string DisplayHelp(ProgramArgs args)
        {
            throw new System.NotImplementedException();
        }

        public void Start(ProgramArgs args)
        {
            traceSource.WriteLine("Sorry, but the Hello Worlds reports are not yet available :(");
        }
    }
}
