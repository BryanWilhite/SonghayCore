namespace Songhay.Abstractions;

/// <summary>
/// Tags instances for DI with keyed services.
/// </summary>
/// <remarks>
/// This contract helps extension methods verify that a specific implementation is being operated upon.
/// </remarks>
public interface ITaggedInstance
{
    /// <summary>
    /// Returns the conventional ID or tag of this instance.
    /// </summary>
    /// <remarks>
    /// This identifier is intended for asserting that an expected ID is present
    /// which is useful for ensuring that an extension method is operating on the expected instance.
    /// </remarks>
    string InstanceTag { get; }
}
