using Songhay.Extensions;

namespace Songhay.Tests.Orderers;

public class OrderedTestBaseTests : OrderedTestBase
{
    const int ExpectedResult = 16;

    static readonly List<string> MethodCalls = new();

    static int _currentValue = 32;

    public OrderedTestBaseTests(ITestOutputHelper testOutputHelper)
    {
        AssertNoXUnitException();

        _testOutputHelper = testOutputHelper;
    }

    [Fact, TestOrder(ordinal: 0, reason: "Subtract 8")]
    public void ShouldSubtract8()
    {
        _currentValue -= 8;

        Assert.Equal(24, _currentValue);

        MethodCalls.Add(nameof(ShouldSubtract8));
    }

    [Fact, TestOrder(ordinal: 1, reason: "Subtract 4")]
    public void ShouldSubtract4()
    {
        _currentValue -= 4;

        Assert.Equal(20, _currentValue);

        MethodCalls.Add(nameof(ShouldSubtract4));
    }

    [Fact, TestOrder(ordinal: 2, reason: "Subtract 4 again")]
    public void ShouldSubtract4Again()
    {
        _currentValue -= 4;

        Assert.Equal(ExpectedResult, _currentValue);

        MethodCalls.Add(nameof(ShouldSubtract4Again));

        _testOutputHelper.WriteLine("method calls:");
        MethodCalls.ForEachInEnumerable(m => _testOutputHelper.WriteLine(m));
    }

    readonly ITestOutputHelper _testOutputHelper;
}
