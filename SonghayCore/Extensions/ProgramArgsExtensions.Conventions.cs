namespace Songhay.Extensions;

public static partial class ProgramArgsExtensions
{
    /// <summary>
    /// Gets the conventional base-path value.
    /// </summary>
    /// <param name="args">The <see cref="ProgramArgs"/>.</param>
    public static string? GetBasePathValue(this ProgramArgs? args)
    {
        var isBasePathRequired = args.HasArg(ProgramArgs.BasePathRequired, required: false);
        if (!args.HasArg(ProgramArgs.BasePath, isBasePathRequired)) return null;

        var basePath = args.GetArgValue(ProgramArgs.BasePath);
        if (!Directory.Exists(basePath)) throw new DirectoryNotFoundException($"`{basePath}` does not exist.");

        return basePath;
    }

    /// <summary>
    /// Gets the path to the output file specified by conventional arguments.
    /// </summary>
    /// <param name="args">The <see cref="ProgramArgs"/>.</param>
    /// <remarks>
    /// This member will generate a file when it does not exist.
    /// </remarks>
    public static string? GetOutputPath(this ProgramArgs? args)
    {
        var outputFile = args.GetArgValue(ProgramArgs.OutputFile);

        if (args.HasArg(ProgramArgs.OutputUnderBasePath, required: false))
        {
            var basePath = args.GetBasePathValue();

            outputFile = ProgramFileUtility.GetCombinedPath(basePath, outputFile);
        }

        if (!File.Exists(outputFile)) File.WriteAllText(outputFile!, string.Empty);

        return outputFile;
    }

    /// <summary>
    /// Gets the conventional settings file <see cref="string"/> content.
    /// </summary>
    /// <param name="args">The <see cref="ProgramArgs"/>.</param>
    public static string GetSettings(this ProgramArgs? args)
    {
        var path = args.GetSettingsFilePath();

        if (File.Exists(path)) return File.ReadAllText(path);

        var basePath = args.GetBasePathValue();

        path = ProgramFileUtility.GetCombinedPath(basePath, path, fileIsExpected: true);

        return File.ReadAllText(path);
    }

    /// <summary>
    /// Gets the conventional settings file path.
    /// </summary>
    /// <param name="args">The <see cref="ProgramArgs"/>.</param>
    public static string? GetSettingsFilePath(this ProgramArgs? args)
    {
        var settingsFileName = args.GetArgValue(ProgramArgs.SettingsFile);

        return settingsFileName;
    }

    /// <summary>
    /// Gets <see cref="string"/> input
    /// from either <see cref="ProgramArgs.InputString"/>
    /// or <see cref="ProgramArgs.InputFile"/>.
    /// </summary>
    /// <param name="args">The <see cref="ProgramArgs"/>.</param>
    public static string GetStringInput(this ProgramArgs? args)
    {
        string? input = null;

        if (args.HasArg(ProgramArgs.InputString, required: false))
        {
            input = args.GetArgValue(ProgramArgs.InputString);
        }
        else if (args.HasArg(ProgramArgs.InputFile, required: true))
        {
            var path = args.GetArgValue(ProgramArgs.InputFile);

            if (!File.Exists(path))
            {
                var basePath = args.GetBasePathValue();

                path = ProgramFileUtility
                    .GetCombinedPath(basePath, path, fileIsExpected: true);

                if (!File.Exists(path))
                    throw new FileNotFoundException($"The expected input file, `{path}`, is not here.");
            }

            input = File.ReadAllText(path);
        }

        return input.ToReferenceTypeValueOrThrow();
    }

    /// <summary>
    /// Determines whether args contain the <see cref="ProgramArgs.Help"/> flag.
    /// </summary>
    /// <param name="args">The <see cref="ProgramArgs"/>.</param>
    public static bool IsHelpRequest(this ProgramArgs? args) => args.HasArg(ProgramArgs.Help, required: false);

    /// <summary>
    /// Converts the <c>args</c> key to a conventional Configuration key.
    /// </summary>
    /// <param name="args">The <see cref="ProgramArgs"/>.</param>
    /// <param name="argKey">The arguments key.</param>
    public static string ToConfigurationKey(this ProgramArgs? args, string argKey)
    {
        argKey.ThrowWhenNullOrWhiteSpace();

        return argKey
            .TrimStart('-')
            .Replace("/", string.Empty)
            .Replace("=", string.Empty);
    }

    /// <summary>
    /// Returns <see cref="ProgramArgs"/>
    /// </summary>
    /// <param name="args">The <see cref="ProgramArgs"/>.</param>
    public static ProgramArgs? WithDefaultHelpText(this ProgramArgs? args)
    {
        if (args == null) return null;

        args.HelpSet.TryAdd(ProgramArgs.BasePath, "The path to the Directory where the Activity will set its context.");

        args.HelpSet.TryAdd(ProgramArgs.BasePathRequired, $"Indicates that {ProgramArgs.BasePath} is required.");

        args.HelpSet.TryAdd(ProgramArgs.Help, "Displays this help text.");

        args.HelpSet.TryAdd(ProgramArgs.InputFile, $"The path to the file to load as Activity input. {ProgramArgs.InputString} can be used alternatively.");

        args.HelpSet.TryAdd(ProgramArgs.InputString, $"The string literal used as Activity input. {ProgramArgs.InputFile} can be used alternatively.");

        args.HelpSet.TryAdd(ProgramArgs.OutputFile, $"The path to the file to write as Activity output. This can be an absolute path or relative to {ProgramArgs.BasePath} when {ProgramArgs.OutputUnderBasePath} is used.");

        args.HelpSet.TryAdd(ProgramArgs.OutputUnderBasePath, $"See {ProgramArgs.OutputFile}.");

        args.HelpSet.TryAdd(ProgramArgs.SettingsFile, $"The path to the file to load as Activity Settings input. This can be an absolute path or relative to {ProgramArgs.BasePath}.");

        return args;
    }

    /// <summary>
    /// Writes the <see cref="string"/> output
    /// to the file specified by <see cref="ProgramArgs.OutputFile"/>.
    /// </summary>
    /// <param name="args">The <see cref="ProgramArgs"/>.</param>
    /// <param name="output">the output to write</param>
    public static void WriteOutputToFile(this ProgramArgs? args, string output)
    {
        var outputFile = args.GetOutputPath();

        File.WriteAllText(outputFile!, output);
    }

    /// <summary>
    /// Writes the <see cref="byte"/> array output
    /// to the file specified by <see cref="ProgramArgs.OutputFile"/>.
    /// </summary>
    /// <param name="args">The <see cref="ProgramArgs"/>.</param>
    /// <param name="output">the output to write</param>
    public static void WriteOutputToFile(this ProgramArgs? args, byte[] output)
    {
        var outputFile = args.GetOutputPath().ToReferenceTypeValueOrThrow();

        File.WriteAllBytes(outputFile, output);
    }

    /// <summary>
    /// Writes the <see cref="Stream"/> output
    /// to the file specified by <see cref="ProgramArgs.OutputFile"/>.
    /// </summary>
    /// <param name="args">The <see cref="ProgramArgs"/>.</param>
    /// <param name="stream">the <see cref="Stream"/> to write</param>
    /// <remarks>
    /// [ see https://stackoverflow.com/a/5515894/22944 ]
    /// </remarks>
    public static void WriteOutputToFile(this ProgramArgs? args, Stream stream)
    {
        var outputFile = args.GetOutputPath().ToReferenceTypeValueOrThrow();

        using var fileStream = File.Create(outputFile);

        stream.Seek(0, SeekOrigin.Begin);
        stream.CopyTo(fileStream);
    }
}
