namespace Songhay.Abstractions;

/// <summary>
/// Adds optional help-display text support to <c>IActivity*</c> types.
/// </summary>
public interface IActivityHelpDisplay
{
    /// <summary>
    /// Display the help text.
    /// </summary>
    public string DisplayHelp();

    /// <summary>
    /// The cached help text.
    /// </summary>
    /// <remarks>
    /// Instead of calling <see cref="IConfigurationExtensions.ToHelpDisplayText(IConfiguration?, int)"/> more than once, return this member.
    /// </remarks>
    public string? CachedHelpText { get; set; }
}
