using Songhay.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Songhay.Models
{
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
            this._defaultActivityName = ToActivityName(args);
            this.Args = new ProgramArgs(ToActivityArgs(args));
        }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        public ProgramArgs Args { get; private set; }

        /// <summary>
        /// Gets the <see cref="IActivity"/>.
        /// </summary>
        /// <returns></returns>
        public virtual IActivity GetActivity()
        {
            return this._activities.GetActivity(this._defaultActivityName);
        }

        /// <summary>
        /// Gets the <see cref="IActivity"/>.
        /// </summary>
        /// <param name="activityName">Name of the <see cref="IActivity"/>.</param>
        /// <returns></returns>
        public virtual IActivity GetActivity(string activityName)
        {
            return this._activities.GetActivity(activityName);
        }

        /// <summary>
        /// Loads the activities.
        /// </summary>
        /// <param name="activities">The activities.</param>
        public virtual void LoadActivities(Dictionary<string, Lazy<IActivity>> activities)
        {
            this._activities = activities;
        }

        static string[] ToActivityArgs(string[] args)
        {
            if (args == null) throw new ArgumentNullException("The expected Activity arguments are not here.");
            if (args.Count() < 2) return null;
            return args.Skip(1).ToArray();
        }

        static string ToActivityName(string[] args)
        {
            if (args == null) throw new ArgumentNullException("The expected Activity arguments are not here.");
            if (!args.Any()) throw new ArgumentException("The expected Activity name is not here.");
            return args.First();
        }

        Dictionary<string, Lazy<IActivity>> _activities;
        string _defaultActivityName;
    }
}
