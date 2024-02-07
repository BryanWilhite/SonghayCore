namespace Songhay.Tests.Orderers;

public class OrderedTestBaseFailureTests : OrderedTestBase
{
    static readonly List<string> MethodCalls = new();

    static int _currentValue = 32;

    public OrderedTestBaseFailureTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact, TestOrder(ordinal: 0, reason: "Subtract 8")]
    public void ShouldSubtract8()
    {
        AssertNoXUnitException();
        MethodCalls.Add(nameof(ShouldSubtract8));

        const int wrongValue = 42;

        _currentValue -= wrongValue;

        Assert.Equal(24, _currentValue);
    }

    [Fact, TestOrder(ordinal: 1, reason: "Subtract 4")]
    public void ShouldSubtract4()
    {
        Assert.True(XUnitExceptionHasOccurred);

        MethodCalls.Add(nameof(ShouldSubtract4));
    }

    [Fact, TestOrder(ordinal: 2, reason: "Subtract 4 again")]
    public void ShouldSubtract4Again()
    {
        Assert.True(XUnitExceptionHasOccurred);

        MethodCalls.Add(nameof(ShouldSubtract4Again));

        _testOutputHelper.WriteLine("method calls:");
        MethodCalls.ForEachInEnumerable(m => _testOutputHelper.WriteLine(m));
    }

    readonly ITestOutputHelper _testOutputHelper;
}
