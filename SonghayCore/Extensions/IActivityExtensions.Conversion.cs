﻿using Songhay.Models;
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
        /// to <see cref="IActivityWithOutput{TInput,TOutput}" />.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="activity">The activity.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">The expected Activity name is not here.
        /// or</exception>
        public static IActivityWithOutput<TInput, TOutput> ToActivityWithOutput<TInput, TOutput>(this IActivity activity)
        {
            return activity.ToActivityWithOutput<TInput, TOutput>(throwException: true);
        }

        /// <summary>
        /// Converts the specified <see cref="IActivity" />
        /// to <see cref="IActivityWithOutput{TInput,TOutput}" />.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="activity">The activity.</param>
        /// <param name="throwException">if set to <c>true</c> throw exception.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">The expected IActivityOutput{TInput, TOutput} is not here.</exception>
        /// <exception cref="ArgumentNullException">The expected Activity name is not here.
        /// or</exception>
        public static IActivityWithOutput<TInput, TOutput> ToActivityWithOutput<TInput, TOutput>(this IActivity activity, bool throwException)
        {
            if (activity == null) return null;

            var output = activity as IActivityWithOutput<TInput, TOutput>;

            if (throwException && (output == null))
                throw new NullReferenceException($"The expected {nameof(IActivityWithOutput<TInput, TOutput>)} is not here.");

            return output;
        }

        /// <summary>
        /// Converts the specified <see cref="IActivity" />
        /// to <see cref="IActivityWithTask{TInput,TOutput}" />.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="activity">The activity.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">
        /// The expected IActivityWithTask{TInput, TOutput} is not here.
        /// </exception>
        public static IActivityWithTask<TInput, TOutput> ToActivityWithTask<TInput, TOutput>(this IActivity activity)
        {
            return activity.ToActivityWithTask<TInput, TOutput>(throwException: true);
        }

        /// <summary>
        /// Converts the specified <see cref="IActivity" />
        /// to <see cref="IActivityWithTask{TInput,TOutput}" />.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="activity">The activity.</param>
        /// <param name="throwException">if set to <c>true</c> throw exception.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">
        /// The expected IActivityWithTask{TInput, TOutput} is not here.
        /// </exception>
        public static IActivityWithTask<TInput, TOutput> ToActivityWithTask<TInput, TOutput>(this IActivity activity, bool throwException)
        {
            if (activity == null) return null;

            var output = activity as IActivityWithTask<TInput, TOutput>;

            if (throwException && (output == null))
                throw new NullReferenceException($"The expected {nameof(IActivityWithTask<TInput, TOutput>)} is not here.");

            return output;
        }

        /// <summary>
        /// Converts the specified <see cref="IActivity" />
        /// to <see cref="IActivityWithTask{TInput}" />.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <param name="activity">The activity.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">
        /// The expected IActivityWithTask{TInput, TOutput} is not here.
        /// </exception>
        public static IActivityWithTask<TInput> ToActivityWithTask<TInput>(this IActivity activity)
        {
            return activity.ToActivityWithTask<TInput>(throwException: true);
        }
        
        /// <summary>
        /// Converts the specified <see cref="IActivity" />
        /// to <see cref="IActivityWithTask{TInput}" />.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <param name="activity">The activity.</param>
        /// <param name="throwException">if set to <c>true</c> throw exception.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">
        /// The expected IActivityWithTask{TInput} is not here.
        /// </exception>
        public static IActivityWithTask<TInput> ToActivityWithTask<TInput>(this IActivity activity, bool throwException)
        {
            if (activity == null) return null;

            var output = activity as IActivityWithTask<TInput>;

            if (throwException && (output == null))
                throw new NullReferenceException($"The expected {nameof(IActivityWithTask<TInput>)} is not here.");

            return output;
        }

        /// <summary>
        /// Converts the specified <see cref="IActivity" />
        /// to <see cref="IActivityWithTask" />.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">
        /// The expected IActivityWithTask{TInput, TOutput} is not here.
        /// </exception>
        public static IActivityWithTask ToActivityWithTask(this IActivity activity)
        {
            return activity.ToActivityWithTask(throwException: true);
        }

        /// <summary>
        /// Converts the specified <see cref="IActivity" />
        /// to <see cref="IActivityWithTask" />.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <param name="throwException">if set to <c>true</c> throw exception.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">
        /// The expected IActivityWithTask is not here.
        /// </exception>
        public static IActivityWithTask ToActivityWithTask(this IActivity activity, bool throwException)
        {
            if (activity == null) return null;

            var output = activity as IActivityWithTask;

            if (throwException && (output == null))
                throw new NullReferenceException($"The expected {nameof(IActivityWithTask)} is not here.");

            return output;
        }

        /// <summary>
        /// Converts the specified <see cref="IActivity" />
        /// to <see cref="IActivityWithTaskOutput{TOutput}" />.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="activity">The activity.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">
        /// The expected IActivityWithTaskOutput{TOutput} is not here.
        /// </exception>
        public static IActivityWithTaskOutput<TOutput> ToActivityWithTaskOutput<TOutput>(this IActivity activity)
        {
            return activity.ToActivityWithTaskOutput<TOutput>(throwException: true);
        }
        
        /// <summary>
        /// Converts the specified <see cref="IActivity" />
        /// to <see cref="IActivityWithTaskOutput{TOutput}" />.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="activity">The activity.</param>
        /// <param name="throwException">if set to <c>true</c> throw exception.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">
        /// The expected IActivityWithTaskOutput{TOutput} is not here.
        /// </exception>
        public static IActivityWithTaskOutput<TOutput> ToActivityWithTaskOutput<TOutput>(this IActivity activity, bool throwException)
        {
            if (activity == null) return null;

            var output = activity as IActivityWithTaskOutput<TOutput>;

            if (throwException && (output == null))
                throw new NullReferenceException($"The expected {nameof(IActivityWithTaskOutput<TOutput>)} is not here.");

            return output;
        }
    }
}