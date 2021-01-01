using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests.Orderers
{
    static class MyTestCaseData
    {
        public static int MyNumber = 0;
    }

    public class TestCaseOrdererTest
    {
        static TestCaseOrdererTest() => Log = new StringBuilder();

        public TestCaseOrdererTest(ITestOutputHelper output) => _output = output;

        [Fact, TestOrder(ordinal: 4, reason: "This test is last and depends on the other ordered tests to complete.")]
        public void LastTest()
        {
            Log.AppendLine(nameof(this.LastTest));
            _output.WriteLine(Log.ToString());

            var expected = 1;
            Assert.Equal(expected, MyTestCaseData.MyNumber);
        }

        [Fact, TestOrder(ordinal: 3)]
        public void TestOne()
        {
            Log.AppendLine(nameof(this.TestOne));

            MyTestCaseData.MyNumber = 1;
        }

        [Fact, TestOrder(ordinal: 2)]
        public void TestTwo()
        {
            Log.AppendLine(nameof(this.TestTwo));

            MyTestCaseData.MyNumber = 2;
        }

        [Fact, TestOrder(ordinal: 1)]
        public void TestThree()
        {
            Log.AppendLine(nameof(this.TestThree));

            MyTestCaseData.MyNumber = 3;
        }

        private static readonly StringBuilder Log;

        private readonly ITestOutputHelper _output;
    }
}
