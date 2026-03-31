namespace Songhay.Models;

/// <summary>
/// Centralizes all <see cref="Regex"/> strings
/// </summary>
public static class RegexScalars
{
    /// <summary><see cref="Regex"/> pattern</summary>
    public const string AllCharactersIndicatingParentDirectory = @"\.\./|\.\.\\";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string AllCharactersInQuotes = @"([""'])(?:(?=(\\?))\2.)*?\1";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string AllCharactersNotAlphanumeric = "[^a-z^0-9]";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string AllCharactersNotNumeric = @"\D";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string AllSpaceCharactersRepeatedTwoOrMoreTimes = " {2,}";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string AllThatLooksLikeEmailAddress = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string AllThatLooksLikeUnc = @"^(\\(\\[^\s\\]+)+|([A-Za-z]:(\\)?|[A-z]:(\\[^\s\\]+)+))(\\)?$";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string AllThatLooksLikeXmlMarkup = @"<\s*([^>]+)\s*>";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string AllThatLooksLikeXmlSelfClosingTags = "<([^>]+)/>";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string AllWords = @"\w+";

    /// <summary><see cref="Regex"/> pattern</summary> 
    public const string CommandLineArgumentInQuotesFollowedByOtherArguments = """
                                                                              "[^"]+"|\s+.+
                                                                              """;
 
    /// <summary><see cref="Regex"/> pattern</summary>
    public const string HtmlAttributesWithoutQuotes = """\s([^'\"\s]+)(\s*=\s*)([^'\"\s]+)\s""";

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
    public const string XhtmlDocTypeDeclaration = @"\<\!DOCTYPE [^<]+\>";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string XhtmlEndTagsThatShouldBeMinimized = @"\s*</(base|isindex|link|meta)\s*>";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string XhtmlMinimizedEndChars = @"\s*/>";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string XhtmlSelfClosingTags = @"<\s*(br|hr|img|link|meta)([^>]*)(>)";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string XmlNamedEntities = @"\&\w+\;";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string XmlNamespaceAttributes = """\s*xmlns\s*=\s*"[^"]+"\s*""";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string XmlNamespaceDeclarations = """\s*(xmlns:?[^=]*=["][^"]*["])\s*""";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string XmlNamespacePrefixes = """\s*xmlns:?([^=]*)=["][^"]*["]\s*""";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string XmlNamespaceSchemaLocationAttributes = """\s*([a-zA-z0-9:]*schemaLocation\s*=["][^"]*["])\s*""";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string XmlNumericEntities = @"\&\#\d+\;";
}
