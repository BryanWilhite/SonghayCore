using Xunit;

namespace Songhay.Tests.Orderers
{
    /// <summary>
    /// Provides ordered test assertions.
    /// </summary>
    /// <remarks>
    /// For more detail, see “How to Order xUnit Tests and Collections” by Tom DuPont
    /// [http://www.tomdupont.net/2016/04/how-to-order-xunit-tests-and-collections.html]
    /// </remarks>
    [TestCaseOrderer(TestCaseOrderer.TypeName, TestCaseOrderer.AssemblyName)]
    public abstract class OrderedTestBase
    {
        /// <summary>
        /// The expected ordinal of the current test.
        /// </summary>
        protected static int TestOrdinal;

        protected void AssertTestName(string testName)
        {
            var type = GetType();
            var queue = TestCaseOrderer.QueuedTests[type.FullName];
            var result = queue.TryDequeue(out string dequeuedName);
            Assert.True(result);
            Assert.Equal(testName, dequeuedName);
        }
    }
}
