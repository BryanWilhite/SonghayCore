namespace Songhay.Models;

/// <summary>
/// Defines the conventional XHTML Document.
/// </summary>
[Serializable]
public class XhtmlDocument
{
    /// <summary>
    /// Gets or sets the location.
    /// </summary>
    [XmlAttribute]
    public string? Location { get; set; }

    /// <summary>
    /// Gets or sets the header.
    /// </summary>
    [XmlAttribute]
    public string? Header { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    [XmlAttribute]
    public string? Title { get; set; }
}
