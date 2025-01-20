namespace Songhay.Models;

/// <summary>
/// Centralizes conventional console <c>args</c> passed to <see cref="IConfiguration"/>.
/// </summary>
public static class ConsoleArgsScalars
{
    /// <summary>
    /// The base-directory argument.
    /// </summary>
    public const string BaseDirectory = "--base-directory";

    /// <summary>
    /// Is <see cref="BaseDirectory"/> required flag.
    /// </summary>
    public const string BaseDirectoryRequired = "--base-directory-required";

    /// <summary>
    /// Dry-run flag.
    /// </summary>
    public const string DryRun = "--dry-run";

    /// <summary>
    /// Ensures the space after a flag is clearly seen.
    /// </summary>
    public const string FlagSpacer = " ";

    /// <summary>
    /// The help argument flag.
    /// </summary>
    public const string Help = "--help";

    /// <summary>
    /// The conventional <see cref="IConfiguration"/>-key help text suffix.
    /// </summary>
    public const string HelpTextSuffix = "-help";

    /// <summary>
    /// The input file argument.
    /// </summary>
    public const string InputFile = "--input-file";

    /// <summary>
    /// The input <see cref="string" /> argument.
    /// </summary>
    public const string InputString = "--input-string";

    /// <summary>
    /// The output file argument.
    /// </summary>
    public const string OutputFile = "--output-file";

    /// <summary>
    /// Flag: use the output file argument relative to <see cref="BaseDirectory"/>.
    /// </summary>
    public const string OutputUnderBasePath = "--output-under-base-path";

    /// <summary>
    /// The settings file argument.
    /// </summary>
    public const string SettingsFile = "--settings-file";
}
