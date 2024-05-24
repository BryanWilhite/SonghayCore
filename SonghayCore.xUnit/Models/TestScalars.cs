namespace Songhay.Tests.Models;

/// <summary>
/// Shared values for this assembly.
/// </summary>
public static class TestScalars
{
    /// <summary>
    /// Returns <c>true</c> when <see cref="Debugger.IsAttached"/> is <c>false</c>.
    /// </summary>
    public static bool IsNotDebugging { get; } = !Debugger.IsAttached;

    /// <summary>
    /// The Microsoft conventional relative path, indicating the number of directories
    /// above compiled assembly output in a modern .NET project.
    /// </summary>
    /// <remarks>
    /// In .NET core and in modern .NET the path to compiled assembly output is:
    ///
    /// <code>
    /// {Project Directory}/bin/Debug/{.NET Version}
    /// </code>
    ///
    /// The <c>/bin/Debug/{.NET Version}</c> directory is three levels below {Project Directory}.
    /// </remarks>
    public const string NetCoreRelativePathToProjectFolder = "../../../";

    /// <summary>
    /// The conventional reason for skipping a test
    /// when not debugging the test
    /// </summary>
    public const string ReasonForSkippingWhenNotDebugging = "This test is intended to run when a Debugger is attached.";

    /// <summary>
    /// The xunit category trait
    /// </summary>
    public const string XunitCategory = "Category";

    /// <summary>
    /// The xunit category integration test
    /// </summary>
    public const string XunitCategoryIntegrationTest = "Integration";

    /// <summary>
    /// The xunit category integration manual test
    /// </summary>
    public const string XunitCategoryIntegrationManualTest = "IntegrationManual";
}
