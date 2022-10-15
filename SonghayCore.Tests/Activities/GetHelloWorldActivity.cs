using Songhay.Diagnostics;
using Songhay.Models;
using Songhay.Abstractions;

namespace Songhay.Tests.Activities;

public class GetHelloWorldActivity : IActivity
{
    static GetHelloWorldActivity() => TraceSource = TraceSources.Instance.GetConfiguredTraceSource();
    static readonly TraceSource? TraceSource;

    public string DisplayHelp(ProgramArgs? args)
    {
        ArgumentNullException.ThrowIfNull(args);

        if(!args.HelpSet.Any())SetupHelp(args);

        return string.Concat(
            ProgramAssemblyUtility.GetAssemblyInfo(GetType().Assembly),
            args.ToHelpDisplayText()
        );
    }

    public void Start(ProgramArgs? args)
    {
        if (args.IsHelpRequest()) return;

        var worldName = args.GetArgValue(ArgWorldName);
        TraceSource.ToReferenceTypeValueOrThrow().WriteLine($"Hello from world {worldName}!");
    }

    void SetupHelp(ProgramArgs args)
    {
        var indentation = string.Join(string.Empty, Enumerable.Repeat(" ", 4).ToArray());
        args.HelpSet.Add(ArgWorldName, $"{ArgWorldName} <world name>{indentation}Returns a greeting for the specified world.");
        args.HelpSet.Add(ArgMoonList, $"{ArgMoonList} \"<list of moons>\"{indentation}A comma-separated list of moons.");
        args.HelpSet.Add(ArgMoonsRequired, $"{ArgMoonsRequired}{indentation}Indicates that moons are required for the world.");
    }

    const string ArgMoonList = "--moon-list";
    const string ArgMoonsRequired = "--moons-required";
    const string ArgWorldName = "--world-name";
}
