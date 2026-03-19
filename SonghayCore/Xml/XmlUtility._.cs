namespace Songhay.Xml;

/// <summary>
/// Static helper members for XML-related routines.
/// </summary>
/// <remarks>
/// These definitions are biased toward
/// emitting <see cref="XPathDocument"/> documents.
/// However, many accept any input implementing the
/// <see cref="IXPathNavigable"/> interface.
/// </remarks>
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
    /// Deserializes based on the specified XML file.
    /// </summary>
    /// <typeparam name="T">
    /// The specified type to deserialize.
    /// </typeparam>
    /// <param name="xmlPath">The XML file path.</param>
    /// <remarks>
    /// This member does not support <see cref="XPathDocument"/>
    /// and will return <c>null</c> when the type parameter is set
    /// to <see cref="XPathDocument"/>.
    /// </remarks>
    public static T? GetInstance<T>(string? xmlPath) where T : class
    {
        if (typeof(T).IsAssignableFrom(typeof(XPathDocument)))
        {
            return null;
        }

        xmlPath.ThrowWhenNullOrWhiteSpace();

        XmlSerializer serializer = new(typeof(T));

        using XmlReader reader = XmlReader.Create(xmlPath);
        T? instance = serializer.Deserialize(reader) as T;

        return instance;
    }

    /// <summary>
    /// Deserializes based on the specified raw XML.
    /// </summary>
    /// <typeparam name="T">
    /// The specified type to deserialize.
    /// </typeparam>
    /// <param name="xmlFragment">The raw XML.</param>
    /// <remarks>
    /// This member does not support <see cref="XPathDocument"/>
    /// and will return <c>null</c> when the type parameter is set
    /// to <see cref="XPathDocument"/>.
    /// </remarks>
    public static T? GetInstanceRaw<T>(string? xmlFragment) where T : class
    {
        if (typeof(T).IsAssignableFrom(typeof(XPathDocument)))
        {
            return null;
        }

        xmlFragment.ThrowWhenNullOrWhiteSpace();

        XmlSerializer serializer = new XmlSerializer(typeof(T));

        using StringReader reader = new StringReader(xmlFragment);
        T? instance = serializer.Deserialize(reader) as T;

        return instance;
    }

    /// <summary>
    /// Returns <c>true</c> when the fragment is XML-like.
    /// </summary>
    /// <param name="fragment">The fragment to test.</param>
    public static bool IsXml(string? fragment)
    {
        if (string.IsNullOrWhiteSpace(fragment)) return false;

        Match xmlMatch = MatchAllThatLooksLikeXmlMarkup().Match(fragment);
        Match xmlMatchMinimized = MatchAllThatLooksLikeXmlSelfClosingTags().Match(fragment);

        return xmlMatch.Success || xmlMatchMinimized.Success;
    }

    /// <summary>
    /// Strip the namespaces from specified <see cref="string"/>.
    /// </summary>
    /// <param name="xml">
    /// The source <see cref="string"/>.
    /// </param>
    /// <remarks>
    /// WARNING: Stripping namespaces “flattens” the document
    /// and can cause local-name collisions.
    /// 
    /// This routine does not remove namespace prefixes.
    /// 
    /// </remarks>
    public static string? StripNamespaces(string xml) => StripNamespaces(xml, false);

    /// <summary>
    /// Strip the namespaces from specified <see cref="string"/>.
    /// </summary>
    /// <param name="xml">
    /// The source <see cref="string"/>.
    /// </param>
    /// <param name="removeDocType">
    /// When <c>true</c>, removes any DOCTYPE preambles.
    /// </param>
    /// <remarks>
    /// WARNING: Stripping namespaces “flattens” the document
    /// and can cause local-name collisions.
    /// 
    /// This routine does not remove namespace prefixes.
    /// 
    /// </remarks>
    public static string? StripNamespaces(string? xml, bool removeDocType)
    {
        if (string.IsNullOrWhiteSpace(xml)) return null;

        if (removeDocType)
        {
            xml = MatchXhtmlDocTypeDeclaration().Replace(xml, string.Empty);
        }

        //Attempt to remove all namespace prefixes:
        foreach (Match m in MatchXmlNamespacePrefixes().Matches(xml))
        {
            if (m.Groups.Count != 2) continue;

            string pattern = m.Groups[1].Value;
            pattern = string.Concat("<", pattern, ":");
            xml = xml.Replace(pattern, "<");

            pattern = m.Groups[1].Value;
            pattern = string.Concat("</", pattern, ":");
            xml = xml.Replace(pattern, "</");
        }

        //Attempt to remove namespace declarations:
        xml = MatchXmlNamespaceDeclarations().Replace(xml, string.Empty);

        //Attempt to remove schemaLocation attribute:
        xml = MatchXmlNamespaceSchemaLocationAttributes().Replace(xml, string.Empty);

        return !string.IsNullOrWhiteSpace(xml) ? xml.Trim() : null;
    }

    /// <summary>
    /// Serializes and writes to the specified path.
    /// </summary>
    /// <typeparam name="T">The type of the instance.</typeparam>
    /// <param name="instance">The instance.</param>
    /// <param name="path">The path.</param>
    public static void Write<T>(T? instance, string? path) where T : class
    {
        ArgumentNullException.ThrowIfNull(instance);

        path.ThrowWhenNullOrWhiteSpace();

        XmlSerializer serializer = new XmlSerializer(typeof(T));

        XmlWriterSettings settings = new XmlWriterSettings
        {
            Encoding = Encoding.UTF8,
            Indent = true,
            IndentChars = "    ",
            NewLineOnAttributes = false,
            OmitXmlDeclaration = false
        };

        using XmlWriter writer = XmlWriter.Create(path, settings);

        serializer.Serialize(writer, instance);
    }

    /// <summary>
    /// Transfers the data in the Source to the Destination.
    /// </summary>
    /// <param name="readerSource">The <see cref="XmlReader"/>.</param>
    /// <param name="writerDestination">The <see cref="XmlWriter"/>.</param>
    public static void WriteReader(XmlReader? readerSource, XmlWriter? writerDestination)
    {
        ArgumentNullException.ThrowIfNull(readerSource);
        ArgumentNullException.ThrowIfNull(writerDestination);

        while (readerSource is {EOF: false}) writerDestination.WriteNode(readerSource, false);
    }

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

        value = ConventionalHtmlEntities.Aggregate(value, (current, pair) => current.Replace(pair.Value, pair.Key));

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
    public static string? XmlDecode(string? value) => string.IsNullOrWhiteSpace(value) ? value : ConventionalHtmlEntities.Aggregate(value, (current, pair) => current.Replace(pair.Key, pair.Value));
}
