namespace Songhay.Models
{
    /// <summary>
    /// Defines a colorable visual.
    /// </summary>
    public interface IColorable
    {
        /// <summary>
        /// Gets or sets the background hexadecimal value.
        /// </summary>
        /// <value>The background hexadecimal value.</value>
        string BackgroundHex { get; set; }

        /// <summary>
        /// Gets or sets the foreground hexadecimal value.
        /// </summary>
        /// <value>The foreground hexadecimal value.</value>
        string ForegroundHex { get; set; }
    }
}
