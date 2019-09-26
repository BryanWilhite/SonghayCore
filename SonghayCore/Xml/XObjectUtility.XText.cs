using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Songhay.Xml
{
    /// <summary>
    /// Static helper members for XML-related routines.
    /// </summary>
    public static partial class XObjectUtility
    {
        /// <summary>
        /// Glyph: Non-Breaking Space
        /// </summary>
        public static readonly string GlyphNonBreakingSpace = " ";

        /// <summary>
        /// <see cref="System.Xml.Linq.XText"/>: Non-Breaking Space
        /// </summary>
        public static XText XTextNonBreakingSpace { get { return new XText(GlyphNonBreakingSpace); } }

        /// <summary>
        /// Joins the flattened <see cref="XText"/> nodes.
        /// </summary>
        /// <param name="rootElement">The root element.</param>
        public static string JoinFlattenedXTextNodes(XElement rootElement)
        {
            return JoinFlattenedXTextNodes(rootElement, includeRootElement: false, joinDelimiter: string.Empty);
        }

        /// <summary>
        /// Joins the flattened <see cref="XText"/> nodes.
        /// </summary>
        /// <param name="rootElement">The root element.</param>
        /// <param name="includeRootElement">if set to <c>true</c> [include root element].</param>
        public static string JoinFlattenedXTextNodes(XElement rootElement, bool includeRootElement)
        {
            return JoinFlattenedXTextNodes(rootElement, includeRootElement, joinDelimiter: string.Empty);
        }

        /// <summary>
        /// Joins the flattened <see cref="XText"/> nodes.
        /// </summary>
        /// <param name="rootElement">The root element.</param>
        /// <param name="includeRootElement">if set to <c>true</c> [include root element].</param>
        /// <param name="joinDelimiter">The join delimiter.</param>
        /// <returns></returns>
        public static string JoinFlattenedXTextNodes(XElement rootElement, bool includeRootElement, string joinDelimiter)
        {
            if (rootElement == null) return null;
            if (string.IsNullOrWhiteSpace(joinDelimiter)) joinDelimiter = string.Empty;

            var nodes = includeRootElement ?
                rootElement.DescendantNodesAndSelf().Where(i => i.NodeType == XmlNodeType.Text)
                :
                rootElement.DescendantNodes().Where(i => i.NodeType == XmlNodeType.Text);

            var displayText = string.Join(joinDelimiter, nodes.Select(i => i.ToString()).ToArray());
            return displayText;
        }
    }
}
