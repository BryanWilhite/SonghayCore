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
}
