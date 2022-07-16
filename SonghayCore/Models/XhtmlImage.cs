namespace Songhay.Models;

/// <summary>
/// Defines an image used by XHTML(5) clients
/// </summary>
public class XhtmlImage
{
    /// <summary>
    /// Gets or sets the location.
    /// </summary>
    public Uri? Location { get; set; }

    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the height in pixels.
    /// </summary>
    public int? HeightInPixels { get; set; }

    /// <summary>
    /// Gets or sets the width.
    /// </summary>
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the width in pixels.
    /// </summary>
    public int? WidthInPixels { get; set; }
}
