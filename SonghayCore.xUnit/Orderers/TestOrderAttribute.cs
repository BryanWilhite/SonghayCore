using System;

namespace Songhay.Tests.Orderers;

/// <summary>
/// Defines the attribute used to order tests
/// by the specified ordinal.
/// </summary>
/// <seealso cref="Attribute" />
public class TestOrderAttribute : Attribute
{
    /// <summary>Gets the ordinal.</summary>
    /// <value>The ordinal.</value>
    public int Ordinal { get; }

    /// <summary>Gets the reason for choosing the order.</summary>
    /// <value>The reason.</value>
    public string? Reason { get; }

    /// <summary>Initializes a new instance of the <see cref="TestOrderAttribute"/> class.</summary>
    /// <param name="ordinal">The ordinal.</param>
    public TestOrderAttribute(int ordinal) => Ordinal = ordinal;

    /// <summary>Initializes a new instance of the <see cref="TestOrderAttribute"/> class.</summary>
    /// <param name="ordinal">The ordinal.</param>
    /// <param name="reason">The reason.</param>
    public TestOrderAttribute(int ordinal, string? reason)
    {
        Ordinal = ordinal;
        Reason = reason;
    }
}
