using Meziantou.Extensions.Logging.Xunit;
using Microsoft.Extensions.Logging;

namespace Songhay.Tests.Extensions;

// ReSharper disable once InconsistentNaming
public class ILoggerExtensionsTests(ITestOutputHelper helper)
{
    [Fact]
    public void LogErrorForMissingData_Test()
    {
        ILogger logger = _loggerProvider.CreateLogger(nameof(LogErrorForMissingData_Test));
        logger.LogErrorForMissingData("foo");
        logger.LogErrorForMissingData<TestMessage>();
    }

    [Fact]
    public void LogTraceMethodCall_Test()
    {
        ILogger logger = _loggerProvider.CreateLogger(nameof(LogTraceMethodCall_Test));
        logger.LogTraceMethodCall(nameof(LogTraceMethodCall_Test));
    }

    [Fact]
    public void LogTraceDataTypeAndValue_Test()
    {
        ILogger logger = _loggerProvider.CreateLogger(nameof(LogTraceMethodCall_Test));
        MyRecord data = new();
        logger.LogTraceDataTypeAndValue(data);
    }

    record struct MyRecord
    {
        public MyRecord() { }
        public string One { get; init; } = "one point eight";
        public string Two { get; init; } = "two";
        public DateTime Stamp {get; init; } = DateTime.Now;
    }

    readonly XUnitLoggerProvider _loggerProvider = new(helper);
}
