namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="IConfiguration"/>.
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IConfigurationExtensions
{
    /// <summary>
    /// Returns the value of the conventional <see cref="ConsoleArgsScalars.BasePath"/> <c>arg</c>
    /// or throws <see cref="DirectoryNotFoundException"/>
    /// when the conventional <see cref="ConsoleArgsScalars.BasePathRequired"/> <c>arg</c> is present.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    public static string? GetBasePathValue(this IConfiguration? configuration)
    {
        if (configuration == null) return null;

        bool isBasePathRequired = configuration.HasArg(ConsoleArgsScalars.BasePathRequired);
        string? basePath = configuration[ConsoleArgsScalars.BasePath.ToConfigurationKey()];

        if(string.IsNullOrWhiteSpace(basePath) && isBasePathRequired)
            throw new DirectoryNotFoundException($"The expected `{ConsoleArgsScalars.BasePath}` is not here.");
        if(!Directory.Exists(basePath) && isBasePathRequired)
            throw new DirectoryNotFoundException($"`{ConsoleArgsScalars.BasePath} = {basePath}` does not exist.");

        return basePath;
    }

    /// <summary>
    /// Returns the value of the conventional <see cref="ConsoleArgsScalars.OutputFile"/> <c>arg</c>
    /// and will prefix it with the value of the <see cref="ConsoleArgsScalars.BasePath"/> <c>arg</c>
    /// when the conventional <see cref="ConsoleArgsScalars.OutputUnderBasePath"/> <c>arg</c> is present.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    public static string GetOutputPath(this IConfiguration? configuration)
    {
        if (configuration == null) return string.Empty;

        string outputFile = configuration[ConsoleArgsScalars.OutputFile.ToConfigurationKey()];

        bool outputUnderBasePath = configuration.HasArg(ConsoleArgsScalars.OutputUnderBasePath);

        if (outputUnderBasePath)
        {
            string? basePath = configuration.GetBasePathValue();

            outputFile = ProgramFileUtility.GetCombinedPath(basePath, outputFile);
        }

        if (!File.Exists(outputFile)) File.WriteAllText(outputFile, string.Empty);

        return outputFile;
    }

    /// <summary>
    /// Returns the value of the conventional <see cref="ConsoleArgsScalars.SettingsFile"/> <c>arg</c>
    /// which should be a path to a text file.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    public static string? GetSettingsFilePath(this IConfiguration? configuration) =>
        configuration?[ConsoleArgsScalars.SettingsFile.WithConfigurationHelpTextSuffix()];

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
    /// Determines whether <c>args</c> contain the convnetional <see cref="ProgramArgs.Help"/> flag.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    public static bool IsHelpRequest(this IConfiguration? configuration) =>
        configuration.HasArg(ConsoleArgsScalars.Help.ToConfigurationKey());

    /// <summary>
    /// Reads the settings data in the file specified by <see cref="ConsoleArgsScalars.SettingsFile"/>
    /// which could be a relative or absolute path.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    public static string ReadSettings(this IConfiguration? configuration)
    {
        string? path = configuration.GetSettingsFilePath();

        if (File.Exists(path)) return File.ReadAllText(path);

        string? basePath = configuration.GetBasePathValue();

        path = ProgramFileUtility.GetCombinedPath(basePath, path, fileIsExpected: true);

        return File.ReadAllText(path);
    }

    /// <summary>
    /// Reads the input data in the inline <see cref="string"/>
    /// specified by <see cref="ConsoleArgsScalars.InputString"/>
    /// or the file specified by <see cref="ConsoleArgsScalars.InputFile"/>
    /// which could be a relative or absolute path.
    /// </summary>
    /// <param name="configuration"></param>
    /// <exception cref="FileNotFoundException"></exception>
    public static string? ReadStringInput(this IConfiguration? configuration)
    {
        string? input = null;

        if (configuration.HasArg(ConsoleArgsScalars.InputString))
        {
            input = configuration?[ConsoleArgsScalars.InputString.ToConfigurationKey()];
        }
        else if (configuration.HasArg(ConsoleArgsScalars.InputFile))
        {
            var path = configuration?[ConsoleArgsScalars.InputFile.ToConfigurationKey()];

            if (!File.Exists(path))
            {
                var basePath = configuration.GetBasePathValue();

                path = ProgramFileUtility
                    .GetCombinedPath(basePath, path, fileIsExpected: true);

                if (!File.Exists(path))
                    throw new FileNotFoundException($"The expected input file, `{path}`, is not here.");
            }

            input = File.ReadAllText(path);
        }

        return input;
    }

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

    /// <summary>
    /// Writes the <see cref="string"/> output
    /// to the file specified by <see cref="ConsoleArgsScalars.OutputFile"/>.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    /// <param name="output">the output to write</param>
    public static void WriteOutputToFile(this IConfiguration? configuration, string output)
    {
        var outputFile = configuration.GetOutputPath();

        File.WriteAllText(outputFile, output);
    }

    /// <summary>
    /// Writes the <see cref="string"/> output
    /// to the file specified by <see cref="ConsoleArgsScalars.OutputFile"/>.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    /// <param name="output">the output to write</param>
    public static async Task WriteOutputToFileAsync(this IConfiguration? configuration, string output)
    {
        var outputFile = configuration.GetOutputPath();

        await File.WriteAllTextAsync(outputFile, output);
    }

    /// <summary>
    /// Writes the <see cref="byte"/> array output
    /// to the file specified by <see cref="ConsoleArgsScalars.OutputFile"/>.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    /// <param name="output">the output to write</param>
    public static void WriteOutputToFile(this IConfiguration? configuration, byte[] output)
    {
        var outputFile = configuration.GetOutputPath();

        File.WriteAllBytes(outputFile, output);
    }

    /// <summary>
    /// Writes the <see cref="byte"/> array output
    /// to the file specified by <see cref="ConsoleArgsScalars.OutputFile"/>.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    /// <param name="output">the output to write</param>
    public static async Task WriteOutputToFileAsync(this IConfiguration? configuration, byte[] output)
    {
        var outputFile = configuration.GetOutputPath();

        await File.WriteAllBytesAsync(outputFile, output);
    }

    /// <summary>
    /// Writes the <see cref="Stream"/> output
    /// to the file specified by <see cref="ConsoleArgsScalars.OutputFile"/>.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    /// <param name="output">the output to write</param>
    /// <remarks>
    /// [ see https://stackoverflow.com/a/5515894/22944 ]
    /// </remarks>
    public static void WriteOutputToFile(this IConfiguration? configuration, Stream output)
    {
        var outputFile = configuration.GetOutputPath();

        using var fileStream = File.Create(outputFile);

        output.Seek(0, SeekOrigin.Begin);
        output.CopyTo(fileStream);
    }

    /// <summary>
    /// Writes the <see cref="Stream"/> output
    /// to the file specified by <see cref="ConsoleArgsScalars.OutputFile"/>.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    /// <param name="output">the output to write</param>
    /// <remarks>
    /// [ see https://stackoverflow.com/a/5515894/22944 ]
    /// </remarks>
    public static async Task WriteOutputToFileAsync(this IConfiguration? configuration, Stream output)
    {
        var outputFile = configuration.GetOutputPath();

        using var fileStream = File.Create(outputFile);

        output.Seek(0, SeekOrigin.Begin);
        await output.CopyToAsync(fileStream);
    }
}
