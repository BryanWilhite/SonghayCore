using Songhay.Models;
using System;
using System.IO;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="ProgramArgs"/>
/// </summary>
public static partial class ProgramArgsExtensions
{
    /// <summary>
    /// Gets the conventional base-path value.
    /// </summary>
    /// <param name="args">The <see cref="ProgramArgs"/>.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string? GetBasePathValue(this ProgramArgs? args)
    {
        var isBasePathRequired = args.HasArg(ProgramArgs.BasePathRequired, requiresValue: false);
        if (!args.HasArg(ProgramArgs.BasePath, isBasePathRequired)) return null;

        var basePath = args.GetArgValue(ProgramArgs.BasePath);
        if (!Directory.Exists(basePath)) throw new ArgumentException($"{basePath} does not exist.");

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

        if (args.HasArg(ProgramArgs.OutputUnderBasePath, requiresValue: false))
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
    /// <returns></returns>
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
    /// <returns></returns>
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
    /// <returns></returns>
    public static string? GetStringInput(this ProgramArgs? args)
    {
        string? input = null;

        if (args.HasArg(ProgramArgs.InputString, requiresValue: false))
        {
            input = args.GetArgValue(ProgramArgs.InputString);
        }
        else if (args.HasArg(ProgramArgs.InputFile, requiresValue: true))
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

        return input;
    }

    /// <summary>
    /// Determines whether args contain the <see cref="ProgramArgs.Help"/> flag.
    /// </summary>
    /// <param name="args">The <see cref="ProgramArgs"/>.</param>
    public static bool IsHelpRequest(this ProgramArgs? args) => args.HasArg(ProgramArgs.Help, requiresValue: false);

    /// <summary>
    /// Converts the <c>args</c> key to a conventional Configuration key.
    /// </summary>
    /// <param name="args">The <see cref="ProgramArgs"/>.</param>
    /// <param name="argKey">The arguments key.</param>
    /// <returns></returns>
    /// <exception cref="System.NullReferenceException">The expected argument key is not here.</exception>
    public static string ToConfigurationKey(this ProgramArgs? args, string argKey)
    {
        if (string.IsNullOrWhiteSpace(argKey))
            throw new NullReferenceException("The expected argument key is not here.");

        return argKey
            .TrimStart('-')
            .Replace("/", string.Empty)
            .Replace("=", string.Empty);
    }

    /// <summary>
    /// Returns <see cref="ProgramArgs"/>
    /// </summary>
    /// <param name="args">The <see cref="ProgramArgs"/>.</param>
    /// <returns></returns>
    public static ProgramArgs? WithDefaultHelpText(this ProgramArgs? args)
    {
        if (args == null) return null;

        if (!args.HelpSet.ContainsKey(ProgramArgs.BasePath))
            args.HelpSet.Add(ProgramArgs.BasePath,
                "The path to the Directory where the Activity will set its context.");

        if (!args.HelpSet.ContainsKey(ProgramArgs.BasePathRequired))
            args.HelpSet.Add(ProgramArgs.BasePathRequired, $"Indicates that {ProgramArgs.BasePath} is required.");

        if (!args.HelpSet.ContainsKey(ProgramArgs.Help))
            args.HelpSet.Add(ProgramArgs.Help, "Displays this help text.");

        if (!args.HelpSet.ContainsKey(ProgramArgs.InputFile))
            args.HelpSet.Add(ProgramArgs.InputFile,
                $"The path to the file to load as Activity input. {ProgramArgs.InputString} can be used alternatively.");

        if (!args.HelpSet.ContainsKey(ProgramArgs.InputString))
            args.HelpSet.Add(ProgramArgs.InputString,
                $"The string literal used as Activity input. {ProgramArgs.InputFile} can be used alternatively.");

        if (!args.HelpSet.ContainsKey(ProgramArgs.OutputFile))
            args.HelpSet.Add(ProgramArgs.OutputFile,
                $"The path to the file to write as Activity output. This can be an absolute path or relative to {ProgramArgs.BasePath} when {ProgramArgs.OutputUnderBasePath} is used.");

        if (!args.HelpSet.ContainsKey(ProgramArgs.OutputUnderBasePath))
            args.HelpSet.Add(ProgramArgs.OutputUnderBasePath, $"See {ProgramArgs.OutputFile}.");

        if (!args.HelpSet.ContainsKey(ProgramArgs.SettingsFile))
            args.HelpSet.Add(ProgramArgs.SettingsFile,
                $"The path to the file to load as Activity Settings input. This can be an absolute path or relative to {ProgramArgs.BasePath}.");

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
        var outputFile = args.GetOutputPath();

        File.WriteAllBytes(outputFile!, output);
    }
}
