using Songhay.Models;
using System;
using System.IO;
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
        /// <param name="args">The <see cref="ProgramArgs"/>.</param>
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
        /// Gets <see cref="string"/> input
        /// from either <see cref="ProgramArgs.InputString"/>
        /// or <see cref="ProgramArgs.InputFile"/>.
        /// </summary>
        /// <param name="args">The <see cref="ProgramArgs"/>.</param>
        /// <returns></returns>
        public static string GetStringInput(this ProgramArgs args)
        {
            string input = null;

            if (args.HasArg(ProgramArgs.InputString, requiresValue: false))
            {
                input = args.GetArgValue(ProgramArgs.InputString);
            }
            else if (args.HasArg(ProgramArgs.InputFile, requiresValue: true))
            {
                var path = args.GetArgValue(ProgramArgs.InputFile);

                if (!File.Exists(path))
                {
                    throw new FileNotFoundException($"The expected input file, `{path ?? "[null]"}`, is not here.");
                }

                input = File.ReadAllText(path);
            }

            return input;
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
        /// <param name="args">The <see cref="ProgramArgs"/>.</param>
        public static bool IsHelpRequest(this ProgramArgs args)
        {
            return args.HasArg(ProgramArgs.Help, requiresValue: false);
        }

        /// <summary>
        /// Converts the <c>args</c> key to a conventional Configuration key.
        /// </summary>
        /// <param name="args">The <see cref="ProgramArgs"/>.</param>
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
        /// <param name="args">The <see cref="ProgramArgs"/>.</param>
        public static string ToHelpDisplayText(this ProgramArgs args)
        {
            if (args == null) return null;
            var builder = new StringBuilder();
            builder.AppendLine();
            args.HelpSet.ForEachInEnumerable(i => builder.AppendLine(i.Value));
            return builder.ToString();
        }

        /// <summary>
        /// Writes the <see cref="string"/> output
        /// to the file specified by <see cref="ProgramArgs.OutputFile"/>.
        /// </summary>
        /// <param name="args">The <see cref="ProgramArgs"/>.</param>
        /// <param name="output">the output to write</param>
        public static void WriteOutputToFile(this ProgramArgs args, string output)
        {
            if (args.HasArg(ProgramArgs.OutputFile, requiresValue: true))
            {
                var outputFile = args.GetArgValue(ProgramArgs.OutputFile);

                if (!File.Exists(outputFile)) throw new FileNotFoundException($"The expected file, `{outputFile ?? "[null]"}`, is not here.");

                File.WriteAllText(outputFile, output);
            }
        }

        /// <summary>
        /// Writes the <see cref="byte"/> array output
        /// to the file specified by <see cref="ProgramArgs.OutputFile"/>.
        /// </summary>
        /// <param name="args">The <see cref="ProgramArgs"/>.</param>
        /// <param name="output">the output to write</param>
        public static void WriteOutputToFile(this ProgramArgs args, byte[] output)
        {
            if (args.HasArg(ProgramArgs.OutputFile, requiresValue: true))
            {
                var outputFile = args.GetArgValue(ProgramArgs.OutputFile);

                if (!File.Exists(outputFile)) throw new FileNotFoundException($"The expected file, `{outputFile ?? "[null]"}`, is not here.");

                File.WriteAllBytes(outputFile, output);
            }
        }
    }
}
