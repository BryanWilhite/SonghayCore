using System;
using System.Threading.Tasks;

namespace Songhay.Models
{
    /// <summary>
    /// Extends <see cref="IActivity" /> with output support.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <seealso cref="IActivity" />
    [Obsolete("use `IActivityWithTask<TInput, TOutput>` instead", error: false)]
    public interface IActivityWithOutput<TInput, TOutput> : IActivity
    {
        /// <summary>
        /// Starts with the specified input
        /// and asynchronously returns <c>TResult</c>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        Task<TOutput> StartAsync(TInput input);
    }
}
