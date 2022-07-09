using Songhay.Models;
using System;
using System.Linq;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="ProgramArgs"/>
/// </summary>
public static partial class ProgramArgsExtensions
{
    /// <summary>
    /// Gets the argument value.
    /// </summary>
    /// <param name="args">The <see cref="ProgramArgs"/>.</param>
    /// <param name="arg">The argument.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException"></exception>
    public static string? GetArgValue(this ProgramArgs? args, string? arg)
    {
        if (args?.Args == null) return null;

        var index = Array.IndexOf(args.Args, arg);
        var @value = args.Args.ElementAtOrDefault(index + 1);
        if (string.IsNullOrWhiteSpace(@value)) throw new ArgumentException($"Argument {arg} is not here.");

        return value;
    }

    /// <summary>
    /// Determines whether the specified <see cref="ProgramArgs"/> has argument.
    /// </summary>
    /// <param name="args">The <see cref="ProgramArgs"/>.</param>
    /// <param name="arg">The argument.</param>
    /// <param name="requiresValue">if set to <c>true</c> [requires value].</param>
    /// <returns>
    ///   <c>true</c> if the specified argument has argument; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="System.ArgumentException">
    /// </exception>
    public static bool HasArg(this ProgramArgs? args, string? arg, bool requiresValue)
    {
        if (args?.Args == null) return false;

        var index = Array.IndexOf(args.Args, arg);
        if (index == -1) return false;

        var lastIndex = Array.LastIndexOf(args.Args, arg);
        if (index != lastIndex) throw new ArgumentException($"Argument {arg} used more than once.");

        if (!requiresValue) return true;

        if (args.Args.Length < (index + 1)) throw new ArgumentException($"Argument {arg} requires a value.");

        return true;
    }

    /// <summary>
    /// Converts the <c>args</c> key any help text.
    /// </summary>
    /// <param name="args">The <see cref="ProgramArgs"/>.</param>
    public static string? ToHelpDisplayText(this ProgramArgs? args) => args.ToHelpDisplayText(padding: 4);

    /// <summary>
    /// Converts the <c>args</c> key any help text.
    /// </summary>
    /// <param name="args">The <see cref="ProgramArgs"/>.</param>
    /// <param name="padding">the padding between <see cref="ProgramArgs.HelpSet"/> keys and values</param>
    public static string? ToHelpDisplayText(this ProgramArgs? args, int padding)
    {
        if (args == null) return null;

        var maxLength = args.HelpSet.Select(pair => pair.Key.Length).Max();

        return args.HelpSet
            .Where(pair =>
                !string.IsNullOrWhiteSpace(pair.Key) &&
                !string.IsNullOrWhiteSpace(pair.Value))
            .Select(pair =>
            {
                var count = (maxLength - pair.Key.Length) + padding;
                var spaces = Enumerable.Repeat(" ", count).ToArray();
                return
                    $"{pair.Key}{string.Join(string.Empty, spaces)}{pair.Value}{Environment.NewLine}{Environment.NewLine}";
            })
            .Aggregate((a, i) => $"{a}{i}");
    }
}
