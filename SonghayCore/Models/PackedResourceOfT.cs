namespace Songhay.Models;

/// <summary>
/// Defines the meta-data of an Application resource.
/// </summary>
/// <typeparam name="TResource">The type of the resource.</typeparam>
public class PackedResource<TResource> : PackedResource
{
    /// <summary>
    /// Gets or sets the XAML string.
    /// </summary>
    public string? XamlString { get; set; }

    /// <summary>
    /// Gets or sets the XAML object.
    /// </summary>
    public TResource? XamlObject { get; set; }
}
