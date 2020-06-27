using System.Threading.Tasks;

namespace Songhay.Models
{
    /// <summary>
    /// Extends <see cref="IActivity" /> with <see cref="Task"/> support.
    /// </summary>
    /// <seealso cref="IActivity" />
    /// <remarks>
    /// For detail aound why this definition exists,
    /// see https://github.com/BryanWilhite/SonghayCore/issues/83
    /// </remarks>
    public interface IActivityWithTask : IActivity
    {
        /// <summary>
        /// Starts the <see cref="IActivity"/> asynchronously.
        /// </summary>
        /// <returns></returns>
        Task StartAsync();
    }

    /// <summary>
    /// Extends <see cref="IActivity" /> with <see cref="Task"/> support.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <seealso cref="IActivity" />
    /// <remarks>
    /// For detail aound why this definition exists,
    /// see https://github.com/BryanWilhite/SonghayCore/issues/83
    /// </remarks>
    public interface IActivityWithTask<TInput> : IActivity
    {
        /// <summary>
        /// Starts the <see cref="IActivity" /> asynchronously.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        Task StartAsync(TInput input);
    }

    /// <summary>
    /// Extends <see cref="IActivity" /> with <see cref="Task"/> output support.
    /// </summary>
    public interface IActivityWithTaskOutput<TOutput> : IActivity
    {
        /// <summary>
        /// Starts the <see cref="IActivity" /> asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<TOutput> StartAsync();
    }

    /// <summary>
    /// Extends <see cref="IActivity" /> with <see cref="Task"/> support.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <seealso cref="IActivity" />
    /// <remarks>
    /// For detail aound why this definition exists,
    /// see https://github.com/BryanWilhite/SonghayCore/issues/83
    /// </remarks>
    public interface IActivityWithTask<TInput, TOutput> : IActivity
    {
        /// <summary>
        /// Starts the <see cref="IActivity"/> asynchronously.
        /// </summary>
        Task<TOutput> StartAsync(TInput input);
    }
}
