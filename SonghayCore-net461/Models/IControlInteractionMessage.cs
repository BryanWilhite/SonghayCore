
namespace Songhay.Models
{
    /// <summary>
    /// Defines a Composition Message
    /// for a Control interaction event.
    /// </summary>
    public interface IControlInteractionMessage
    {
        /// <summary>
        /// Gets or sets the control value.
        /// </summary>
        /// <value>The control value.</value>
        object ControlValue { get; set; }

        /// <summary>
        /// Gets or sets the control id.
        /// </summary>
        /// <value>The control id.</value>
        string ControlId { get; set; }

        /// <summary>
        /// Gets or sets the control tag.
        /// </summary>
        /// <value>The control tag.</value>
        string ControlTag { get; set; }
    }
}
