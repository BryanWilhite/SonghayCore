using Songhay.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="IActivity"/>
    /// </summary>
    public static partial class IActivityExtensions
    {
        /// <summary>
        /// Gets the activity.
        /// </summary>
        /// <param name="activities">The activities.</param>
        /// <param name="activityName">Name of the activity.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// activityName
        /// </exception>
        public static IActivity GetActivity(this Dictionary<string, Lazy<IActivity>> activities, string activityName)
        {
            if (activities == null) return null;
            if (string.IsNullOrWhiteSpace(activityName)) throw new ArgumentNullException(nameof(activityName));
            if (!activities.Keys.Contains(activityName)) throw new FormatException($"The expected {nameof(IActivity)} name, {activityName}, is not here.");

            var activity = activities[activityName].Value;
            if (activity == null) throw new NullReferenceException("The expected Activity is not here.");

            return activity;
        }
    }
}