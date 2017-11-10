using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Songhay.Models
{
    /// <summary>
    /// Defines a managed representation of the OPML body element.
    /// </summary>
#if !NETSTANDARD1_2
    [Serializable]
#endif
    [JsonObject("head", MemberSerialization = MemberSerialization.OptIn)]
    public class OpmlBody
    {
        /// <summary>
        /// Gets or sets the outlines.
        /// </summary>
        /// <value>The outlines.</value>
#if !NETSTANDARD1_2
        [XmlElement(ElementName = "outline")]
#endif
        [JsonProperty("outline")]
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays",
            Justification = "Used for XML serialization.")]
        public OpmlOutline[] Outlines { get; set; }
    }
}
