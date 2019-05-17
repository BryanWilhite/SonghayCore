namespace Songhay.Models
{
    /// <summary>
    /// Defines a group-able visual
    /// </summary>
    public interface IGroupable
    {
        /// <summary>
        /// Display text of the Group
        /// </summary>
        string GroupDisplayText { get; set; }

        /// <summary>
        /// Identifier of the Group
        /// </summary>
        string GroupId { get; set; }

        /// <summary>
        /// Returns `true` when group is visually collapsed
        /// </summary>
        bool IsCollapsed { get; set; }
    }
}
