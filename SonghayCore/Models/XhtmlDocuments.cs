namespace Songhay.Models;

/// <summary>
/// Collects <see cref="XhtmlDocument"/>.
/// </summary>
[Serializable]
public class XhtmlDocuments
{
    /// <summary>
    /// Gets or sets the documents.
    /// </summary>
    [XmlElement("XhtmlDocument")]
    [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays",
        Justification = "Used for XML serialization.")]
    public XhtmlDocument[] Documents { get; set; } = Enumerable.Empty<XhtmlDocument>().ToArray();

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    [XmlAttribute]
    public string? Title { get; set; }
}
