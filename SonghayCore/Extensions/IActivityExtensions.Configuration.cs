using Microsoft.Extensions.Configuration;
using Songhay.Models;
using System;
using Songhay.Abstractions;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="IActivity"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static partial class IActivityExtensions
{
    /// <summary>
    /// Returns <see cref="IActivity"/> with <see cref="IConfigurationRoot"/> added when available.
    /// </summary>
    /// <param name="activity">The activity.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">activity</exception>
    public static IActivity WithConfiguration(this IActivity? activity, IConfigurationRoot? configuration)
    {
        if (activity == null) throw new ArgumentNullException(nameof(activity));
        if (configuration == null) return activity;

        // ReSharper disable once SuspiciousTypeConversion.Global
        if (activity is not IActivityConfigurationSupport activityWithConfiguration) return activity;

        activityWithConfiguration.AddConfiguration(configuration);

        return activity;
    }
}
