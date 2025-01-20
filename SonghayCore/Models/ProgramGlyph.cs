namespace Songhay.Models;

/// <summary>
/// Defines a Unicode glyphic character
/// </summary>
public readonly record struct ProgramGlyph
{
    /// <summary>
    /// Gets or sets the unicode point.
    /// </summary>
    public string UnicodePoint { get; init; }

    /// <summary>
    /// Gets or sets the unicode integer.
    /// </summary>
    public int UnicodeInteger => string.IsNullOrWhiteSpace(UnicodePoint) ? 0 : Convert.ToInt32(UnicodePoint, 16);

    /// <summary>
    /// Gets or sets the unicode group.
    /// </summary>
    public string UnicodeGroup { get; init; }

    /// <summary>
    /// Gets or sets the name of the unicode.
    /// </summary>
    public string UnicodeName { get; init; }

    /// <summary>
    /// Gets or sets the character, usually the Unicode Point.
    /// </summary>
    public string Character { get; init; }

    /// <summary>
    /// Gets or sets the windows1252 URL encoding.
    /// </summary>
    public string Windows1252UrlEncoding { get; init; }

    /// <summary>
    /// Gets or sets the UTF8 URL encoding.
    /// </summary>
    public string Utf8UrlEncoding { get; init; }

    /// <summary>
    /// Gets or sets the name of the HTML entity.
    /// </summary>
    public string HtmlEntityName { get; init; }

    /// <summary>
    /// Gets or sets the XML entity number.
    /// </summary>
    public string XmlEntityNumber { get; init; }
}
