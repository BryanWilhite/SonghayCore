using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Songhay.Models;

/// <summary>
/// Defines a managed representation of the OPML outline element.
/// </summary>
[Serializable]
[XmlRoot(ElementName = "outline")]
[DataContract(Name ="outline")]
public class OpmlOutline
{
    /// <summary>
    /// Gets or sets the Category.
    /// </summary>
    /// <value>The ID.</value>
    [XmlAttribute(AttributeName = "category")]
    [JsonPropertyName("category")]
    public string Category { get; set; }

    /// <summary>
    /// Gets or sets the ID.
    /// </summary>
    /// <value>The ID.</value>
    [XmlAttribute(AttributeName = "id")]
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the outlines.
    /// </summary>
    /// <value>The outlines.</value>
    [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays",
        Justification = "Used for XML serialization.")]
    [XmlElement(ElementName = "outline")]
    [JsonPropertyName("outline")]
    public OpmlOutline[] Outlines { get; set; }

    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    /// <value>The text.</value>
    [XmlAttribute(AttributeName = "text")]
    [JsonPropertyName("text")]
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>The title.</value>
    [XmlAttribute(AttributeName = "title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    /// <value>The type.</value>
    [XmlAttribute(AttributeName = "type")]
    [JsonPropertyName("type")]
    public string OutlineType { get; set; }

    /// <summary>
    /// Gets or sets the URL.
    /// </summary>
    /// <value>The URL.</value>
    [XmlAttribute(AttributeName = "url")]
    [JsonPropertyName("url")]
    [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings",
        Justification = "OPML does not recognize the concept of the System.Uri.")]
    public string Url { get; set; }

    /// <summary>
    /// Gets or sets the XML URL.
    /// </summary>
    /// <value>The XML URL.</value>
    [XmlAttribute(AttributeName = "xmlUrl")]
    [JsonPropertyName("xmlUrl")]
    [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings",
        Justification = "OPML does not recognize the concept of the System.Uri.")]
    public string XmlUrl { get; set; }
}