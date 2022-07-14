namespace Songhay.Xml;

/// <summary>
/// Static members for HTML text processing.
/// </summary>
public static class HtmlUtility
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
        input
            = Regex.Replace(input, @"</(base|isindex|link|meta)>",
                string.Empty, RegexOptions.IgnoreCase);

        //Remove XHTML html element attributes.
        input
            = Regex.Replace(input, @"<html*>",
                "<html>", RegexOptions.IgnoreCase);

        //Remove XHTML element minimization.
        input = Regex.Replace(input, @"\s*/>", ">");

        //Remove XHTML attribute minimization.
        foreach (Match mTag in Regex.Matches(input, @"<[^/][^>]*>"))
        {
            //An opening input element has been found.
            string strReplace = mTag.Value;
            foreach (Match mAttr in Regex.Matches(strReplace, @"\s+(.+)\s*=\s*""\1"""))
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
    /// <remarks>This task is simpler than converting to XHTML.</remarks>
    public static string? ConvertToXml(string? html)
    {
        if (string.IsNullOrWhiteSpace(html)) return null;

        Regex re;
        MatchEvaluator me;

        //Remove xmlns attributes:
        html = Regex.Replace(html, @"\s*xmlns\s*=\s*""[^""]+""\s*", string.Empty);

        //Close open elements:
        me = EvaluateOpenElement;

        re = new Regex(@"<\s*(br|hr|img|link|meta)([^>]*)(>)", RegexOptions.IgnoreCase);
        html = re.Replace(html, me);

        //Find attribute minimization:
        me = EvaluateElementForMinimizedAttribute;

        re = new Regex(@"<[^>]+>", RegexOptions.IgnoreCase);
        html = re.Replace(html, me);

        //Find attributes without quotes:
        me = EvaluateElementForMalformedAttribute;

        re = new Regex(@"<[^>]+>", RegexOptions.IgnoreCase);
        html = re.Replace(html, me);

        //Generate attributes:
        me = EvaluateAttribute;

        re = new Regex(@"<\s*[^>]+\s(checked|nobreak|nosave|selected)[^=>]*\/*>", RegexOptions.IgnoreCase);
        html = re.Replace(html, me);

        //Look for Query strings with raw ampersands:
        foreach (Match m in Regex.Matches(html, @"href\s*=\s*""[^""]+"""))
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

        //Maximize selected empty minimized block elements.
        foreach (Match m in Regex.Matches(xmlFragment, @"<(a|iframe|td|th|script)\s+[^>]*\s*(\/>)",
                     RegexOptions.IgnoreCase))
        {
            if (m.Groups.Count == 2)
            {
                var newValue = m.Value.Replace(m.Groups[1].Value,
                    string.Concat("></", m.Groups[0].Value, ">"));
                xmlFragment = xmlFragment.Replace(m.Value, newValue);
            }
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
    public static string? GetInnerXml(string? xmlFragment, string? elementName)
    {
        if (string.IsNullOrWhiteSpace(xmlFragment)) return null;

        string ret = xmlFragment;

        string pattern = string.Format(CultureInfo.InvariantCulture, @"<{0}[^>]*>((\s*.+\s*)+)<\/{0}>", elementName);
        foreach (Match m in Regex.Matches(ret, pattern, RegexOptions.IgnoreCase))
        {
            if (m.Groups.Count > 1) ret = m.Groups[1].Value;
            break;
        }

        //Remove first four spaces at start of line.
        ret = Regex.Replace(ret, @"\r\n\W{4}", "\r\n");

        return ret;
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

    #region Regular Expression Match Evaluators

    static string EvaluateAttribute(Match match)
    {
        var s = match.Value;
        if (match.Groups.Count != 2) return s;

        var group1Value = match.Groups[1].Value;
        s = match.Groups[0].Value.Replace(group1Value,
            string.Format(CultureInfo.InvariantCulture, @"{0}=""{0}""", group1Value));

        return s;
    }

    static string EvaluateElementForMalformedAttribute(Match match)
    {
        var s = match.Value;
        var re = new Regex(@"([^\""\s]+)(\s*=\s*)([^\""\s]+)\s", RegexOptions.IgnoreCase);
        var me = new MatchEvaluator(EvaluateMalformedAttribute);
        return re.Replace(s, me);
    }

    static string EvaluateElementForMinimizedAttribute(Match match)
    {
        var s = match.Value;

        var re = new Regex(@"\<\s*/");
        if (re.IsMatch(s)) return s; //ignore closing element

        var placeholderPrefix = "!*m";
        var placeholderTemplate = string.Concat(placeholderPrefix, "{0}");

        //remove strings between quotes:
        var betweenQuotes = Regex.Matches(s, @"([""'])(?:(?=(\\?))\2.)*?\1", RegexOptions.IgnoreCase);
        foreach (Match m in betweenQuotes)
        {
            var placeholder = string.Format(placeholderTemplate, m.Index);
            s = s.Replace(m.Value, string.Format(placeholder, m.Index));
        }

        //evaluate what was not removed:
        var possibilities = Regex.Matches(s, @"(\b[^\s]+\b)", RegexOptions.IgnoreCase);
        foreach (Match m in possibilities)
        {
            if (m.Index == 1) continue; //match should not be element name
            if (m.Value.Contains('=')) continue; //match should not be attribute-value pair
            s = s.Replace(m.Value, string.Format(@"{0}=""{0}""", m.Value));
        }

        //restore strings between quotes:
        foreach (Match m in betweenQuotes)
        {
            var reArg = string.Concat(Regex.Escape(placeholderPrefix), m.Index, @"\b");
            re = new Regex(reArg);
            s = re.Replace(s, m.Value, 1);
        }

        return s;
    }

    static string EvaluateMalformedAttribute(Match match)
    {
        var s = match.Value;
        if (match.Groups.Count != 4) return s;
        if (s.Contains('\'')) return s;

        return s.Contains('"')
            ? s
            : $@" {match.Groups[1].Value.Trim()}{match.Groups[2].Value.Trim()}""{match.Groups[3].Value.Trim()}"" ";
    }

    static string EvaluateOpenElement(Match match)
    {
        var s = match.Value;
        if (match.Groups.Count != 4) return s;

        //Refuse closed elements:
        if (match.Groups[2].Value.Trim().EndsWith("/", StringComparison.OrdinalIgnoreCase)) return s;

        string oldValue = match.Groups[3].Value;

        if (oldValue.IndexOf(">", StringComparison.OrdinalIgnoreCase) != -1) s = s.Replace(oldValue, " />");

        return s;
    }

    #endregion
}
