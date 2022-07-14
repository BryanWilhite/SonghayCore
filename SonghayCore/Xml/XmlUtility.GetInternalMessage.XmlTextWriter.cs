namespace Songhay.Xml;

public static partial class XmlUtility
{
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
}
