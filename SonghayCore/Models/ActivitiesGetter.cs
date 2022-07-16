namespace Songhay.Models;

/// <summary>
/// Defines the in-memory storage
/// and Program access to <see cref="IActivity"/> types.
/// </summary>
public abstract class ActivitiesGetter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActivitiesGetter" /> class.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public ActivitiesGetter(string[] args)
    {
        _defaultActivityName = ToActivityName(args);
        Args = new ProgramArgs(ToActivityArgs(args));
    }

    /// <summary>
    /// Gets the <see cref="ProgramArgs"/>.
    /// </summary>
    public ProgramArgs Args { get; }

    /// <summary>
    /// Gets the <see cref="IActivity"/>.
    /// </summary>
    public virtual IActivity? GetActivity() => _activities.GetActivity(_defaultActivityName);

    /// <summary>
    /// Gets the <see cref="IActivity"/>.
    /// </summary>
    /// <param name="activityName">Name of the <see cref="IActivity"/>.</param>
    public virtual IActivity? GetActivity(string activityName) => _activities.GetActivity(activityName);

    /// <summary>
    /// Loads the activities into memory, lazily.
    /// </summary>
    /// <param name="activities">The activities.</param>
    public virtual void LoadActivities(Dictionary<string, Lazy<IActivity?>>? activities) => _activities = activities;

    static string[] ToActivityArgs(string[] args)
    {
        return args.Length < 2 ? Enumerable.Empty<string>().ToArray() : args.Skip(1).ToArray();
    }

    static string ToActivityName(string[] args)
    {
        args.ThrowWhenNullOrEmpty();

        return args.First();
    }

    Dictionary<string, Lazy<IActivity?>>? _activities;
    readonly string _defaultActivityName;
}
