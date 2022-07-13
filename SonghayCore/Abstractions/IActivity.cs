namespace Songhay.Abstractions;

/// <summary>
/// Defines an Activity in a shell environment.
/// </summary>
/// <remarks>
/// For more detail, see “Songhay System Activities example”
/// [https://github.com/BryanWilhite/Songhay.HelloWorlds.Activities]
/// </remarks>
public interface IActivity
{
    /// <summary>
    /// Displays the conventional help text
    /// of the Activity.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns></returns>
    string DisplayHelp(ProgramArgs? args);

    /// <summary>
    /// Start the Activity with the specified arguments.
    /// </summary>
    /// <param name="args">The arguments.</param>
    void Start(ProgramArgs? args);
}
