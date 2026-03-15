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
    /// Attempts to convert HTML to well-formed XML
    /// with a conventional set of <see cref="Regex"/> strategies.
    /// </summary>
    /// <param name="html">An HTML <see cref="string"/>.</param>
    /// <remarks>
    /// This method is simpler than converting to XHTML.
    ///
    /// This member uses the following <see cref="Regex"/> strategies:
    ///
    /// - <see cref="MatchXmlNamespaceAttributes"/>
    /// - <see cref="MatchXhtmlSelfClosingTags"/>
    /// - <see cref="MatchHtmlStartTags"/>
    /// - <see cref="MatchHtmlBooleanAttributes"/>
    /// - <see cref="MatchHtmlHrefOrSrcAttributes"/>
    ///
    /// </remarks>
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
    /// Formats the specified fragment
    /// with a conventional set of <see cref="Regex"/> strategies.
    /// </summary>
    /// <param name="xmlFragment">
    /// A well-formed <see cref="string"/> of XML.
    /// </param>
    /// <remarks>
    /// This member uses the following <see cref="Regex"/> strategy:
    ///
    /// - <see cref="MatchHtmlElementsThatShouldNotBeMinimized"/>
    ///
    /// </remarks>
    public static string? FormatXhtmlElements(string? xmlFragment)
    {
        if (string.IsNullOrWhiteSpace(xmlFragment)) return null;

        //Maximize selected minimized elements.
        foreach (Match m in MatchHtmlElementsThatShouldNotBeMinimized().Matches(xmlFragment))
        {
            if (m.Groups.Count != 4) continue;

            string element = m.Groups[1].Value;
            string additional = m.Groups[2].Value.Trim();

            if (!string.IsNullOrWhiteSpace(additional)) additional = $" {additional}";

            string replacement = $"<{element}{additional}></{element}>";

            xmlFragment = xmlFragment.Replace(m.Value, replacement);
        }

        return xmlFragment;
    }

    /// <summary>Returns all known HTML Boolean attributes</summary>
    /// <remarks>See https://meiert.com/blog/boolean-attributes-of-html/</remarks>
    public static string[] GetBooleanAttributes() =>
        [
            "allowfullscreen",
            "alpha",
            "async",
            "autofocus",
            "autoplay",
            "checked",
            "controls",
            "default",
            "defer",
            "disabled",
            "formnovalidate",
            "inert",
            "ismap",
            "itemscope",
            "loop",
            "multiple",
            "muted",
            "nomodule",
            "novalidate",
            "open",
            "playsinline",
            "readonly",
            "required",
            "reversed",
            "selected",
            "shadowrootclonable",
            "shadowrootcustomelementregistry",
            "shadowrootdelegatesfocus",
            "shadowrootserializable",
        ];

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
        pattern = RegexUtility.GetLinesStartingWithWhitespaceCharactersPattern(newLine, numberOfChars);
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
}
