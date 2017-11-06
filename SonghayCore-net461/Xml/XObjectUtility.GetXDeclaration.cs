using Songhay.Models;
using System.Xml.Linq;

namespace Songhay.Xml
{
    /// <summary>
    /// Static helper members for XML-related routines.
    /// </summary>
    public static partial class XObjectUtility
    {
        /// <summary>
        /// Gets the <see cref="System.Xml.Linq.XDeclaration"/>.
        /// </summary>
        public static XDeclaration GetXDeclaration()
        {
            return XObjectUtility.GetXDeclaration(XEncoding.Utf08, true);
        }

        /// <summary>
        /// Gets the <see cref="System.Xml.Linq.XDeclaration"/>.
        /// </summary>
        /// <param name="encoding">The encoding (<see cref="XEncoding.Utf08"/> by default).</param>
        /// <param name="isStandAlone">When <c>true</c> document is stand-alone (<c>true</c> by default).</param>
        public static XDeclaration GetXDeclaration(string encoding, bool isStandAlone)
        {
            var declaration = new XDeclaration("1.0",
                encoding, (isStandAlone ? "yes" : "no"));
            return declaration;
        }
    }
}
