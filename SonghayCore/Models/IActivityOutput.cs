using System.Threading.Tasks;

namespace Songhay.Models
{
    /// <summary>
    /// Extends <see cref="IActivity"/> with output support.
    /// </summary>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <seealso cref="IActivity" />
    public interface IActivityOutput<TOutput> : IActivity
    {
        /// <summary>
        /// Starts with the specified arguments
        /// and asynchronously returns <c>TResult</c>.
        /// </summary>
        /// <param name="args">The arguments.</param>
        Task<TOutput> StartAsync(ProgramArgs args);
    }
}
