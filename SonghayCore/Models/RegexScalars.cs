namespace Songhay.Models;

/// <summary>
/// Centralizes all <see cref="Regex"/> strings
/// </summary>
public static class RegexScalars
{
    /// <summary><see cref="Regex"/> pattern</summary>
    public const string HtmlTagContents = "<[^/][^>]*>";
 
    /// <summary><see cref="Regex"/> pattern</summary>
    public const string HtmlTagWithAnyAttributes = "<html*>";

    /// <summary><see cref="Regex"/> pattern</summary>
    /// <remarks>This expression intends to match XHTML-style attributes like <c>foo="foo"</c></remarks>
    public const string XhtmlAttribute = """
                                        \s+(.+)\s*=\s*"\1"
                                        """;

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string XhtmlEndTagsThatShouldBeMinimized = "</(base|isindex|link|meta)>";

    /// <summary><see cref="Regex"/> pattern</summary>
    public const string XhtmlMinimizedEndChars = @"\s*/>";
}
