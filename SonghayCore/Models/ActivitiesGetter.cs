using Songhay.Extensions;
using Songhay.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SonghayCore.Models
{
    /// <summary>
    /// Defines the in-memory storage
    /// and getting of <see cref="IActivity"/> types.
    /// </summary>
    public abstract class ActivitiesGetter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivitiesGetter"/> class.
        /// </summary>
        public ActivitiesGetter()
        {
            this.SetupActivities();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivitiesGetter" /> class.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public ActivitiesGetter(string[] args)
        {
            this._defaultActivityName = ToActivityName(args);
            this.Args = new ProgramArgs(ToActivityArgs(args));
            this.SetupActivities();
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
        /// Called when the storage and lazy instantiation
        /// of <see cref="IActivity"/> types needs to be stated.
        /// </summary>
        /// <param name="activities">The activities.</param>
        protected virtual void OnSetupActivities(Dictionary<string, Lazy<IActivity>> activities)
        {
        }

        static string[] ToActivityArgs(string[] args)
        {
            if (args?.Count() < 2) return null;
            return args.Skip(1).ToArray();
        }

        static string ToActivityName(string[] args)
        {
            if (args?.Count() < 1) throw new ArgumentException("The expected Activity name is not here.");
            return args.First();
        }

        void SetupActivities()
        {
            this._activities = new Dictionary<string, Lazy<IActivity>>();
            this.OnSetupActivities(this._activities);
        }

        Dictionary<string, Lazy<IActivity>> _activities;
        string _defaultActivityName;
    }
}
