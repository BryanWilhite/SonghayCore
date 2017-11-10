using Songhay.Diagnostics;
using Songhay.Extensions;
using Songhay.Models;
using System.Diagnostics;

namespace Songhay.Tests.Activities
{
    public class GetHelloWorldActivity : IActivity
    {
        static GetHelloWorldActivity() => traceSource = TraceSources.Instance.GetConfiguredTraceSource().WithAllSourceLevels();
        static readonly TraceSource traceSource;

        public void Start(ProgramArgs args)
        {
            var worldName = args.GetArgValue("--world-name");
            traceSource.EnsureTraceSource().TraceInformation($"Hello from world {worldName}!");
        }
    }
}
