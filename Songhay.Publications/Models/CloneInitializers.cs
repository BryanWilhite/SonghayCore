using System;
using System.Collections.Generic;

namespace Songhay.Publications.Models
{
    /// <summary>
    /// Centralizes initializers for <see cref="CloneExtensions"/>
    /// </summary>
    public static class CloneInitializers
    {
        static CloneInitializers()
        {
            GenericWeb = new Dictionary<Type, Func<object, object>>
            {
                { typeof(ISegment), o => new Segment() },
                { typeof(IDocument), o => new Document() },
                { typeof(IFragment), o => new Fragment() },
                { typeof(IWebKeyword), o => new WebKeyword() },
            };
        }

        /// <summary>
        /// Gets initializers for GenericWeb initializers
        /// </summary>
        public static Dictionary<Type, Func<object, object>> GenericWeb { get; }
    }
}
