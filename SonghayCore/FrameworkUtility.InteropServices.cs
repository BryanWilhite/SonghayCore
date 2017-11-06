using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Songhay
{
    /// <summary>
    /// Static members for framework-level procedures.
    /// </summary>
    public static partial class FrameworkUtility
    {
        /// <summary>
        /// Splits the arguments.
        /// </summary>
        /// <param name="rawArguments">The raw/unsplit argument line.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Unable to split argument.</exception>
        /// <exception cref="System.ComponentModel.Win32Exception"></exception>
        /// <remarks>
        /// From “RegEx function to parse a command line without using a library”
        /// [http://stackoverflow.com/a/17052018/22944]
        /// </remarks>
        public static string[] SplitArgs(string rawArguments)
        {
            int numberOfArgs;
            string[] splitArgs;

            IntPtr ptrToSplitArgs = CommandLineToArgvW(rawArguments, out numberOfArgs);
            if (ptrToSplitArgs == IntPtr.Zero) throw new ArgumentException("Unable to split argument.", new Win32Exception());

            try
            {
                splitArgs = new string[numberOfArgs];
                for (int i = 0; i < numberOfArgs; i++) splitArgs[i] = Marshal.PtrToStringUni(Marshal.ReadIntPtr(ptrToSplitArgs, i * IntPtr.Size));
                return splitArgs;
            }
            finally
            {
                LocalFree(ptrToSplitArgs);
            }
        }

        [DllImport("shell32.dll", SetLastError = true)]
        static extern IntPtr CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);

        [DllImport("kernel32.dll")]
        static extern IntPtr LocalFree(IntPtr hMem);
    }
}
