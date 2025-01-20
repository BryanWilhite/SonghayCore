using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Songhay.Abstractions;

namespace Songhay.Tests.Activities;

public class GetHelloWorldReportActivity(IConfiguration configuration, ILogger<GetHelloWorldReportActivity> logger) : IActivity, IActivityHelpDisplay
{
    public string DisplayHelp()
    {
        throw new NotImplementedException();
    }

    public string? CachedHelpText { get; set; }

    public void Start()
    {
        logger.LogInformation("Sorry, but the Hello Worlds reports are not yet available :(");
    }
}
