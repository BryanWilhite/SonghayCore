#if NET5_0_WINDOWS

using System.Security.Principal;

namespace Songhay.Security
{
    /// <summary>
    /// Shared routines for Windows security
    /// </summary>
    public static class SecurityPrincipalUtility
    {
        /// <summary>
        /// Gets the name of the current <see cref="WindowsIdentity" />.
        /// </summary>
        /// <value></value>
        public static string WindowsIdentityName
        {
            get { return WindowsIdentity?.GetCurrent()?.Name; }
        }
    }
}

#endif
