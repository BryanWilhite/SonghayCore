using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Songhay.Models
{
    /// <summary>
    /// Defines a managed representation of the OPML outline element.
    /// </summary>
#if !NETSTANDARD1_2
    [Serializable]
    [XmlRoot(ElementName = "outline")]
#endif
    [JsonObject("outline", MemberSerialization = MemberSerialization.OptIn)]
    public class OpmlOutline
    {
        /// <summary>
        /// Gets or sets the Category.
        /// </summary>
        /// <value>The ID.</value>
#if !NETSTANDARD1_2
        [XmlAttribute(AttributeName = "category")]
#endif
        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
#if !NETSTANDARD1_2
        [XmlAttribute(AttributeName = "id")]
#endif
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the outlines.
        /// </summary>
        /// <value>The outlines.</value>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays",
            Justification = "Used for XML serialization.")]
#if !NETSTANDARD1_2
        [XmlElement(ElementName = "outline")]
#endif
        [JsonProperty("outline")]
        public OpmlOutline[] Outlines { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
#if !NETSTANDARD1_2
        [XmlAttribute(AttributeName = "text")]
#endif
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
#if !NETSTANDARD1_2
        [XmlAttribute(AttributeName = "title")]
#endif
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
#if !NETSTANDARD1_2
        [XmlAttribute(AttributeName = "type")]
#endif
        [JsonProperty("type")]
        public string OutlineType { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
#if !NETSTANDARD1_2
        [XmlAttribute(AttributeName = "url")]
#endif
        [JsonProperty("url")]
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings",
            Justification = "OPML does not recognize the concept of the System.Uri.")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the XML URL.
        /// </summary>
        /// <value>The XML URL.</value>
#if !NETSTANDARD1_2
        [XmlAttribute(AttributeName = "xmlUrl")]
#endif
        [JsonProperty("xmlUrl")]
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings",
            Justification = "OPML does not recognize the concept of the System.Uri.")]
        public string XmlUrl { get; set; }
    }
}
