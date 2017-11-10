using Songhay.Extensions;
using Songhay.Models;
using System;
using System.Collections.Generic;

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
        /// Gets the <see cref="IActivity"/>.
        /// </summary>
        /// <param name="activityName">Name of the <see cref="IActivity"/>.</param>
        /// <returns></returns>
        protected virtual IActivity GetActivity(string activityName)
        {
            return this._activities.GetActivity(activityName);
        }

        /// <summary>
        /// Gets the activity help.
        /// </summary>
        /// <returns></returns>
        protected virtual Dictionary<string, string> GetActivityHelp()
        {
            return this._activitiesHelp;
        }

        /// <summary>
        /// Called when the storage and lazy instantiation
        /// of <see cref="IActivity"/> types needs to be stated.
        /// </summary>
        /// <param name="activities">The activities.</param>
        /// <param name="activitiesHelp">The activities help.</param>
        protected virtual void OnSetupActivities(Dictionary<string, Lazy<IActivity>> activities, Dictionary<string, string> activitiesHelp)
        {
        }

        void SetupActivities()
        {
            this._activities = new Dictionary<string, Lazy<IActivity>>();
            this._activitiesHelp = new Dictionary<string, string>();
            this.OnSetupActivities(this._activities, this._activitiesHelp);
        }

        Dictionary<string, Lazy<IActivity>> _activities;
        Dictionary<string, string> _activitiesHelp;
    }
}
