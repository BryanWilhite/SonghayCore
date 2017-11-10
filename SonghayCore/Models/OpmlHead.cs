using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace Songhay.Models
{
    /// <summary>
    /// Defines a managed representation of the OPML head element.
    /// </summary>
#if !NETSTANDARD1_2
    [Serializable]
    [XmlRoot(ElementName = "head")]
#endif
    [JsonObject("head", MemberSerialization = MemberSerialization.OptIn)]
    public class OpmlHead
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpmlHead"/> class.
        /// </summary>
        public OpmlHead()
        {
            this.DateCreated = DateTime.Now;
            this.DateModified = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
#if !NETSTANDARD1_2
        [XmlElement(ElementName = "title")]
#endif
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>The date created.</value>
#if !NETSTANDARD1_2
        [XmlIgnore]
#endif
        public DateTime? DateCreated
        {
            get
            {
                return _dateCreated;
            }
            set
            {
                _dateCreated = value;
                this.DateCreatedString = value.HasValue ?
                    FrameworkTypeUtility.ConvertDateTimeToRfc822DateTime(value.Value) :
                    null;
            }
        }

        /// <summary>
        /// Gets the date created string.
        /// </summary>
        /// <value>The date created string.</value>
#if !NETSTANDARD1_2
        [XmlElement(ElementName = "dateCreated")]
#endif
        [JsonProperty("dateCreated")]
        public string DateCreatedString { get; set; }

        /// <summary>
        /// Gets or sets the date modified.
        /// </summary>
        /// <value>The date modified.</value>
#if !NETSTANDARD1_2
        [XmlIgnore]
#endif
        public DateTime? DateModified
        {
            get
            {
                return _dateModified;
            }
            set
            {
                _dateModified = value;
                this.DateModifiedString = value.HasValue ?
                    FrameworkTypeUtility.ConvertDateTimeToRfc822DateTime(value.Value) :
                    null;
            }
        }

        /// <summary>
        /// Gets the date created string.
        /// </summary>
        /// <value>The date created string.</value>
#if !NETSTANDARD1_2
        [XmlElement(ElementName = "dateModified")]
#endif
        [JsonProperty("dateModified")]
        public string DateModifiedString { get; set; }

        /// <summary>
        /// Gets or sets the name of the owner.
        /// </summary>
        /// <value>The name of the owner.</value>
#if !NETSTANDARD1_2
        [XmlElement(ElementName = "ownerName")]
#endif
        [JsonProperty("ownerName")]
        public string OwnerName { get; set; }

        /// <summary>
        /// Gets or sets the owner email.
        /// </summary>
        /// <value>The owner email.</value>
#if !NETSTANDARD1_2
        [XmlElement(ElementName = "ownerEmail")]
#endif
        [JsonProperty("ownerEmail")]
        public string OwnerEmail { get; set; }

        DateTime? _dateCreated;
        DateTime? _dateModified;
    }
}
