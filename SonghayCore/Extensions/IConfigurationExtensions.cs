namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="IConfiguration"/>.
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IConfigurationExtensions
{
    /// <summary>
    /// Returns <c>true</c> when the specified <see cref="IConfiguration"/>
    /// contains the specified key.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    /// <param name="key">the <see cref="IConfiguration"/> key</param>
    /// <remarks>
    /// This member will call <see cref="StringExtensions.ToConfigurationKey"/>
    /// to convert a console argument to <see cref="IConfiguration"/>-key format.
    /// </remarks>
    public static bool HasArg(this IConfiguration? configuration, string? key) =>
        configuration.ToKeys().Contains(key.ToConfigurationKey());

    /// <summary>
    /// Converts the specified <see cref="IConfiguration"/>
    /// to a collection of its underlying keys.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    public static IReadOnlyCollection<string> ToKeys(this IConfiguration? configuration) =>
        configuration == null ?
            Array.Empty<string>()
            :
            configuration.AsEnumerable().Select(kv => kv.Key).ToArray();

    /// <summary>
    /// Returns <see cref="IConfiguration"/> with the conventional, console, <c>args</c> help text
    /// for <see cref="ConsoleArgsScalars"/>.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    public static IConfiguration? WithDefaultHelpText(this IConfiguration? configuration)
    {
        if (configuration == null) return default;

        configuration[ConsoleArgsScalars.BasePath.WithConfigurationHelpTextSuffix()] =
            "The path to the Directory where the Activity will set its context.";

        configuration[ConsoleArgsScalars.BasePathRequired.WithConfigurationHelpTextSuffix()] =
            $"Indicates that {ConsoleArgsScalars.BasePath} is required.";

        configuration[ConsoleArgsScalars.Help.WithConfigurationHelpTextSuffix()] =
            "Displays this help text.";

        configuration[ConsoleArgsScalars.InputFile.WithConfigurationHelpTextSuffix()] =
            $"The path to the file to load as Activity input. {ConsoleArgsScalars.InputString} can be used alternatively.";

        configuration[ConsoleArgsScalars.InputString.WithConfigurationHelpTextSuffix()] =
            $"The string literal used as Activity input. {ConsoleArgsScalars.InputFile} can be used alternatively.";

        configuration[ConsoleArgsScalars.OutputFile.WithConfigurationHelpTextSuffix()] =
            $"The path to the file to write as Activity output. This can be an absolute path or relative to {ConsoleArgsScalars.BasePath} when {ConsoleArgsScalars.OutputUnderBasePath} is used.";

        configuration[ConsoleArgsScalars.OutputUnderBasePath.WithConfigurationHelpTextSuffix()] =
            $"See {ConsoleArgsScalars.OutputFile}.";

        configuration[ConsoleArgsScalars.SettingsFile.WithConfigurationHelpTextSuffix()] =
            $"The path to the file to load as Activity Settings input. This can be an absolute path or relative to {ConsoleArgsScalars.BasePath}.";

        return configuration;
    }
}
