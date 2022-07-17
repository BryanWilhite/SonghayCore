namespace Songhay.Abstractions;

/// <summary>
/// Defines a selectable visual.
/// </summary>
public interface ISelectable
{
    /// <summary>
    /// Gets or sets whether this is default selection.
    /// </summary>
    bool? IsDefaultSelection { get; set; }

    /// <summary>
    /// Gets or sets whether this is enabled.
    /// </summary>
    bool? IsEnabled { get; set; }

    /// <summary>
    /// Gets or sets whether this is selected.
    /// </summary>
    bool? IsSelected { get; set; }
}
