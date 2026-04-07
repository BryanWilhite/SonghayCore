namespace Songhay.Models;

/// <summary>
/// Defines a managed representation of the OPML body element.
/// </summary>
[Serializable]
[DataContract(Name = "head")]
public class OpmlBody
{
    /// <summary>
    /// Gets or sets the outlines.
    /// </summary>
    [XmlElement(ElementName = "outline")]
    [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays",
        Justification = "Used for XML serialization.")]
    [JsonPropertyName("outline")]
    public OpmlOutline[] Outlines { get; set; } = [];

    /// <summary>
    /// Converts the value of this instance to a <see cref="string"/>.
    /// </summary>
    public override string ToString()
    {
        StringBuilder sb = new();

        sb.AppendLine($", {nameof(Outlines)}: {Outlines.Length}");

        foreach (OpmlOutline outline in Outlines)
        {
            sb.AppendLine($"{outline}");
        }

        return sb.ToString();
    }
}
