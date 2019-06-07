using System.Collections.Generic;
using System.Linq;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extension of <see cref="char"/>.
    /// </summary>
    public static class CharExtensions
    {
        /// <summary>
        /// Converts an enumeration of <see cref="char"/>
        /// to <see cref="string"/>
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static string FromCharsToString(this IEnumerable<char> chars)
        {
            if (chars == null) return null;
            if (!chars.Any()) return string.Empty;
            return new string(chars.ToArray());
        }
    }
}
