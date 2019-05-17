using System;

namespace Songhay.Models
{
    /// <summary>
    /// Adds temporal properties to an item
    /// </summary>
    public interface ITemporal
    {
        /// <summary>
        /// End/expiration <see cref="DateTime"/> of the item.
        /// </summary>
        DateTime EndDate { get; set; }

        /// <summary>
        /// Origin <see cref="DateTime"/> of the item.
        /// </summary>
        DateTime InceptDate { get; set; }

        /// <summary>
        /// Modification/editorial <see cref="DateTime"/> of the item.
        /// </summary>
        DateTime ModificationDate { get; set; }
    }
}
