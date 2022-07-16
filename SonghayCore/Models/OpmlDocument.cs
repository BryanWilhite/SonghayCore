namespace Songhay.Models;

/// <summary>
/// Dave Winer, his Outline Processor Markup Language document format
/// </summary>
/// <remarks>
/// “OPML an XML-based format that allows exchange of outline-structured information
/// between applications running on different operating systems and environments.”
/// http://www.opml.org/about
/// </remarks>
[Serializable]
[XmlRoot(ElementName = "opml", Namespace = "http://songhaysystem.com/schemas/opml.xsd")]
[DataContract(Name = "opml")]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, WriteIndented = true)]
public class OpmlDocument
{
    /// <summary>
    /// The rx opml schema URI
    /// </summary>
    public const string RxOpmlSchema = "http://songhaysystem.com/schemas/opml.xsd";

    /// <summary>
    /// Initializes a new instance of the <see cref="OpmlDocument"/> class.
    /// </summary>
    public OpmlDocument()
    {
        Version = "2.0";
        XsiSchemaLocation = RxOpmlSchema + " " + RxOpmlSchema;
    }

    /// <summary>
    /// Gets or sets the schema location.
    /// </summary>
    [XmlAttribute("schemaLocation", Namespace = RxOpmlSchema)]
    [JsonPropertyName("schemaLocation")]
    public string? XsiSchemaLocation { get; set; }

    /// <summary>
    /// Gets the version.
    /// </summary>
    [XmlAttribute(AttributeName = "version")]
    [JsonPropertyName("version")]
    public string? Version { get; set; }

    /// <summary>
    /// Gets the OPML head element.
    /// </summary>
    [XmlElement(ElementName = "head")]
    [JsonPropertyName("head")]
    public OpmlHead? OpmlHead { get; set; }

    /// <summary>
    /// Gets the OPML body element.
    /// </summary>
    [XmlElement(ElementName = "body")]
    [JsonPropertyName("body")]
    public OpmlBody? OpmlBody { get; set; }
}
