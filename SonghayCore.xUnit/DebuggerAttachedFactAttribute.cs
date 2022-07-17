using System.Diagnostics;

namespace Songhay.Tests;

/// <summary>
/// Extends <see cref="FactAttribute"/> to skip
/// when <see cref="Debugger.IsAttached"/> is <c>false</c>.
/// </summary>
public sealed class DebuggerAttachedFactAttribute : FactAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DebuggerAttachedFactAttribute"/> class.
    /// </summary>
    public DebuggerAttachedFactAttribute()
    {
        if (!Debugger.IsAttached) Skip = "This test is intended to run when a Debugger is attached.";
    }
}
