namespace Songhay.Xml;

public static partial class XObjectUtility
{
    /// <summary>
    /// Glyph: Non-Breaking Space
    /// </summary>
    public static readonly string GlyphNonBreakingSpace = " ";

    /// <summary>
    /// <see cref="XText"/>: Non-Breaking Space
    /// </summary>
    public static XText XTextNonBreakingSpace => new(GlyphNonBreakingSpace);

    /// <summary>
    /// Joins the flattened <see cref="XText"/> nodes.
    /// </summary>
    /// <param name="rootElement">The root element.</param>
    public static string? JoinFlattenedXTextNodes(XElement? rootElement) =>
        JoinFlattenedXTextNodes(rootElement, includeRootElement: false, joinDelimiter: string.Empty);

    /// <summary>
    /// Joins the flattened <see cref="XText"/> nodes.
    /// </summary>
    /// <param name="rootElement">The root element.</param>
    /// <param name="includeRootElement">When <c>true</c> include root element.</param>
    public static string? JoinFlattenedXTextNodes(XElement? rootElement, bool includeRootElement) =>
        JoinFlattenedXTextNodes(rootElement, includeRootElement, joinDelimiter: string.Empty);

    /// <summary>
    /// Joins the flattened <see cref="XText"/> nodes.
    /// </summary>
    /// <param name="rootElement">The root element.</param>
    /// <param name="includeRootElement">When <c>true</c> include root element.</param>
    /// <param name="joinDelimiter">The join delimiter.</param>
    public static string? JoinFlattenedXTextNodes(XElement? rootElement, bool includeRootElement, string? joinDelimiter)
    {
        if (rootElement == null) return null;

        if (string.IsNullOrWhiteSpace(joinDelimiter)) joinDelimiter = string.Empty;

        var nodes = includeRootElement
            ? rootElement.DescendantNodesAndSelf().Where(i => i.NodeType == XmlNodeType.Text)
            : rootElement.DescendantNodes().Where(i => i.NodeType == XmlNodeType.Text);

        var displayText = string.Join(joinDelimiter, nodes.Select(i => i.ToString()).ToArray());

        return displayText;
    }
}
