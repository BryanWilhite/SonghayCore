using System;
using System.Collections.Generic;
using System.Linq;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="Songhay.Models.IActivity"/>
    /// </summary>
    public static partial class IActivityExtensions
    {
        /// <summary>
        /// Converts <c>args</c> to the name of the Activity.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static string[] ToActivityArgs(this string[] args)
        {
            if (args?.Count() < 2) return args;

            return args.Skip(1).ToArray();
        }

        /// <summary>
        /// Converts <c>args</c> to the name of the Activity.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The expected Activity name is not here.</exception>
        public static string ToActivityName(this IEnumerable<string> args)
        {
            if (args?.Count() < 1) throw new ArgumentException("The expected Activity name is not here.");
            var activityName = args.ElementAt(0);
            return activityName;
        }
    }
}