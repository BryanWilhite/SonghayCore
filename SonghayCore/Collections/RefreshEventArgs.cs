#if NET452 || NET462

using System;
using System.ComponentModel;

namespace Songhay.Collections
{
    /// <summary>
    /// Refresh Event Arguments, provides indication of need for data refresh
    /// </summary>
    public class RefreshEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the sort descriptions.
        /// </summary>
        /// <value>The sort descriptions.</value>
        public SortDescriptionCollection SortDescriptions { get; set; }
    }
}

#endif
