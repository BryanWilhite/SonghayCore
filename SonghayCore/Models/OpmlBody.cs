using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Songhay.Models
{
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
        /// <value>The outlines.</value>
        [XmlElement(ElementName = "outline")]
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays",
            Justification = "Used for XML serialization.")]
        [JsonPropertyName("outline")]
        public OpmlOutline[] Outlines { get; set; }
    }
}
