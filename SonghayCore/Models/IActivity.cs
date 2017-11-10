using System.Collections.Generic;

namespace Songhay.Models
{
    /// <summary>
    /// Defines an Activity in a shell environment.
    /// </summary>
    /// <remarks>
    /// For more detail, see “Songhay Shell Activities”
    /// [https://github.com/BryanWilhite/Songhay.HelloWorlds.Activities]
    /// </remarks>
    public interface IActivity
    {
        /// <summary>
        /// Starts with the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        void Start(ProgramArgs args);
    }
}