using Songhay.Models;
using System;
using System.Linq;
using System.Text;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="ProgramArgs"/>
    /// </summary>
    public static partial class ProgramArgsExtensions
    {
        /// <summary>
        /// Gets the argument value.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="arg">The argument.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        public static string GetArgValue(this ProgramArgs args, string arg)
        {
            if (args == null) return null;
            if (args.Args == null) return null;

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
        public static bool HasArg(this ProgramArgs args, string arg, bool requiresValue)
        {
            if (args == null) return false;
            if (args.Args == null) return false;

            var index = Array.IndexOf(args.Args, arg);
            if (index == -1) return false;

            var lastIndex = Array.LastIndexOf(args.Args, arg);
            if (index != lastIndex) throw new ArgumentException($"Argument {arg} used more than once.");

            if (!requiresValue) return true;

            if (args.Args.Length < (index + 1)) throw new ArgumentException($"Argument {arg} requires a value.");

            return true;
        }

        /// <summary>
        /// Determines whether args contain the <see cref="ProgramArgs.Help"/> flag.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static bool IsHelpRequest(this ProgramArgs args)
        {
            return args.HasArg(ProgramArgs.Help, requiresValue: false);
        }

        /// <summary>
        /// Converts the <c>args</c> key to a conventional Configuration key.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="argKey">The arguments key.</param>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">The expected argument key is not here.</exception>
        public static string ToConfigurationKey(this ProgramArgs args, string argKey)
        {
            if (string.IsNullOrWhiteSpace(argKey)) throw new NullReferenceException("The expected argument key is not here.");
            return argKey
                    .TrimStart('-')
                    .Replace("/", string.Empty)
                    .Replace("=", string.Empty);
        }

        /// <summary>
        /// Converts the <c>args</c> key any help text.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static string ToHelpDisplayText(this ProgramArgs args)
        {
            if (args == null) return null;
            var builder = new StringBuilder();
            builder.AppendLine();
            args.HelpSet.ForEachInEnumerable(i => builder.AppendLine(i.Value));
            return builder.ToString();
        }
    }
}
