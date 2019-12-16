namespace Songhay.Models
{
    /// <summary>
    /// Defines the conventional output
    /// of <see cref="IActivityWithTask{TInput, TOutput}"/>
    /// </summary>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    public class ActivityOutput<TOutput>
    {
        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        /// <value>
        /// The output.
        /// </value>
        public TOutput Output { get; set; }

        /// <summary>
        /// Gets or sets the log.
        /// </summary>
        /// <value>
        /// The log.
        /// </value>
        public string Log { get; set; }
    }
}
