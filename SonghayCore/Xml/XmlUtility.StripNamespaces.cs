using System.Text.RegularExpressions;

namespace Songhay.Xml
{
    public static partial class XmlUtility
    {
        /// <summary>
        /// Strip the namespaces from specified <see cref="System.String"/>.
        /// </summary>
        /// <param name="xml">
        /// The source <see cref="System.String"/>.
        /// </param>
        /// <remarks>
        /// WARNING: Stripping namespaces “flattens” the document
        /// and can cause local-name collisions.
        /// 
        /// This routine does not remove namespace prefixes.
        /// 
        /// </remarks>
        public static string StripNamespaces(string xml)
        {
            return XmlUtility.StripNamespaces(xml, false);
        }

        /// <summary>
        /// Strip the namespaces from specified <see cref="System.String"/>.
        /// </summary>
        /// <param name="xml">
        /// The source <see cref="System.String"/>.
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
        public static string StripNamespaces(string xml, bool removeDocType)
        {
            if(string.IsNullOrWhiteSpace(xml)) return null;

            if(removeDocType)
            {
                xml = Regex.Replace(xml,
                    @"\<\!DOCTYPE [^<]+\>", string.Empty,
                    RegexOptions.IgnoreCase);
            }

            //Attempt to remove all namespace prefixes:
            foreach(Match m in Regex.Matches(xml,
                @"\s*xmlns:?([^=]*)=[""][^""]*[""]\s*", RegexOptions.IgnoreCase | RegexOptions.Multiline))
            {
                if(m.Groups.Count == 2)
                {
                    string pattern = string.Empty;

                    pattern = m.Groups[1].Value;
                    pattern = string.Concat("<", pattern, ":");
                    xml = xml.Replace(pattern, "<");

                    pattern = m.Groups[1].Value;
                    pattern = string.Concat("</", pattern, ":");
                    xml = xml.Replace(pattern, "</");
                }
            }

            //Attempt to remove namespace declarations:
            xml = Regex.Replace(xml,
                @"\s*(xmlns:?[^=]*=[""][^""]*[""])\s*", string.Empty,
                RegexOptions.IgnoreCase);

            //Attempt to remove schemaLocation attribute:
            xml = Regex.Replace(xml,
                @"\s*([a-zA-z0-9:]*schemaLocation\s*=[""][^""]*[""])\s*", string.Empty,
                RegexOptions.IgnoreCase);

            return (!string.IsNullOrWhiteSpace(xml)) ? xml.Trim() : null;
        }
    }
}
