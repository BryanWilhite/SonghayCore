namespace Songhay.Tests.Orderers;

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
    /// Initializes a new instance of the <see cref="OrderedTestBase"/> class.
    /// </summary>
    protected OrderedTestBase()
    {
        AppDomain.CurrentDomain.FirstChanceException += (_, e) =>
        {
            if (XUnitExceptionHasOccurred || !IsXunitException(e.Exception)) return;

            XUnitExceptionHasOccurred = true;
            _lastException = e.Exception;
        };
    }

    /// <summary>
    /// Returns <c>true</c> when an XUnit assertion exception has occurred.
    /// </summary>
    protected static bool XUnitExceptionHasOccurred { get; private set; }

    /// <summary>
    /// Asserts there are no exceptions of type <c>xUnit.Sdk.*</c>
    /// to prevent ordered tests from running.
    /// </summary>
    /// <remarks>
    /// See https://github.com/xunit/xunit/issues/856
    /// </remarks>
    protected static void AssertNoXUnitException() =>
        Assert.False(XUnitExceptionHasOccurred,
            $"Assertion Failed: An exception has occurred under test [message: `{_lastException?.Message}`].");

    static Exception? _lastException;

    /// <summary>
    /// Determines whether the specified <see cref="Exception"/> is from an xUnit assertion.
    /// </summary>
    /// <param name="ex">The ex.</param>
    /// <returns>
    ///   <c>true</c> if it is a xUnit <see cref="Exception"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// <see cref="AppDomain.FirstChanceException"/> will detect ALL exceptions,
    /// including those NOT on the current path of execution!
    /// 
    /// This means a huge amount of exceptions can pass through <see cref="AppDomain.FirstChanceException"/>.
    /// </remarks>
    static bool IsXunitException(Exception? ex) =>
        ex?.GetType().FullName?.ToLowerInvariant().StartsWith("xunit.sdk") == true;
}
