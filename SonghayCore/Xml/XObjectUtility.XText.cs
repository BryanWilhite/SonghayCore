using System;
using System.Collections;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

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
    }
}
