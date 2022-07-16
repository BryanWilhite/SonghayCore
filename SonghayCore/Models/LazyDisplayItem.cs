namespace Songhay.Models;

/// <summary>
/// Wraps a lazy-initialized object
/// with meta-data for display.
/// </summary>
/// <typeparam name="T"></typeparam>
public class LazyDisplayItem<T>
{
    /// <summary>
    /// Gets or sets the targetValues name.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the lazy item.
    /// </summary>
    public Lazy<T?>? LazyItem { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string? Name { get; set; }
}
