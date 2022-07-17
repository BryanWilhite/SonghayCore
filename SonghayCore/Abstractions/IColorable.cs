namespace Songhay.Abstractions;

/// <summary>
/// Defines a colorable visual.
/// </summary>
public interface IColorable
{
    /// <summary>
    /// Gets or sets the background hexadecimal value.
    /// </summary>
    string? BackgroundHex { get; set; }

    /// <summary>
    /// Gets or sets the foreground hexadecimal value.
    /// </summary>
    string? ForegroundHex { get; set; }
}
