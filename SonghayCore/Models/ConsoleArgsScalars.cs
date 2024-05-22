namespace Songhay.Models;

/// <summary>
/// Centralizes conventional console <c>args</c> passed to <see cref="IConfiguration"/>.
/// </summary>
public static class ConsoleArgsScalars
{
    /// <summary>
    /// The base-path argument.
    /// </summary>
    public const string BasePath = "--base-path";

    /// <summary>
    /// The base path required argument.
    /// </summary>
    public const string BasePathRequired = "--base-path-required";

    /// <summary>
    /// The help argument.
    /// </summary>
    public const string Help = "--help";

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
    /// Use the output file argument relative to <see cref="ProgramArgs.BasePath"/>.
    /// </summary>
    public const string OutputUnderBasePath = "--output-under-base-path";

    /// <summary>
    /// The settings file argument.
    /// </summary>
    public const string SettingsFile = "--settings-file";
}
