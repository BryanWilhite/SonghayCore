using System.Text.RegularExpressions;

namespace Songhay.Xml
{
    public static partial class XmlUtility
    {
        /// <summary>
        /// Returns <c>true</c> when the fragment is XML-like.
        /// </summary>
        /// <param name="fragment">The fragment to test.</param>
        public static bool IsXml(string fragment)
        {
            Match xmlMatch = Regex.Match(fragment, @"<([^>]+)>(.*?</(\1)>|[^>]*/>)");
            Match xmlMatchMinimized = Regex.Match(fragment, @"<([^>]+)/>");
            return (xmlMatch.Success || xmlMatchMinimized.Success);
        }
    }
}
