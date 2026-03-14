namespace Songhay.Xml;

/// <summary>
/// Static members for HTML text processing.
/// </summary>
public static partial class HtmlUtility
{
    /// <summary>
    /// Returns a string of marked up text compatible
    /// with browsers that do not support XHTML
    /// (loosely towards HTML 4.x W3C standard).
    /// </summary>
    /// <param name="input">A <see cref="string"/> of markup.</param>
    public static string? ConvertToHtml(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

        //Minimize selected XHTML block elements.
        input = MatchXhtmlEndTagsThatShouldBeMinimized().Replace(input, string.Empty);

        //Remove XHTML html element attributes.
        input = MatchHtmlTagWithAnyAttributes().Replace(input, "<html>");

        //Remove XHTML element minimization.
        input = MatchXhtmlMinimizedEndChars().Replace(input, ">");

        //Remove XHTML attribute minimization.
        foreach (Match mTag in MatchHtmlTagContents().Matches(input))
        {
            //An opening input element has been found.
            string strReplace = mTag.Value;
            foreach (Match mAttr in MatchXhtmlAttribute().Matches(strReplace))
            {
                //XHTML minimization found (e.g. foo="foo").
                strReplace
                    = strReplace.Replace(mAttr.Value,
                        string.Concat(" ", mAttr.Groups[1].Value));
            }

            input = input.Replace(mTag.Value, strReplace);
        }

        return input;
    }

    /// <summary>
    /// Attempts to convert HTML to well-formed XML.
    /// </summary>
    /// <param name="html">An HTML <see cref="string"/>.</param>
    /// <remarks>This method is simpler than converting to XHTML.</remarks>
    public static string? ConvertToXml(string? html)
    {
        if (string.IsNullOrWhiteSpace(html)) return null;

        //Remove xmlns attributes:
        html = MatchXmlNamespaceAttributes().Replace(html, string.Empty);

        //Close open elements:
        html = MatchXhtmlSelfClosingTags().Replace(html, EvaluateOpenElement);

        //Find attribute minimization:
        html = MatchHtmlStartTags().Replace(html, EvaluateElementForMinimizedAttribute);

        //Find attributes without quotes:
        html = MatchHtmlStartTags().Replace(html, EvaluateElementForMalformedAttribute);

        //Generate attributes:
        html = MatchHtmlBooleanAttributes().Replace(html, EvaluateBooleanAttribute);

        //Look for Query strings with raw ampersands:
        foreach (Match m in MatchHtmlHrefOrSrcAttributes().Matches(html))
        {
            if (!m.Value.Contains("&amp;")) html = html.Replace(m.Value, m.Value.Replace("&", "&amp;"));
        }

        //Replace the CDATA "xmlns" with "x…mlns" (adds a soft-hyphen):
        html = html.Replace("xmlns", "x…mlns");

        return html;
    }

    /// <summary>
    /// Returns an XHTML string derived from a .NET procedure.
    /// </summary>
    /// <param name="xmlFragment">
    /// A well-formed <see cref="string"/> of XML.
    /// </param>
    /// <remarks>
    /// This member addresses certain quirks
    /// that well-formed XML cannot have in a contemporary Web browser.
    /// </remarks>
    public static string? FormatXhtmlElements(string? xmlFragment)
    {
        if (string.IsNullOrWhiteSpace(xmlFragment)) return null;

        //Maximize selected minimized elements.
        foreach (Match m in MatchHtmlElementsThatShouldNotBeMinimized().Matches(xmlFragment))
        {
            if (m.Groups.Count != 2) continue;

            string newValue = m.Value.Replace(m.Groups[1].Value, string.Concat("></", m.Groups[0].Value, ">"));
            xmlFragment = xmlFragment.Replace(m.Value, newValue);
        }

        return xmlFragment;
    }

    /// <summary>
    /// Returns the …inner… fragment of XML
    /// from the specified unique element.
    /// </summary>
    /// <param name="xmlFragment">
    /// A well-formed <see cref="string"/> of XML.
    /// </param>
    /// <param name="elementName">
    /// The local name of the element in the XML string.
    /// </param>
    /// <param name="newLine">the conventional <see cref="Environment.NewLine"/> characters of the lines</param>
    /// <param name="numberOfChars">the number of white space characters to remove</param>
    public static string? GetInnerXml(string? xmlFragment, string? elementName, string newLine = "\r\n", byte numberOfChars = 4)
    {
        if (string.IsNullOrWhiteSpace(xmlFragment)) return null;

        string pattern = RegexUtility.GetInnerXmlPattern(elementName);
        foreach (Match m in Regex.Matches(xmlFragment, pattern, RegexOptions.IgnoreCase))
        {
            if (m.Groups.Count > 1) xmlFragment = m.Groups[1].Value;

            break;
        }

        //Remove first four spaces at start of line.
        pattern = RegexUtility.GetLinesStartingWithWhitespaceCharacters(newLine, numberOfChars);
        xmlFragment = Regex.Replace(xmlFragment, pattern, newLine);

        return xmlFragment;
    }

    /// <summary>
    /// Emits a public <c>DOCTYPE</c> tag.
    /// </summary>
    /// <param name="rootElement">
    /// The root element of the DTD.
    /// </param>
    /// <param name="publicIdentifier">
    /// The public identifier of the DTD.
    /// </param>
    /// <param name="resourceReference">
    /// The link to reference material of the DTD.
    /// </param>
    /// <returns>
    /// A public <c>DOCTYPE</c> tag.
    /// </returns>
    public static string PublicDocType(string? rootElement = "html",
        string? publicIdentifier = "-//W3C//DTD XHTML 1.0 Transitional//EN",
        string? resourceReference = "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd") =>
        string.Format(CultureInfo.InvariantCulture, "<!DOCTYPE {0} PUBLIC \"{1}\" \"{2}\">",
            rootElement, publicIdentifier, resourceReference);

