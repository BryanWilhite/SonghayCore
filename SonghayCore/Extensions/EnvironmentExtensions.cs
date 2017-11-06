using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;

namespace Songhay.Extensions
{
    using Models;

    /// <summary>
    /// Extensions of <see cref="System.Environment"/>.
    /// </summary>
    public static partial class EnvironmentExtensions
    {
        /// <summary>
        /// Gets the conventional value.
        /// </summary>
        /// <param name="conventionalName">Name of the conventional.</param>
        /// <returns>Returns the value of named data.</returns>
        public static string GetConventionalValue(string conventionalName)
        {
            return (from e in _environmentVariables
                    where e.VariableName == conventionalName
                    select e.VariableValue ?? string.Empty).First<string>();
        }

        private static ICollection<SystemVariable> ListEnvironmentVariables()
        {
                var list = new List<SystemVariable>();

                var variableName = "";
                var variableDescription = "";
                var variableValue = "";

                #region Insert data into list:

                variableName = "MachineName";
                variableDescription = "Network Identification";
                variableValue = Environment.MachineName;
                list.Add(new SystemVariable { VariableName = variableName, VariableDescription = variableDescription, VariableValue = variableValue });

                variableName = "OSVersion.Platform";
                variableDescription = "Operating System Platform";
                variableValue = Environment.OSVersion.Platform.ToString();
                list.Add(new SystemVariable { VariableName = variableName, VariableDescription = variableDescription, VariableValue = variableValue });

                variableName = "OSVersion.ServicePack";
                variableDescription = "Operating System Service Pack";
                variableValue = Environment.OSVersion.ServicePack;
                list.Add(new SystemVariable { VariableName = variableName, VariableDescription = variableDescription, VariableValue = variableValue });

                variableName = "OSVersion.VersionString";
                variableDescription = "Operating System Version Summary";
                variableValue = Environment.OSVersion.VersionString;
                list.Add(new SystemVariable { VariableName = variableName, VariableDescription = variableDescription, VariableValue = variableValue });

                variableName = "UserDomainName";
                variableDescription = "User Domain Name";
                variableValue = Environment.UserDomainName;
                list.Add(new SystemVariable { VariableName = variableName, VariableDescription = variableDescription, VariableValue = variableValue });

                variableName = "UserName";
                variableDescription = "User Name";
                variableValue = Environment.UserName;
                list.Add(new SystemVariable { VariableName = variableName, VariableDescription = variableDescription, VariableValue = variableValue });

                variableName = "Version.Major";
                variableDescription = "CLR Major Version";
                variableValue = Environment.Version.Major.ToString(CultureInfo.CurrentCulture);
                list.Add(new SystemVariable { VariableName = variableName, VariableDescription = variableDescription, VariableValue = variableValue });

                variableName = "Version.MajorRevision";
                variableDescription = "CLR Major Revision";
                variableValue = Environment.Version.MajorRevision.ToString(CultureInfo.CurrentCulture);
                list.Add(new SystemVariable { VariableName = variableName, VariableDescription = variableDescription, VariableValue = variableValue });

                variableName = "Version.Minor";
                variableDescription = "CLR Minor Version";
                variableValue = Environment.Version.Minor.ToString(CultureInfo.CurrentCulture);
                list.Add(new SystemVariable { VariableName = variableName, VariableDescription = variableDescription, VariableValue = variableValue });

                variableName = "Version.MinorRevision";
                variableDescription = "CLR Minor Revision";
                variableValue = Environment.Version.MinorRevision.ToString(CultureInfo.CurrentCulture);
                list.Add(new SystemVariable { VariableName = variableName, VariableDescription = variableDescription, VariableValue = variableValue });

                variableName = "Version.Revision";
                variableDescription = "CLR Revision";
                variableValue = Environment.Version.Revision.ToString(CultureInfo.CurrentCulture);
                list.Add(new SystemVariable { VariableName = variableName, VariableDescription = variableDescription, VariableValue = variableValue });

                try
                {
                    foreach(DictionaryEntry environment in Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine))
                    {
                        variableName = string.Format(CultureInfo.CurrentCulture, "EnvironmentVariableTarget.Machine [key: {0}]", environment.Key);
                        variableDescription = "Machine Environment Variables";
                        variableValue = environment.Value.ToString();
                        list.Add(new SystemVariable { VariableName = variableName, VariableDescription = variableDescription, VariableValue = variableValue });
                    }

                }
                catch(SecurityException ex)
                {
                    variableName = "EXCEPTION for EnvironmentVariableTarget.Machine";
                    variableDescription = string.Empty;
                    variableValue = string.Format(CultureInfo.CurrentCulture, "Message: {0}\nGranted Set: {1}\nPermission State: {2}\nRefused State: {3}", ex.Message, ex.GrantedSet, ex.PermissionState, ex.RefusedSet);
                    list.Add(new SystemVariable { VariableName = variableName, VariableDescription = variableDescription, VariableValue = variableValue });
                }

                try
                {
                    foreach(DictionaryEntry environment in Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process))
                    {
                        variableName = string.Format(CultureInfo.CurrentCulture, "EnvironmentVariableTarget.Process [key: {0}]", environment.Key);
                        variableDescription = "Process Environment Variables";
                        variableValue = environment.Value.ToString();
                        list.Add(new SystemVariable { VariableName = variableName, VariableDescription = variableDescription, VariableValue = variableValue });
                    }
                }
                catch(SecurityException ex)
                {
                    variableName = "EXCEPTION for EnvironmentVariableTarget.Process";
                    variableDescription = string.Empty;
                    variableValue = string.Format(CultureInfo.CurrentCulture, "Message: {0}\nGranted Set: {1}\nPermission State: {2}\nRefused State: {3}", ex.Message, ex.GrantedSet, ex.PermissionState, ex.RefusedSet);
                    list.Add(new SystemVariable { VariableName = variableName, VariableDescription = variableDescription, VariableValue = variableValue });
                }

                try
                {
                    foreach(DictionaryEntry environment in Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User))
                    {
                        variableName = string.Format(CultureInfo.CurrentCulture, "EnvironmentVariableTarget.User [key: {0}]", environment.Key);
                        variableDescription = "User Environment Variables";
                        variableValue = environment.Value.ToString();
                        list.Add(new SystemVariable { VariableName = variableName, VariableDescription = variableDescription, VariableValue = variableValue });
                    }
                }
                catch(SecurityException ex)
                {
                    variableName = "EXCEPTION!";
                    variableDescription = "EXCEPTION for EnvironmentVariableTarget.User";
                    variableValue = string.Format(CultureInfo.CurrentCulture, "Message: {0}\nGranted Set: {1}\nPermission State: {2}\nRefused State: {3}", ex.Message, ex.GrantedSet, ex.PermissionState, ex.RefusedSet);
                    list.Add(new SystemVariable { VariableName = variableName, VariableDescription = variableDescription, VariableValue = variableValue });
                }

                #endregion

                return list;
        }

        private static readonly ICollection<SystemVariable> _environmentVariables = EnvironmentExtensions.ListEnvironmentVariables();
    }
}
