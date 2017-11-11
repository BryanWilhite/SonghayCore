using Songhay.Models;
using System;
using System.Collections.Generic;

namespace Songhay.Tests.Activities
{
    public class MyActivitiesGetter : ActivitiesGetter
    {
        public MyActivitiesGetter(string[] args) : base(args) { }

        protected override void OnSetupActivities(Dictionary<string, Lazy<IActivity>> activities)
        {
            activities.Add(
                nameof(Activities.GetHelloWorldActivity),
                new Lazy<IActivity>(() => new Activities.GetHelloWorldActivity())
                );
            activities.Add(
                nameof(Activities.GetHelloWorldReportActivity),
                new Lazy<IActivity>(() => new Activities.GetHelloWorldReportActivity())
                );
        }
    }
}
