namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="IConfiguration"/>.
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IConfigurationExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    /// <param name="key">the key to add</param>
    /// <param name="helpText">the help text to add</param>
    public static void AddHelpDisplayText(this IConfiguration? configuration, string key, string helpText)
    {
        if (configuration == null) return;

        configuration[key.ToConfigurationKey().WithConfigurationHelpTextSuffix()] = helpText;
    }

    /// <summary>
    /// Returns the value of the conventional <see cref="ConsoleArgsScalars.BaseDirectory"/> <c>arg</c>
    /// or throws <see cref="DirectoryNotFoundException"/>
    /// when the conventional <see cref="ConsoleArgsScalars.BaseDirectoryRequired"/> <c>arg</c> is present.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    public static string? GetBasePathValue(this IConfiguration? configuration)
    {
        if (configuration == null) return null;

        bool isBasePathRequired = configuration.HasArg(ConsoleArgsScalars.BaseDirectoryRequired);
        string? basePath = configuration.GetCommandLineArgValue(ConsoleArgsScalars.BaseDirectory);

        if(string.IsNullOrWhiteSpace(basePath) && isBasePathRequired)
            throw new DirectoryNotFoundException($"The expected `{ConsoleArgsScalars.BaseDirectory}` is not here.");
        if(!Directory.Exists(basePath) && isBasePathRequired)
            throw new DirectoryNotFoundException($"`{ConsoleArgsScalars.BaseDirectory} = {basePath}` does not exist.");

        return basePath;
    }

    /// <summary>
    /// Returns a value from the specified <see cref="IConfiguration"/>
    /// by calling <see cref="StringExtensions.ToConfigurationKey"/>
    /// on the specified <c>arg</c>.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    /// <param name="arg">the command-line argument to be converted into a <see cref="IConfiguration"/> key</param>
    public static string? GetCommandLineArgValue(this IConfiguration? configuration, string arg) => configuration?[arg.ToConfigurationKey()];

    /// <summary>
    /// Returns the value of the conventional <see cref="ConsoleArgsScalars.OutputFile"/> <c>arg</c>
    /// and will prefix it with the value of the <see cref="ConsoleArgsScalars.BaseDirectory"/> <c>arg</c>
    /// when the conventional <see cref="ConsoleArgsScalars.OutputUnderBasePath"/> <c>arg</c> is present.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    public static string? GetOutputPath(this IConfiguration? configuration)
    {
        if (configuration == null) return string.Empty;

        string? outputFile = configuration.GetCommandLineArgValue(ConsoleArgsScalars.OutputFile);

        bool outputUnderBasePath = configuration.HasArg(ConsoleArgsScalars.OutputUnderBasePath);

        if (outputUnderBasePath)
        {
            string? basePath = configuration.GetBasePathValue();

            outputFile = ProgramFileUtility.GetCombinedPath(basePath, outputFile);
        }

        if (!string.IsNullOrWhiteSpace(outputFile) &&
            !File.Exists(outputFile) &&
            Directory.Exists(Path.GetPathRoot(outputFile)))
            File.WriteAllText(outputFile, string.Empty);

        return outputFile;
    }

    /// <summary>
    /// Returns the value of the conventional <see cref="ConsoleArgsScalars.SettingsFile"/> <c>arg</c>
    /// which should be a path to a text file.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    public static string GetSettingsFilePath(this IConfiguration? configuration)
    {
        string? path =  configuration.GetCommandLineArgValue(ConsoleArgsScalars.SettingsFile);

        if (File.Exists(path)) return path;

        string? basePath = configuration.GetBasePathValue();

        path = ProgramFileUtility.GetCombinedPath(basePath, path, fileIsExpected: true);

        return path;
    }

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
    public static bool HasArg(this IConfiguration? configuration, string? key) => configuration.ToKeys().Contains(key.ToConfigurationKey());

    /// <summary>
    /// Determines whether <c>args</c> contain the conventional <see cref="ConsoleArgsScalars.DryRun"/> flag.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    public static bool IsDryRun(this IConfiguration? configuration) => configuration.HasArg(ConsoleArgsScalars.DryRun);

    /// <summary>
    /// Determines whether <c>args</c> contain the conventional <see cref="ConsoleArgsScalars.Help"/> flag.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    public static bool IsHelpRequest(this IConfiguration? configuration) => configuration.HasArg(ConsoleArgsScalars.Help);

    /// <summary>
    /// Reads the settings data in the file specified by <see cref="ConsoleArgsScalars.SettingsFile"/>
    /// which could be a relative or absolute path.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    public static string ReadSettings(this IConfiguration? configuration)
    {
        string path = configuration.GetSettingsFilePath();

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
            input = configuration.GetCommandLineArgValue(ConsoleArgsScalars.InputString);
        }
        else if (configuration.HasArg(ConsoleArgsScalars.InputFile))
        {
            string? path = configuration.GetCommandLineArgValue(ConsoleArgsScalars.InputFile);

            if (!File.Exists(path))
            {
                var basePath = configuration.GetBasePathValue();

                path = ProgramFileUtility.GetCombinedPath(basePath, path, fileIsExpected: true);

                if (!File.Exists(path))
                    throw new FileNotFoundException($"The expected input file, `{path}`, is not here.");
            }

            input = File.ReadAllText(path);
        }

        return input;
    }

    /// <summary>
    /// Converts the specified <see cref="IConfiguration"/>
    /// to any keys ending with <see cref="ConsoleArgsScalars.HelpTextSuffix"/>.
    /// </summary>
    /// <param name="configuration"></param>
    public static string? ToHelpDisplayText(this IConfiguration? configuration) => configuration.ToHelpDisplayText(4);

    /// <summary>
    /// Converts the specified <see cref="IConfiguration"/>
    /// to any keys ending with <see cref="ConsoleArgsScalars.HelpTextSuffix"/>.
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="padding">the padding between keys and values</param>
    public static string? ToHelpDisplayText(this IConfiguration? configuration, int padding)
    {
        if (configuration == null) return null;

        string[] helpKeys =
            configuration.ToKeys()
                .Where(k =>
                    !k.EqualsInvariant(ConsoleArgsScalars.Help.ToConfigurationKey()) &&
                    k.EndsWith(ConsoleArgsScalars.HelpTextSuffix))
                .ToArray();

        if (!helpKeys.Any()) return null;

        var pairs = helpKeys
            .Select(k => new KeyValuePair<string, string?>(k, configuration.GetValue<string>(k)))
            .ToArray();

        int maxLength = helpKeys.Select(k => k.Length).Max();

        return pairs
            .Where(pair =>
                !string.IsNullOrWhiteSpace(pair.Key) &&
                !string.IsNullOrWhiteSpace(pair.Value))
            .Select(pair =>
            {
                int count = maxLength - pair.Key.Length + padding;
                string[] spaces = Enumerable.Repeat(" ", count).ToArray();
                string key = pair.Key.Replace(ConsoleArgsScalars.HelpTextSuffix, string.Empty);
                if (key.EqualsInvariant("-")) key = ConsoleArgsScalars.Help;
                string newLine = $"{Environment.NewLine}{Environment.NewLine}";

                return $"{key}{string.Join(string.Empty, spaces)}{pair.Value}{newLine}";
            })
            .Aggregate((a, i) => $"{a}{i}");
    }

    /// <summary>
    /// Converts the specified <see cref="IConfiguration"/>
    /// to a collection of its underlying keys.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    public static IReadOnlyCollection<string> ToKeys(this IConfiguration? configuration) =>
        configuration == null ?
            []
            :
            configuration.AsEnumerable().Select(kv => kv.Key).ToArray();

    /// <summary>
    /// Returns <see cref="IConfiguration"/> with the conventional, console, <c>args</c> help text
    /// for <see cref="ConsoleArgsScalars"/>.
    /// </summary>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    public static IConfiguration? WithDefaultHelpText(this IConfiguration? configuration)
    {
        if (configuration == null) return null;

        configuration.AddHelpDisplayText(ConsoleArgsScalars.BaseDirectory,
            "The path to the Directory where the Activity will set its context.");

        configuration.AddHelpDisplayText(ConsoleArgsScalars.BaseDirectoryRequired,
            $"Indicates that {ConsoleArgsScalars.BaseDirectory} is required.");

        configuration.AddHelpDisplayText(ConsoleArgsScalars.Help,
            "Displays this help text.");

        configuration.AddHelpDisplayText(ConsoleArgsScalars.InputFile,
            $"The path to the file to load as Activity input. {ConsoleArgsScalars.InputString} can be used alternatively.");

        configuration.AddHelpDisplayText(ConsoleArgsScalars.InputString,
            $"The string literal used as Activity input. {ConsoleArgsScalars.InputFile} can be used alternatively.");

        configuration.AddHelpDisplayText(ConsoleArgsScalars.OutputFile,
            $"The path to the file to write as Activity output. This can be an absolute path or relative to {ConsoleArgsScalars.BaseDirectory} when {ConsoleArgsScalars.OutputUnderBasePath} is used.");

        configuration.AddHelpDisplayText(ConsoleArgsScalars.OutputUnderBasePath,
            $"See {ConsoleArgsScalars.OutputFile}.");

        configuration.AddHelpDisplayText(ConsoleArgsScalars.SettingsFile,
            $"The path to the file to load as Activity Settings input. This can be an absolute path or relative to {ConsoleArgsScalars.BaseDirectory}.");

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
        string? outputFile = configuration.GetOutputPath();

        if (string.IsNullOrWhiteSpace(outputFile)) return;

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
        string? outputFile = configuration.GetOutputPath();

        if (string.IsNullOrWhiteSpace(outputFile)) return;

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
        string? outputFile = configuration.GetOutputPath();

        if (string.IsNullOrWhiteSpace(outputFile)) return;

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
        string? outputFile = configuration.GetOutputPath();

        if (string.IsNullOrWhiteSpace(outputFile)) return;

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
        string? outputFile = configuration.GetOutputPath();

        if (string.IsNullOrWhiteSpace(outputFile)) return;

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
        string? outputFile = configuration.GetOutputPath();

        if (string.IsNullOrWhiteSpace(outputFile)) return;

        await using var fileStream = File.Create(outputFile);

        output.Seek(0, SeekOrigin.Begin);
        await output.CopyToAsync(fileStream);
    }
}
