namespace Songhay.Xml;

public static partial class XmlUtility
{
    /// <summary>
    /// Returns an XML <see cref="string"/>
    /// based on the specified header and lines.
    /// </summary>
    /// <param name="messageHeader">Message header.</param>
    public static string GetInternalMessage(string messageHeader) =>
        GetInternalMessage(messageHeader, string.Empty, null);

    /// <summary>
    /// Returns an XML <see cref="string"/>
    /// based on the specified header and lines.
    /// </summary>
    /// <param name="messageHeader">Message header.</param>
    /// <param name="messageLines">Message lines.</param>
    public static string GetInternalMessage(string messageHeader, string[] messageLines) =>
        GetInternalMessage(messageHeader, string.Empty, messageLines);

    /// <summary>
    /// Returns an XML <see cref="string"/>
    /// based on the specified header and lines.
    /// </summary>
    /// <param name="messageHeader">Message header.</param>
    /// <param name="messageLines">Message lines.</param>
    public static string GetInternalMessage(string messageHeader, ReadOnlyCollection<string>? messageLines) =>
        GetInternalMessage(messageHeader, string.Empty, messageLines);

    /// <summary>
    /// Returns an XML <see cref="string"/>
    /// based on the specified header and lines.
    /// </summary>
    /// <param name="messageHeader">Message header.</param>
    /// <param name="messageCode">Message code for errors, exceptions or faults.</param>
    /// <param name="messageLines">Message lines.</param>
    public static string GetInternalMessage(string? messageHeader, string? messageCode,
        IEnumerable<string>? messageLines)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<InternalMessage>");

        sb.AppendLine(CultureInfo.InvariantCulture, $"<Header>{messageHeader}</Header>");
        if (!string.IsNullOrWhiteSpace(messageCode))
            sb.AppendLine(CultureInfo.InvariantCulture, $"<Code>{messageCode}</Code>");

        if (messageLines is not null)
        {
            foreach (string line in messageLines)
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"<Line>{line}</Line>");
            }
        }

        sb.AppendLine("</InternalMessage>");

        return sb.ToString();
    }

    /// <summary>
    /// Gets the conventional, XML <c>&lt;InternalMessage&gt;</c>.
    /// </summary>
    /// <param name="messageHeader">Message header.</param>
    /// <param name="messageLines">Message lines.</param>
    /// <param name="xmlDataWriter">The <see cref="XmlWriter"/>.</param>
    public static void GetInternalMessage(string? messageHeader, string[]? messageLines, XmlWriter? xmlDataWriter) =>
        GetInternalMessage(messageHeader, string.Empty, messageLines, xmlDataWriter, false);

    /// <summary>
    /// Gets the conventional, XML <c>&lt;InternalMessage&gt;</c>.
    /// </summary>
    /// <param name="messageHeader">Message header.</param>
    /// <param name="messageCode">Message code for errors, exceptions or faults.</param>
    /// <param name="messageLines">Message lines.</param>
    /// <param name="xmlDataWriter">The <see cref="XmlTextWriter"/>.</param>
    public static void GetInternalMessage(string? messageHeader, string? messageCode, string[]? messageLines,
        XmlWriter xmlDataWriter) => GetInternalMessage(messageHeader, messageCode, messageLines, xmlDataWriter, false);

    /// <summary>
    /// Gets the conventional, XML <c>&lt;InternalMessage&gt;</c>.
    /// </summary>
    /// <param name="messageHeader">Message header.</param>
    /// <param name="messageLines">Message lines.</param>
    /// <param name="xmlDataWriter">The <see cref="XmlWriter"/>.</param>
    /// <param name="isFragment">When <c>false</c> a new document is started.</param>
    public static void GetInternalMessage(string? messageHeader, string[]? messageLines, XmlWriter? xmlDataWriter,
        bool isFragment) => GetInternalMessage(messageHeader, string.Empty, messageLines, xmlDataWriter, isFragment);

    /// <summary>
    /// Gets the conventional, XML <c>&lt;InternalMessage&gt;</c>.
    /// </summary>
    /// <param name="messageHeader">Message header.</param>
    /// <param name="messageCode">Message code for errors, exceptions or faults.</param>
    /// <param name="messageLines">Message lines.</param>
    /// <param name="xmlDataWriter">The <see cref="XmlWriter"/>.</param>
    /// <param name="isFragment">When <c>false</c> a new document is started.</param>
    public static void GetInternalMessage(string? messageHeader, string? messageCode, string[]? messageLines,
        XmlWriter? xmlDataWriter, bool isFragment)
    {
        ArgumentNullException.ThrowIfNull(xmlDataWriter);

        if (!isFragment) xmlDataWriter.WriteStartDocument();
        xmlDataWriter.WriteStartElement("InternalMessage");

        xmlDataWriter.WriteElementString("Header", messageHeader);
        if (!string.IsNullOrWhiteSpace(messageCode)) xmlDataWriter.WriteElementString("Code", messageCode);

        if (messageLines is {Length: > 0})
        {
            foreach (string line in messageLines)
            {
                xmlDataWriter.WriteElementString("Line", line);
            }
        }

        xmlDataWriter.WriteFullEndElement();
        if (!isFragment) xmlDataWriter.WriteEndDocument();
    }
    /// <summary>
    /// Returns an <see cref="XPathDocument"/>
    /// based on the specified header and lines.
    /// </summary>
    /// <param name="messageHeader">Message header.</param>
    /// <param name="messageLines">Message lines.</param>
    [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
        Justification = "Specific functionality provided by the concrete type may be required.")]
    public static XPathDocument? GetInternalMessageDocument(string? messageHeader, string[]? messageLines) =>
        GetInternalMessageDocument(messageHeader, string.Empty, messageLines);

    /// <summary>
    /// Returns an <see cref="XPathDocument"/>
    /// based on the specified header and lines.
    /// </summary>
    /// <param name="messageHeader">Message header.</param>
    /// <param name="messageCode">Message code for errors, exceptions or faults</param>
    /// <param name="messageLines">Message lines.</param>
    [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
        Justification = "Specific functionality provided by the concrete type may be required.")]
    public static XPathDocument? GetInternalMessageDocument(string? messageHeader, string? messageCode,
        string[]? messageLines)
    {
        string s = GetInternalMessage(messageHeader, messageCode, messageLines);

        return GetNavigableDocument(s);
    }
}
