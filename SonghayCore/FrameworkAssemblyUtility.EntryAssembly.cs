#if NET452 || NET462

using System;
using System.Reflection;

namespace Songhay
{
    /// <summary>
    /// Static members related to <see cref="System.Reflection"/>.
    /// </summary>
    public static partial class FrameworkAssemblyUtility
    {

        /// <summary>
        /// Sets the entry assembly.
        /// </summary>
        public static void SetEntryAssembly()
        {
            SetEntryAssembly(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Sets the entry assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <remarks>
        /// This member addresses the null entry assembly problem in VSTEST/MSTEST scenarios.
        /// For detail, see http://stackoverflow.com/a/21888521/22944
        /// </remarks>
        public static void SetEntryAssembly(Assembly assembly)
        {
            AppDomainManager manager = new AppDomainManager();
            FieldInfo entryAssemblyfield = manager.GetType().GetField("m_entryAssembly", BindingFlags.Instance | BindingFlags.NonPublic);
            entryAssemblyfield.SetValue(manager, assembly);

            AppDomain domain = AppDomain.CurrentDomain;
            FieldInfo domainManagerField = domain.GetType().GetField("_domainManager", BindingFlags.Instance | BindingFlags.NonPublic);
            domainManagerField.SetValue(domain, manager);
        }
    }
}

#endif
