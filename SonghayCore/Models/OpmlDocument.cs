
using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace Songhay.Models
{
    /// <summary>
    /// Dave Winer, his Outline Processor Markup Language document format
    /// </summary>
    /// <remarks>
    /// “OPML an XML-based format that allows exchange of outline-structured information
    /// between applications running on different operating systems and environments.”
    /// http://www.opml.org/about
    /// </remarks>
#if !NETSTANDARD1_2
    [Serializable]
    [XmlRoot(ElementName = "opml", Namespace = "http://songhaysystem.com/schemas/opml.xsd")]
#endif
    [JsonObject("opml", MemberSerialization = MemberSerialization.OptIn)]
    public class OpmlDocument
    {
        /// <summary>
        /// The rx opml schema URI
        /// </summary>
        public const string rxOpmlSchema = "http://songhaysystem.com/schemas/opml.xsd";

        /// <summary>
        /// Initializes a new instance of the <see cref="OpmlDocument"/> class.
        /// </summary>
        public OpmlDocument()
        {
            this.Version = "2.0";
            this.XsiSchemaLocation = rxOpmlSchema + " " + rxOpmlSchema;
        }

        /// <summary>
        /// Gets or sets the schema location.
        /// </summary>
        /// <value>The schema location.</value>
#if !NETSTANDARD1_2
        [XmlAttribute("schemaLocation", Namespace = rxOpmlSchema)]
#endif
        [JsonProperty("schemaLocation")]
        public string XsiSchemaLocation { get; set; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
#if !NETSTANDARD1_2
        [XmlAttribute(AttributeName = "version")]
#endif
        [JsonProperty("version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets the OPML head element.
        /// </summary>
        /// <value>The OPML head element.</value>
#if !NETSTANDARD1_2
        [XmlElement(ElementName = "head")]
#endif
        [JsonProperty("head")]
        public OpmlHead OpmlHead { get; set; }

        /// <summary>
        /// Gets the OPML body element.
        /// </summary>
        /// <value>The OPML body element.</value>
#if !NETSTANDARD1_2
        [XmlElement(ElementName = "body")]
#endif
        [JsonProperty("body")]
        public OpmlBody OpmlBody { get; set; }
    }
}
