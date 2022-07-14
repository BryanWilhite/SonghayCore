namespace Songhay.Extensions;

// ReSharper disable once InconsistentNaming
public static partial class IActivityExtensions
{
    /// <summary>
    /// Returns <see cref="IActivity"/> with <see cref="IConfigurationRoot"/> added when available.
    /// </summary>
    /// <param name="activity">The activity.</param>
    /// <param name="configuration">The configuration.</param>
    public static IActivity WithConfiguration(this IActivity? activity, IConfigurationRoot? configuration)
    {
        ArgumentNullException.ThrowIfNull(activity);
        if (configuration == null) return activity;

        // ReSharper disable once SuspiciousTypeConversion.Global
        if (activity is not IActivityConfigurationSupport activityWithConfiguration) return activity;

        activityWithConfiguration.AddConfiguration(configuration);

        return activity;
    }
}
