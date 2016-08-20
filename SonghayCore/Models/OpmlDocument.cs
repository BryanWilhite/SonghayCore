
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
#if !SILVERLIGHT
    [Serializable]
#endif
    [XmlRoot(ElementName = "opml", Namespace = "http://songhaysystem.com/schemas/opml.xsd")]
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
        [XmlAttribute("schemaLocation", Namespace = rxOpmlSchema)]
        [JsonProperty("schemaLocation")]
        public string XsiSchemaLocation { get; set; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        [XmlAttribute(AttributeName = "version")]
        [JsonProperty("version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets the OPML head element.
        /// </summary>
        /// <value>The OPML head element.</value>
        [XmlElement(ElementName = "head")]
        [JsonProperty("head")]
        public OpmlHead OpmlHead { get; set; }

        /// <summary>
        /// Gets the OPML body element.
        /// </summary>
        /// <value>The OPML body element.</value>
        [XmlElement(ElementName = "body")]
        [JsonProperty("body")]
        public OpmlBody OpmlBody { get; set; }
    }
}
