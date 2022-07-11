using System.Security;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="System.Environment"/>.
/// </summary>
public static class EnvironmentExtensions
{
    /// <summary>
    /// Gets the conventional value.
    /// </summary>
    /// <param name="conventionalName">Name of the conventional.</param>
    /// <returns>Returns the value of named data.</returns>
    public static string? GetConventionalValue(string? conventionalName)
    {
        return EnvironmentVariables.Value
            .Where(e => e.VariableName.EqualsInvariant(conventionalName))
            .Select(e => e.VariableValue).First();
    }

    static ICollection<SystemVariable> ListEnvironmentVariables()
    {
        var list = new List<SystemVariable>();

        // ReSharper disable once RedundantAssignment
        var variableName = "";
        // ReSharper disable once RedundantAssignment
        var variableDescription = "";
        // ReSharper disable once RedundantAssignment
        var variableValue = "";

        #region Insert data into list:

        variableName = "MachineName";
        variableDescription = "Network Identification";
        variableValue = Environment.MachineName;
        list.Add(new SystemVariable
        {
            VariableName = variableName,
            VariableDescription = variableDescription,
            VariableValue = variableValue
        });

        variableName = "OSVersion.Platform";
        variableDescription = "Operating System Platform";
        variableValue = Environment.OSVersion.Platform.ToString();
        list.Add(new SystemVariable
        {
            VariableName = variableName,
            VariableDescription = variableDescription,
            VariableValue = variableValue
        });

        variableName = "OSVersion.ServicePack";
        variableDescription = "Operating System Service Pack";
        variableValue = Environment.OSVersion.ServicePack;
        list.Add(new SystemVariable
        {
            VariableName = variableName,
            VariableDescription = variableDescription,
            VariableValue = variableValue
        });

        variableName = "OSVersion.VersionString";
        variableDescription = "Operating System Version Summary";
        variableValue = Environment.OSVersion.VersionString;
        list.Add(new SystemVariable
        {
            VariableName = variableName,
            VariableDescription = variableDescription,
            VariableValue = variableValue
        });

        variableName = "UserDomainName";
        variableDescription = "User Domain Name";
        variableValue = Environment.UserDomainName;
        list.Add(new SystemVariable
        {
            VariableName = variableName,
            VariableDescription = variableDescription,
            VariableValue = variableValue
        });

        variableName = "UserName";
        variableDescription = "User Name";
        variableValue = Environment.UserName;
        list.Add(new SystemVariable
        {
            VariableName = variableName,
            VariableDescription = variableDescription,
            VariableValue = variableValue
        });

        variableName = "Version.Major";
        variableDescription = "CLR Major Version";
        variableValue = Environment.Version.Major.ToString(CultureInfo.InvariantCulture);
        list.Add(new SystemVariable
        {
            VariableName = variableName,
            VariableDescription = variableDescription,
            VariableValue = variableValue
        });

        variableName = "Version.MajorRevision";
        variableDescription = "CLR Major Revision";
        variableValue = Environment.Version.MajorRevision.ToString(CultureInfo.InvariantCulture);
        list.Add(new SystemVariable
        {
            VariableName = variableName,
            VariableDescription = variableDescription,
            VariableValue = variableValue
        });

        variableName = "Version.Minor";
        variableDescription = "CLR Minor Version";
        variableValue = Environment.Version.Minor.ToString(CultureInfo.InvariantCulture);
        list.Add(new SystemVariable
        {
            VariableName = variableName,
            VariableDescription = variableDescription,
            VariableValue = variableValue
        });

        variableName = "Version.MinorRevision";
        variableDescription = "CLR Minor Revision";
        variableValue = Environment.Version.MinorRevision.ToString(CultureInfo.InvariantCulture);
        list.Add(new SystemVariable
        {
            VariableName = variableName,
            VariableDescription = variableDescription,
            VariableValue = variableValue
        });

        variableName = "Version.Revision";
        variableDescription = "CLR Revision";
        variableValue = Environment.Version.Revision.ToString(CultureInfo.InvariantCulture);
        list.Add(new SystemVariable
        {
            VariableName = variableName,
            VariableDescription = variableDescription,
            VariableValue = variableValue
        });

        try
        {
            foreach (DictionaryEntry environment in Environment.GetEnvironmentVariables(
                         EnvironmentVariableTarget.Machine))
            {
                variableName = $"EnvironmentVariableTarget.Machine [key: {environment.Key}]";
                variableDescription = "Machine Environment Variables";
                variableValue = environment.Value?.ToString();

                if (string.IsNullOrWhiteSpace(variableValue)) continue;

                list.Add(new SystemVariable
                {
                    VariableName = variableName,
                    VariableDescription = variableDescription,
                    VariableValue = variableValue
                });
            }
        }
        catch (SecurityException ex)
        {
            variableName = "EXCEPTION for EnvironmentVariableTarget.Machine";
            variableDescription = string.Empty;
            variableValue =
                $"Message: {ex.Message}\nGranted Set: {ex.GrantedSet}\nPermission State: {ex.PermissionState}\nRefused State: {ex.RefusedSet}";
            list.Add(new SystemVariable
            {
                VariableName = variableName,
                VariableDescription = variableDescription,
                VariableValue = variableValue
            });
        }

        try
        {
            foreach (DictionaryEntry environment in Environment.GetEnvironmentVariables(
                         EnvironmentVariableTarget.Process))
            {
                variableName = $"EnvironmentVariableTarget.Process [key: {environment.Key}]";
                variableDescription = "Process Environment Variables";
                variableValue = environment.Value?.ToString();

                if (string.IsNullOrWhiteSpace(variableValue)) continue;

                list.Add(new SystemVariable
                {
                    VariableName = variableName,
                    VariableDescription = variableDescription,
                    VariableValue = variableValue
                });
            }
        }
        catch (SecurityException ex)
        {
            variableName = "EXCEPTION for EnvironmentVariableTarget.Process";
            variableDescription = string.Empty;
            variableValue =
                $"Message: {ex.Message}\nGranted Set: {ex.GrantedSet}\nPermission State: {ex.PermissionState}\nRefused State: {ex.RefusedSet}";
            list.Add(new SystemVariable
            {
                VariableName = variableName,
                VariableDescription = variableDescription,
                VariableValue = variableValue
            });
        }

        try
        {
            foreach (DictionaryEntry environment in Environment.GetEnvironmentVariables(
                         EnvironmentVariableTarget.User))
            {
                variableName = $"EnvironmentVariableTarget.User [key: {environment.Key}]";
                variableDescription = "User Environment Variables";
                variableValue = environment.Value?.ToString();

                if (string.IsNullOrWhiteSpace(variableValue)) continue;

                list.Add(new SystemVariable
                {
                    VariableName = variableName,
                    VariableDescription = variableDescription,
                    VariableValue = variableValue
                });
            }
        }
        catch (SecurityException ex)
        {
            variableName = "EXCEPTION!";
            variableDescription = "EXCEPTION for EnvironmentVariableTarget.User";
            variableValue =
                $"Message: {ex.Message}\nGranted Set: {ex.GrantedSet}\nPermission State: {ex.PermissionState}\nRefused State: {ex.RefusedSet}";
            list.Add(new SystemVariable
            {
                VariableName = variableName,
                VariableDescription = variableDescription,
                VariableValue = variableValue
            });
        }

        #endregion

        return list;
    }

    static readonly Lazy<ICollection<SystemVariable>> EnvironmentVariables =
        new(ListEnvironmentVariables, LazyThreadSafetyMode.PublicationOnly);
}
