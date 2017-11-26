using System.Collections.Generic;
using System.Linq;

namespace Songhay.Models
{
    /// <summary>
    /// Defines conventional command-line arguments.
    /// </summary>
    public class ProgramArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramArgs"/> class.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public ProgramArgs(string[] args)
        {
            this.Args = args;
            if ((args != null) && args.Any()) this.HelpSet = new Dictionary<string, string>(capacity: args.Length);
        }

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
        /// The settings file argument.
        /// </summary>
        public const string SettingsFile = "--settings-file";

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        public string[] Args { get; private set; }

        /// <summary>
        /// Gets the help set.
        /// </summary>
        /// <value>
        /// The help set.
        /// </value>
        public Dictionary<string, string> HelpSet { get; private set; }
    }
}
