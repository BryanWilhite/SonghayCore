#if NET452 || NET462

using System.Security.Principal;

namespace Songhay.Security
{
    public static class SecurityPrincipalUtility
    {
        public static string WindowsIdentityName
        {
            get { return WindowsIdentity.GetCurrent().Name; }
        }
    }
}

#endif
