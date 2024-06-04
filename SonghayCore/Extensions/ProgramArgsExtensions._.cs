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
    public static string? GetArgValue(this ProgramArgs? args, string? arg)
    {
        if (args?.Args == null) return null;

        int keyIndex = Array.IndexOf(args.Args.ToArray(), arg);

        string? value = args.Args.ElementAtOrDefault(keyIndex + 1);

        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException($"Argument {arg} is not here.");

        return value;
    }

    /// <summary>
    /// Determines whether the specified <see cref="ProgramArgs" /> has argument.
    /// </summary>
    /// <param name="args">The <see cref="ProgramArgs" />.</param>
    /// <param name="arg">The argument.</param>
    /// <param name="required">if set to <c>true</c> then the argument is required.</param>
    /// <returns>
    ///   <c>true</c> if the specified argument has argument; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="System.ArgumentException">Argument {arg} requires a value.</exception>
    public static bool HasArg(this ProgramArgs? args, string? arg, bool required)
    {
        if (args?.Args == null) return false;
        if(string.IsNullOrWhiteSpace(arg)) return false;

        bool hasArg = args.Args.Contains(arg);

        if (required && !hasArg) throw new ArgumentException($"Argument {arg} requires a value.");

        return hasArg;
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