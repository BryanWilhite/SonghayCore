using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Songhay.Models
{
    /// <summary>
    /// XHTML Documents
    /// </summary>
    [Serializable]
    public class XhtmlDocuments
    {
        /// <summary>
        /// Gets or sets the documents.
        /// </summary>
        /// <value>The documents.</value>
        [XmlElement("XhtmlDocument")]
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays",
            Justification = "Used for XML serialization.")]
        public XhtmlDocument[] Documents { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [XmlAttribute]
        public string Title { get; set; }
    }
}
