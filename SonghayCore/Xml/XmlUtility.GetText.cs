using System.IO;
using System.Text;

namespace Songhay.Xml
{
    public static partial class XmlUtility
    {
        /// <summary>
        /// “Cleans” XML data returning
        /// in a <see cref="System.IO.MemoryStream"/>.
        /// </summary>
        /// <param name="ramStream"><see cref="System.IO.MemoryStream"/></param>
        public static string GetText(MemoryStream ramStream)
        {
            string s = null;

            if(ramStream == null) return s;
            s = Encoding.UTF8.GetString(ramStream.ToArray());

            if(!string.IsNullOrWhiteSpace(s))
                s = s.Trim().Replace("\0", string.Empty);

            return s;
        }
    }
}
