using System;
using System.Xml.Serialization;

namespace Songhay.Models
{
    /// <summary>
    /// Defines an XHTML Document.
    /// </summary>
    [Serializable]
    public class XhtmlDocument
    {
        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        [XmlAttribute]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        [XmlAttribute]
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [XmlAttribute]
        public string Title { get; set; }
    }
}
