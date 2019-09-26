namespace Songhay.Xml
{
    public static partial class XmlUtility
    {
        /// <summary>
        /// Expands selected characters
        /// in the specified <see cref="System.String"/>
        /// into the standard XML entities.
        /// </summary>
        /// <param name="rawValue">The raw <see cref="System.String"/></param>
        public static string ExpandSpecialChars(string rawValue)
        {
            string s = rawValue;

            if (string.IsNullOrWhiteSpace(s)) return s;

            s = s.Replace("&", "&amp;");
            s = s.Replace("'", "&apos;");
            s = s.Replace(">", "&gt;");
            s = s.Replace("<", "&lt;");
            s = s.Replace("\"", "&quot;");

            //Preserve comments:
            s = s.Replace("--&gt;", "-->");
            s = s.Replace("&lt;!--", "<!--");

            return s;
        }
    }
}
