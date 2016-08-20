using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Songhay.Tests.Extensions
{
    using Songhay.Extensions;

    /// <summary>
    /// Tests for extensions of <see cref="System.Threading.Tasks.Task"/>
    /// </summary>
    [TestClass]
    public class TaskExtensionsTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ShouldDelayByOneSecond()
        {
            var thePast = DateTime.Now;

            Task delayTask = null;
            delayTask = delayTask.Delay(TimeSpan.FromSeconds(1), i =>
            {
                var test = DateTime.Now.Second - thePast.Second >= 1;
                Assert.IsTrue(test, "The expected delay did not occur.");
            });
        }
    }
}
