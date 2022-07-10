using System;
using System.Collections.Generic;
using Songhay.Abstractions;
using Songhay.Models;

namespace Songhay.Tests.Activities;

public sealed class MyActivitiesGetter : ActivitiesGetter
{
    public MyActivitiesGetter(string[] args) : base(args)
    {
        LoadActivities(new Dictionary<string, Lazy<IActivity?>>
        {
            {
                nameof(GetHelloWorldActivity),
                new Lazy<IActivity?>(() => new GetHelloWorldActivity())
            },
            {
                nameof(GetHelloWorldReportActivity),
                new Lazy<IActivity?>(() => new GetHelloWorldReportActivity())
            }
        });
    }
}
