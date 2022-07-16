namespace Songhay.Models;

/// <summary>
/// Defines a managed representation of the OPML head element.
/// </summary>
[Serializable]
[XmlRoot(ElementName = "head")]
[DataContract(Name = "head")]
public class OpmlHead
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OpmlHead"/> class.
    /// </summary>
    public OpmlHead()
    {
        DateCreated = DateTime.Now;
        DateModified = DateTime.Now;
    }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    [XmlElement(ElementName = "title")]
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the date created.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public DateTime? DateCreated
    {
        get => _dateCreated;
        set
        {
            _dateCreated = value;
            DateCreatedString = value.HasValue ? ProgramTypeUtility.ConvertDateTimeToRfc822DateTime(value.Value) : null;
        }
    }

    /// <summary>
    /// Gets the date created string.
    /// </summary>
    [XmlElement(ElementName = "dateCreated")]
    [JsonPropertyName("dateCreated")]
    public string? DateCreatedString { get; set; }

    /// <summary>
    /// Gets or sets the date modified.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public DateTime? DateModified
    {
        get => _dateModified;
        set
        {
            _dateModified = value;
            DateModifiedString =
                value.HasValue ? ProgramTypeUtility.ConvertDateTimeToRfc822DateTime(value.Value) : null;
        }
    }

    /// <summary>
    /// Gets the date created string.
    /// </summary>
    [XmlElement(ElementName = "dateModified")]
    [JsonPropertyName("dateModified")]
    public string? DateModifiedString { get; set; }

    /// <summary>
    /// Gets or sets the name of the owner.
    /// </summary>
    [XmlElement(ElementName = "ownerName")]
    [JsonPropertyName("ownerName")]
    public string? OwnerName { get; set; }

    /// <summary>
    /// Gets or sets the owner email.
    /// </summary>
    [XmlElement(ElementName = "ownerEmail")]
    [JsonPropertyName("ownerEmail")]
    public string? OwnerEmail { get; set; }

    DateTime? _dateCreated;
    DateTime? _dateModified;
}