    private static string EvaluateBooleanAttribute(Match match)
    {
        var s = match.Value;

        if (match.Groups.Count != 2) return s;

        var group1Value = match.Groups[1].Value;
        s = match.Groups[0].Value.Replace(group1Value,
            string.Format(CultureInfo.InvariantCulture, """
                                                        {0}="{0}"
                                                        """, group1Value));

        return s;
    }

    private static string EvaluateElementForMalformedAttribute(Match match) => 
        MatchHtmlAttributes().Replace(match.Value, EvaluateMalformedAttribute);

    private static string EvaluateElementForMinimizedAttribute(Match match)
    {
        string s = match.Value;

        if (MatchHtmlClosingTagCharacters().IsMatch(s)) return s; //ignore closing element

        const string placeholderPrefix = "!*m";

        string placeholderTemplate = string.Concat(placeholderPrefix, "{0}");

        //remove strings between quotes:
        MatchCollection betweenQuotes = MatchAllCharactersInQuotes().Matches(s);
        foreach (Match m in betweenQuotes)
        {
            string placeholder = string.Format(placeholderTemplate, m.Index);
            s = s.Replace(m.Value, string.Format(placeholder, m.Index));
        }

        //evaluate what was not removed:
        foreach (Match m in MatchAllWords().Matches(s))
        {
            if (m.Index == 1) continue; //match should not be element name
            if (m.Value.Contains('=')) continue; //match should not be attribute-value pair

            s = s.Replace(m.Value,
                $"""
                 {m.Value}="{m.Value}"
                 """);
        }

        //restore strings between quotes:
        foreach (Match m in betweenQuotes)
        {
            string reArg = string.Concat(Regex.Escape(placeholderPrefix), m.Index, @"\b");
            Regex re = new(reArg);
            s = re.Replace(s, m.Value, 1);
        }

        return s;
    }

    private static string EvaluateMalformedAttribute(Match match)
    {
        var s = match.Value;

        if (match.Groups.Count != 4 || s.Contains('\'')) return s;

        return s.Contains('"')
            ? s
            : $""" {match.Groups[1].Value.Trim()}{match.Groups[2].Value.Trim()}"{match.Groups[3].Value.Trim()}" """;
    }

    private static string EvaluateOpenElement(Match match)
    {
        string s = match.Value;

        if (match.Groups.Count != 4) return s;

        //Refuse closed elements:
        if (match.Groups[2].Value.Trim().EndsWith("/", StringComparison.OrdinalIgnoreCase))
        {
            s = MatchAllSpaceCharactersRepeatedTwoOrMoreTimes().Replace(s, " ");

            return s;
        }

        s = $"<{match.Groups[1].Value} {match.Groups[2].Value.Trim()}{match.Groups[3].Value}".Replace(">", " />");

        s = MatchAllSpaceCharactersRepeatedTwoOrMoreTimes().Replace(s, " ");

        return s;
    }

    [GeneratedRegex(RegexScalars.AllCharactersInQuotes)]
    private static partial Regex MatchAllCharactersInQuotes();

    [GeneratedRegex(RegexScalars.AllSpaceCharactersRepeatedTwoOrMoreTimes)]
    private static partial Regex MatchAllSpaceCharactersRepeatedTwoOrMoreTimes();

    [GeneratedRegex(RegexScalars.AllWords)]
    private static partial Regex MatchAllWords();

    [GeneratedRegex(RegexScalars.HtmlAttributes, RegexOptions.IgnoreCase)]
    private static partial Regex MatchHtmlAttributes();

    [GeneratedRegex(RegexScalars.HtmlBooleanAttributes, RegexOptions.IgnoreCase)]
    private static partial Regex MatchHtmlBooleanAttributes();

    [GeneratedRegex(RegexScalars.HtmlClosingTagCharacters)]
    private static partial Regex MatchHtmlClosingTagCharacters();

    [GeneratedRegex(RegexScalars.HtmlElementsThatShouldNotBeMinimized, RegexOptions.IgnoreCase)]
    private static partial Regex MatchHtmlElementsThatShouldNotBeMinimized();

    [GeneratedRegex(RegexScalars.HtmlHrefOrSrcAttributes, RegexOptions.IgnoreCase)]
    private static partial Regex MatchHtmlHrefOrSrcAttributes();

    [GeneratedRegex(RegexScalars.HtmlStartTags)]
    private static partial Regex MatchHtmlStartTags();

    [GeneratedRegex(RegexScalars.HtmlTagContents)]
    private static partial Regex MatchHtmlTagContents();

    [GeneratedRegex(RegexScalars.HtmlTagWithAnyAttributes, RegexOptions.IgnoreCase)]
    private static partial Regex MatchHtmlTagWithAnyAttributes();

    [GeneratedRegex(RegexScalars.XhtmlAttribute)]
    private static partial Regex MatchXhtmlAttribute();

    [GeneratedRegex(RegexScalars.XhtmlEndTagsThatShouldBeMinimized, RegexOptions.IgnoreCase)]
    private static partial Regex MatchXhtmlEndTagsThatShouldBeMinimized();

    [GeneratedRegex(RegexScalars.XhtmlMinimizedEndChars)]
    private static partial Regex MatchXhtmlMinimizedEndChars();

    [GeneratedRegex(RegexScalars.XhtmlSelfClosingTags, RegexOptions.IgnoreCase)]
    private static partial Regex MatchXhtmlSelfClosingTags();

    [GeneratedRegex(RegexScalars.XmlNamespaceAttributes)]
    private static partial Regex MatchXmlNamespaceAttributes();
}
