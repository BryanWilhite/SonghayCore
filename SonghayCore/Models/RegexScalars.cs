namespace Songhay.Models;

/// <summary>
/// Centralizes all <see cref="Regex"/> strings
/// </summary>
public static class RegexScalars
{
    /// <summary><see cref="Regex"/> pattern</summary>
    public const string AllCharactersInQuotes = @"([""'])(?:(?=(\\?))\2.)*?\1";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string AllSpaceCharactersRepeatedTwoOrMoreTimes = " {2,}";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string AllWords = @"\w+";
 
    /// <summary><see cref="Regex"/> pattern</summary>
    public const string HtmlAttributesWithoutQuotes = """\s([^'\"\s]+)(\s*=\s*)([^'\"\s]+)\s""";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string HtmlBooleanAttributes = @"<\s*[^>]+\s(checked|nobreak|nosave|selected)[^=>]*\/*>";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string HtmlClosingTagCharacters = @"\<\s*/";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string HtmlElementsThatShouldNotBeMinimized = @"<(a|iframe|td|th|script)\s+([^>]*)(/>)";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string HtmlHrefOrSrcAttributes = """
                                             (href|src)\s*=\s*['"][^"]+['"]
                                             """;  

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string HtmlStartTags = @"<[^>\/]+>";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string HtmlTagContents = "<[^/][^>]*>";
 
    /// <summary><see cref="Regex"/> pattern</summary>
    public const string HtmlTagWithAnyAttributes = "<html [^>]*>";

    /// <summary><see cref="Regex"/> pattern</summary>
    /// <remarks>This expression intends to match XHTML-style attributes like <c>foo="foo"</c></remarks>
    public const string XhtmlAttribute = """
                                        \s+(.+)\s*=\s*"\1"
                                        """;

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string XhtmlEndTagsThatShouldBeMinimized = @"\s*</(base|isindex|link|meta)\s*>";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string XhtmlMinimizedEndChars = @"\s*/>";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string XhtmlSelfClosingTags = @"<\s*(br|hr|img|link|meta)([^>]*)(>)";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string XmlNamespaceAttributes = """\s*xmlns\s*=\s*"[^"]+"\s*""";
}
