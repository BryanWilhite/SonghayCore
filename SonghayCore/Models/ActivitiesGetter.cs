using Songhay.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Songhay.Abstractions;

namespace Songhay.Models;

/// <summary>
/// Defines the in-memory storage
/// and getting of <see cref="IActivity"/> types.
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
    /// Gets the arguments.
    /// </summary>
    /// <value>
    /// The arguments.
    /// </value>
    public ProgramArgs Args { get; }

    /// <summary>
    /// Gets the <see cref="IActivity"/>.
    /// </summary>
    /// <returns></returns>
    public virtual IActivity? GetActivity() => _activities.GetActivity(_defaultActivityName);

    /// <summary>
    /// Gets the <see cref="IActivity"/>.
    /// </summary>
    /// <param name="activityName">Name of the <see cref="IActivity"/>.</param>
    /// <returns></returns>
    public virtual IActivity? GetActivity(string activityName) => _activities.GetActivity(activityName);

    /// <summary>
    /// Loads the activities.
    /// </summary>
    /// <param name="activities">The activities.</param>
    public virtual void LoadActivities(Dictionary<string, Lazy<IActivity?>> activities) => _activities = activities;

    static string[] ToActivityArgs(string[] args)
    {
        if (args == null) throw new ArgumentNullException(nameof(args));

        return args.Length < 2 ? Enumerable.Empty<string>().ToArray() : args.Skip(1).ToArray();
    }

    static string ToActivityName(string[] args)
    {
        if (args == null) throw new ArgumentNullException(nameof(args));
        if (!args.Any()) throw new ArgumentException("The expected Activity name is not here.");

        return args.First();
    }

    Dictionary<string, Lazy<IActivity?>> _activities;
    readonly string _defaultActivityName;
}