using System;
using System.Linq;
using System.Security.Principal;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="IIdentity"/>.
    /// </summary>
    public static class IIdentityExtensions
    {
        /// <summary>
        /// Gets the name of the windows user.
        /// </summary>
        public static string GetWindowsUserName(this IIdentity id)
        {
            if (id == null) return null;

            var ntlmUserName = id.Name;
            if (string.IsNullOrWhiteSpace(ntlmUserName)) throw new ArgumentNullException("The expected user name is not here.");
            return ntlmUserName.Split(new char[] { '\\' }).Last();
        }
    }
}
