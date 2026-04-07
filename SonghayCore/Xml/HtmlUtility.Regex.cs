// ReSharper disable MemberCanBePrivate.Global
namespace Songhay.Xml;

/// <summary>
/// Static members for HTML text processing.
/// </summary>
public static partial class HtmlUtility
{
    /// <summary><see cref="Regex"/> pattern getter</summary>
    public static string GetHtmlBooleanAttributesPattern()
    {
        string[] attrs = GetBooleanAttributes();
        string joined = string.Join('|', attrs);

        return $@"(?: )({joined})\b";
    }

    [GeneratedRegex(RegexScalars.HtmlAttributesWithoutQuotes, RegexOptions.IgnoreCase)]
    public static partial Regex MatchHtmlAttributesWithoutQuotes();

    [GeneratedRegex(RegexScalars.HtmlClosingTagCharacters)]
    public static partial Regex MatchHtmlClosingTagCharacters();

    [GeneratedRegex(RegexScalars.HtmlElementsThatShouldNotBeMinimized, RegexOptions.IgnoreCase)]
    public static partial Regex MatchHtmlElementsThatShouldNotBeMinimized();

    [GeneratedRegex(RegexScalars.HtmlHrefOrSrcAttributes, RegexOptions.IgnoreCase)]
    public static partial Regex MatchHtmlHrefOrSrcAttributes();

    [GeneratedRegex(RegexScalars.HtmlStartTags)]
    public static partial Regex MatchHtmlStartTags();

    [GeneratedRegex(RegexScalars.HtmlTagContents)]
    public static partial Regex MatchHtmlTagContents();

    [GeneratedRegex(RegexScalars.HtmlTagWithAnyAttributes, RegexOptions.IgnoreCase)]
    public static partial Regex MatchHtmlTagWithAnyAttributes();

    [GeneratedRegex(RegexScalars.XhtmlAttribute)]
    public static partial Regex MatchXhtmlAttribute();

    [GeneratedRegex(RegexScalars.XhtmlEndTagsThatShouldBeMinimized, RegexOptions.IgnoreCase)]
    public static partial Regex MatchXhtmlEndTagsThatShouldBeMinimized();

    [GeneratedRegex(RegexScalars.XhtmlMinimizedEndChars)]
    public static partial Regex MatchXhtmlMinimizedEndChars();

    [GeneratedRegex(RegexScalars.XhtmlSelfClosingTags, RegexOptions.IgnoreCase)]
    public static partial Regex MatchXhtmlSelfClosingTags();

    [GeneratedRegex(RegexScalars.XmlNamespaceAttributes)]
    public static partial Regex MatchXmlNamespaceAttributes();

    private static string EvaluateAttributeWithoutQuotes(Match match)
    {
        var s = match.Value;

        if (match.Groups.Count != 4 || s.Contains('\'')) return s;

        return s.Contains('"')
            ? s
            : $""" {match.Groups[1].Value.Trim()}="{match.Groups[3].Value.Trim()}" """;
    }

    private static string EvaluateElementForMalformedAttribute(Match match) => 
        MatchHtmlAttributesWithoutQuotes().Replace(match.Value, EvaluateAttributeWithoutQuotes);

    private static string EvaluateElementForMinimizedAttribute(Match match)
    {
        string s = match.Value;

        if (MatchHtmlClosingTagCharacters().IsMatch(s)) return s; //ignore closing element

        MatchCollection matches = Regex.Matches(s, GetHtmlBooleanAttributesPattern());

        foreach (Match m in matches)
        {
            if(m.Groups.Count != 2) continue;

            string booleanAttr = m.Groups[1].Value;
            string replacement = m.Value.Replace(booleanAttr, $"{booleanAttr}=\"{booleanAttr}\"");

            s = s.Replace(m.Value, replacement);
        }

        return s;
    }

    private static string EvaluateOpenElement(Match match)
    {
        string s = match.Value;

        if (match.Groups.Count != 4) return s;

        //Refuse closed elements:
        if (match.Groups[2].Value.Trim().EndsWith("/", StringComparison.OrdinalIgnoreCase))
        {
            s = RegexUtility.MatchAllSpaceCharactersRepeatedTwoOrMoreTimes().Replace(s, " ");

            return s;
        }

        s = $"<{match.Groups[1].Value} {match.Groups[2].Value.Trim()}{match.Groups[3].Value}".Replace(">", " />");

        s = RegexUtility.MatchAllSpaceCharactersRepeatedTwoOrMoreTimes().Replace(s, " ");

        return s;
    }
}
