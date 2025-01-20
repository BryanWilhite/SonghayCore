using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Songhay.Abstractions;

namespace Songhay.Tests.Activities;

public class GetHelloWorldActivity(IConfiguration configuration, ILogger<GetHelloWorldActivity> logger) : IActivity, IActivityHelpDisplay
{
    public string? CachedHelpText { get; set; }

    public string DisplayHelp()
    {
        SetupHelp();

        return string.Concat(
            ProgramAssemblyUtility.GetAssemblyInfo(GetType().Assembly),
            CachedHelpText
        );
    }

    public void Start()
    {
        if (configuration.IsHelpRequest())
        {
            DisplayHelp();

            return;
        }

        string? worldName = configuration.GetCommandLineArgValue(ArgWorldName);
        logger.LogInformation("Hello from world {Name}!", worldName);
    }

    private void SetupHelp()
    {
        if(!string.IsNullOrWhiteSpace(this.CachedHelpText)) return;

        configuration.AddHelpDisplayText(ArgWorldName, "Returns a greeting for the specified world.");
        configuration.AddHelpDisplayText(ArgMoonList, "A comma-separated list of moons.");
        configuration.AddHelpDisplayText(ArgMoonsRequired, "Indicates that moons are required for the world.");

        CachedHelpText = configuration.ToHelpDisplayText();
    }

    const string ArgMoonList = "--moon-list";
    const string ArgMoonsRequired = "--moons-required";
    const string ArgWorldName = "--world-name";
}
