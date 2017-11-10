using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Diagnostics;
using Songhay.Extensions;
using Songhay.Tests.Activities;
using System;
using System.Diagnostics;

namespace Songhay.Tests.Models
{
    [TestClass]
    public class ActivitiesGetterTest
    {
        static ActivitiesGetterTest() => traceSource = TraceSources.Instance.GetConfiguredTraceSource().WithAllSourceLevels();
        static readonly TraceSource traceSource;

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ShouldGetActivityFromDefaultName()
        {
            using (var listener = new TextWriterTraceListener(Console.Out))
            {
                traceSource.Listeners.Add(listener);

                var args = new[]
                {
                    nameof(GetHelloWorldActivity),
                    "--world-name",
                    "Saturn"
                };

                var getter = new MyActivitiesGetter(args);
                var activity = getter.GetActivity();
                Assert.IsNotNull(activity);

                activity.Start(getter.Args);

                listener.Flush();
            }
        }
    }
}
