namespace Songhay.Xml;

public static partial class XmlUtility
{
    /// <summary>
    /// The conventional HTML entities mapped to their respective glyphs.
    /// </summary>
    public static readonly Dictionary<string, string> ConventionalHtmlEntities = new()
    {
        { "&amp;", "&" },
        { "&lt;", "<" },
        { "&gt;", ">" },
        { "&quot;", "\"" },
        { "&apos;", "'" },
    };

    /// <summary>
    /// Transforms selected XML glyphs
    /// into their respective HTML entities.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <seealso cref="LatinGlyphsUtility.Expand"/>
    /// <remarks>This member will preserve HTML comments.</remarks>
    public static string? XmlEncode(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return value;

        foreach (var pair in ConventionalHtmlEntities)
        {
            value = value.Replace(pair.Value, pair.Key);
        }

        //Preserve comments:
        value = value
            .Replace("--&gt;", "-->")
            .Replace("&lt;!--", "<!--");

        return value;
    }

    /// <summary>
    /// Transforms selected HTML entities
    /// into their respective glyphs.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <seealso cref="LatinGlyphsUtility.Condense"/>
    public static string? XmlDecode(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return value;

        foreach (var pair in ConventionalHtmlEntities)
        {
            value = value.Replace(pair.Key, pair.Value);
        }

        return value;
    }
}
