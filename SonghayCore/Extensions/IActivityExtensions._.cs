using Songhay.Models;
using System;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="IActivity"/>
    /// </summary>
    public static partial class IActivityExtensions
    {
        /// <summary>
        /// Converts the specified <see cref="IActivity" />
        /// to <see cref="IActivityOutput{TOutput}" />.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="activity">The activity.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">The expected Activity name is not here.
        /// or</exception>
        public static IActivityOutput<TOutput> ToIActivityOutput<TOutput>(this IActivity activity)
        {
            return activity.ToIActivityOutput<TOutput>(throwException: true);
        }

        /// <summary>
        /// Converts the specified <see cref="IActivity" />
        /// to <see cref="IActivityOutput{TOutput}" />.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="activity">The activity.</param>
        /// <param name="throwException">if set to <c>true</c> throw exception.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">The expected Activity name is not here.
        /// or</exception>
        public static IActivityOutput<TOutput> ToIActivityOutput<TOutput>(this IActivity activity, bool throwException)
        {
            if (activity == null) return null;

            var output = activity as IActivityOutput<TOutput>;

            if (throwException && (output == null))
                throw new NullReferenceException($"The expected {nameof(IActivityOutput<TOutput>)} is not here.");

            return output;
        }
    }
}