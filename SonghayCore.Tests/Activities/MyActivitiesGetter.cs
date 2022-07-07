using Songhay.Models;
using System;
using System.Collections.Generic;

namespace Songhay.Tests.Activities;

public class MyActivitiesGetter : ActivitiesGetter
{
    public MyActivitiesGetter(string[] args) : base(args)
    {
        this.LoadActivities(new Dictionary<string, Lazy<IActivity>>
        {
            {
                nameof(Activities.GetHelloWorldActivity),
                new Lazy<IActivity>(() => new Activities.GetHelloWorldActivity())
            },
            {
                nameof(Activities.GetHelloWorldReportActivity),
                new Lazy<IActivity>(() => new Activities.GetHelloWorldReportActivity())
            }
        });
    }
}