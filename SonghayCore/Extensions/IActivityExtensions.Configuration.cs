#if NETSTANDARD

using Microsoft.Extensions.Configuration;
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
        /// Returns <see cref="IActivity"/> with <see cref="IConfigurationRoot"/> added when available.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">activity - The expected activity is not here.</exception>
        public static IActivity WithConfiguration(this IActivity activity, IConfigurationRoot configuration)
        {
            if (activity == null) throw new ArgumentNullException(nameof(activity), "The expected activity is not here.");
            if (configuration == null) return activity;

            var activityWithConfiguration = activity as IActivityConfigurationSupport;
            if (activityWithConfiguration == null) return activity;

            activityWithConfiguration.AddConfiguration(configuration);

            return activity;
        }
    }
}

#endif
