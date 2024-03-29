﻿namespace Songhay.Models;

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
    public OpmlOutline[] Outlines { get; set; } = Enumerable.Empty<OpmlOutline>().ToArray();
}
