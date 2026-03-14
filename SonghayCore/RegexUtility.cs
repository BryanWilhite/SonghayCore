namespace Songhay;

/// <summary>
/// Shared routines for <see cref="Regex"/>
/// </summary>
public static class RegexUtility
{
    /// <summary><see cref="Regex"/> pattern getter</summary>
    public static string GetInnerXmlPattern(string? elementName) =>
        string.Format(CultureInfo.InvariantCulture, @"<{0}[^>]*>((\s*.+\s*)+)<\/{0}>", elementName);

    /// <summary><see cref="Regex"/> pattern getter</summary>
    /// <param name="newLine">the conventional <see cref="Environment.NewLine"/> characters of the lines</param>
    /// <param name="numberOfChars">the number of white space characters to remove</param>
    public static string GetLinesStartingWithWhitespaceCharacters(string newLine = "\r\n", byte numberOfChars = 4) =>
        $@"{newLine}\W{{{numberOfChars}}}";
}
