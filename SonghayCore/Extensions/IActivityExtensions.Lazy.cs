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
        /// <exception cref="System.ArgumentNullException">
        /// The expected Activity name is not here.
        /// or
        /// </exception>
        public static IActivity GetActivity(this Dictionary<string, Lazy<IActivity>> activities, string activityName)
        {
            if (activities == null) return null;
            if (string.IsNullOrWhiteSpace(activityName)) throw new ArgumentNullException("The expected Activity name is not here.");
            if (!activities.Keys.Contains(activityName)) throw new ArgumentNullException($"The expected Activity name, {activityName}, is not here.");

            var activity = activities[activityName].Value;
            if (activity == null) throw new NullReferenceException("The expected Activity is not here.");

            return activity;
        }
    }
}