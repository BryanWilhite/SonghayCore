using System.Text;

namespace Songhay.Tests.Orderers;

internal static class MyTestCaseData
{
    public static int MyNumber;
}

public class TestCaseOrdererTests
{
    static TestCaseOrdererTests() => Log = new StringBuilder();

    public TestCaseOrdererTests(ITestOutputHelper output) => _output = output;

    [Fact, TestOrder(ordinal: 4, reason: "This test is last and depends on the other ordered tests to complete.")]
    public void LastTest()
    {
        Log.AppendLine(nameof(LastTest));
        _output.WriteLine(Log.ToString());

        var expected = 1;
        Assert.Equal(expected, MyTestCaseData.MyNumber);
    }

    [Fact, TestOrder(ordinal: 3)]
    public void TestOne()
    {
        Log.AppendLine(nameof(TestOne));

        MyTestCaseData.MyNumber = 1;
    }

    [Fact, TestOrder(ordinal: 2)]
    public void TestTwo()
    {
        Log.AppendLine(nameof(TestTwo));

        MyTestCaseData.MyNumber = 2;
    }

    [Fact, TestOrder(ordinal: 1)]
    public void TestThree()
    {
        Log.AppendLine(nameof(TestThree));

        MyTestCaseData.MyNumber = 3;
    }

    static readonly StringBuilder Log;

    readonly ITestOutputHelper _output;
}