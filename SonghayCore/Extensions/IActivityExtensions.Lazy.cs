namespace Songhay.Extensions;

// ReSharper disable once InconsistentNaming
public static partial class IActivityExtensions
{
    /// <summary>
    /// Gets the <see cref="IActivity"/>.
    /// </summary>
    /// <param name="activities">The activities.</param>
    /// <param name="activityName">Name of the activity.</param>
    public static IActivity? GetActivity(this Dictionary<string, Lazy<IActivity?>>? activities, string? activityName)
    {
        if (activities == null) return null;

        activityName.ThrowWhenNullOrWhiteSpace();

        if (!activities.ContainsKey(activityName))
            throw new FormatException($"The expected {nameof(IActivity)} name, {activityName}, is not here.");

        var activity = activities[activityName].Value;
        if (activity == null) throw new NullReferenceException("The expected Activity is not here.");

        return activity;
    }
}
