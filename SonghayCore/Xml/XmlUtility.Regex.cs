// ReSharper disable MemberCanBePrivate.Global
namespace Songhay.Xml;

public static partial class XmlUtility
{
    [GeneratedRegex(RegexScalars.AllThatLooksLikeXmlMarkup)]
    public static partial Regex MatchAllThatLooksLikeXmlMarkup();

    [GeneratedRegex(RegexScalars.AllThatLooksLikeXmlSelfClosingTags)]
    public static partial Regex MatchAllThatLooksLikeXmlSelfClosingTags();

    [GeneratedRegex(RegexScalars.XhtmlDocTypeDeclaration)]
    public static partial Regex MatchXhtmlDocTypeDeclaration();

    [GeneratedRegex(RegexScalars.XmlNamespaceDeclarations, RegexOptions.IgnoreCase)]
    public static partial Regex MatchXmlNamespaceDeclarations();

    [GeneratedRegex(RegexScalars.XmlNamespacePrefixes, RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    public static partial Regex MatchXmlNamespacePrefixes();

    [GeneratedRegex(RegexScalars.XmlNamespaceSchemaLocationAttributes, RegexOptions.IgnoreCase)]
    public static partial Regex MatchXmlNamespaceSchemaLocationAttributes();
}
