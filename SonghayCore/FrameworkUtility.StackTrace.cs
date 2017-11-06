using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Songhay
{
    /// <summary>
    /// Static members for framework-level procedures.
    /// </summary>
    public static partial class FrameworkUtility
    {
        /// <summary>
        /// Gets the name of the current method.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethodName()
        {
            return GetMethodName(2);
        }

        /// <summary>
        /// Gets the name of the current method.
        /// </summary>
        /// <param name="stackFrameIndex">Index of the stack frame.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetMethodName(int stackFrameIndex)
        {
            var trace = new StackTrace();
            var frame = trace.GetFrame(stackFrameIndex);

            return frame.GetMethod().Name;
        }
    }
}
