#if NET452 || NET462

using System;
using System.ComponentModel;

namespace Songhay.Models
{
    /// <summary>
    /// Defines the meta-data
    /// for <see cref="Songhay.ComponentModel.BackgroundWorkerUtility"/>.
    /// </summary>
    [Obsolete("Since .NET 4, use System.Threading.Tasks (TPL)")]
    public class BackgroundWorkerUtilityData
    {
        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        /// <value>The input.</value>
        public Action<object, DoWorkEventArgs> Input { get; set; }

        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        /// <value>The output.</value>
        public Action<object, RunWorkerCompletedEventArgs> Output { get; set; }

        /// <summary>
        /// Gets or sets the progress.
        /// </summary>
        /// <value>The progress.</value>
        public Action<object, ProgressChangedEventArgs> Progress { get; set; }
    }
}

#endif
