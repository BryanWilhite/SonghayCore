namespace Songhay.Xml;

public static partial class XmlUtility
{
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
