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
        var list = new List<SystemVariable>
        {
            new()
            {
                VariableName = nameof(Environment.MachineName),
                VariableDescription = "Network Identification",
                VariableValue = Environment.MachineName
            },
            new()
            {
                VariableName = $"{nameof(Environment.OSVersion)}.{nameof(Environment.OSVersion.Platform)}",
                VariableDescription = "Operating System Platform",
                VariableValue = Environment.OSVersion.Platform.ToString()
            },
            new()
            {
                VariableName = $"{nameof(Environment.OSVersion)}.{nameof(Environment.OSVersion.ServicePack)}",
                VariableDescription = "Operating System Service Pack",
                VariableValue = Environment.OSVersion.ServicePack
            },
            new()
            {
                VariableName = $"{nameof(Environment.OSVersion)}.{nameof(Environment.OSVersion.VersionString)}",
                VariableDescription = "Operating System Version Summary",
                VariableValue = Environment.OSVersion.VersionString
            },
            new()
            {
                VariableName = nameof(Environment.UserDomainName),
                VariableDescription = "User Domain Name",
                VariableValue = Environment.UserDomainName
            },
            new()
            {
                VariableName = nameof(Environment.UserName),
                VariableDescription = "User Name",
                VariableValue = Environment.UserName
            },
            new()
            {
                VariableName = nameof(Environment.Version.Major),
                VariableDescription = "CLR Major Version",
                VariableValue = Environment.Version.Major.ToString(CultureInfo.InvariantCulture)
            },
            new()
            {
                VariableName = $"{nameof(Environment.Version)}.{nameof(Environment.Version.MajorRevision)}",
                VariableDescription = "CLR Major Revision",
                VariableValue = Environment.Version.MajorRevision.ToString(CultureInfo.InvariantCulture)
            },
            new()
            {
                VariableName = $"{nameof(Environment.Version)}.{nameof(Environment.Version.Minor)}",
                VariableDescription = "CLR Minor Version",
                VariableValue = Environment.Version.Minor.ToString(CultureInfo.InvariantCulture)
            },
            new()
            {
                VariableName = $"{nameof(Environment.Version)}.{nameof(Environment.Version.MinorRevision)}",
                VariableDescription = "CLR Minor Revision",
                VariableValue = Environment.Version.MinorRevision.ToString(CultureInfo.InvariantCulture)
            },
            new()
            {
                VariableName = $"{nameof(Environment.Version)}.{nameof(Environment.Version.Revision)}",
                VariableDescription = "CLR Revision",
                VariableValue = Environment.Version.Revision.ToString(CultureInfo.InvariantCulture)
            }
        };

        #region Insert data into list:

        try
        {
            foreach (DictionaryEntry environment in Environment.GetEnvironmentVariables(
                         EnvironmentVariableTarget.Machine))
            {
                if (string.IsNullOrWhiteSpace(environment.Value?.ToString())) continue;

                list.Add(new SystemVariable
                {
                    VariableName = $"{nameof(EnvironmentVariableTarget)}.{nameof(EnvironmentVariableTarget.Machine)} [key: {environment.Key}]",
                    VariableDescription = "Machine Environment Variables",
                    VariableValue = environment.Value?.ToString()
                });
            }
        }
        catch (SecurityException ex)
        {
            list.Add(new SystemVariable
            {
                VariableName = $"EXCEPTION! {nameof(EnvironmentVariableTarget)}.{nameof(EnvironmentVariableTarget.Machine)}]",
                VariableDescription = $"EXCEPTION for {nameof(EnvironmentVariableTarget)}.{nameof(EnvironmentVariableTarget.Machine)}",
                VariableValue = $"Message: {ex.Message}\nGranted Set: {ex.GrantedSet}\nPermission State: {ex.PermissionState}\nRefused State: {ex.RefusedSet}"
            });
        }

        try
        {
            foreach (DictionaryEntry environment in Environment.GetEnvironmentVariables(
                         EnvironmentVariableTarget.Process))
            {
                if (string.IsNullOrWhiteSpace(environment.Value?.ToString())) continue;

                list.Add(new SystemVariable
                {
                    VariableName = $"{nameof(EnvironmentVariableTarget)}.{nameof(EnvironmentVariableTarget.Process)} [key: {environment.Key}]",
                    VariableDescription = "Process Environment Variables",
                    VariableValue = environment.Value?.ToString()
                });
            }
        }
        catch (SecurityException ex)
        {
            list.Add(new SystemVariable
            {
                VariableName = $"EXCEPTION! {nameof(EnvironmentVariableTarget)}.{nameof(EnvironmentVariableTarget.Process)}]",
                VariableDescription = $"EXCEPTION for {nameof(EnvironmentVariableTarget)}.{nameof(EnvironmentVariableTarget.Process)}",
                VariableValue = $"Message: {ex.Message}\nGranted Set: {ex.GrantedSet}\nPermission State: {ex.PermissionState}\nRefused State: {ex.RefusedSet}"
            });
        }

        try
        {
            foreach (DictionaryEntry environment in Environment.GetEnvironmentVariables(
                         EnvironmentVariableTarget.User))
            {
                if (string.IsNullOrWhiteSpace(environment.Value?.ToString())) continue;

                list.Add(new SystemVariable
                {
                    VariableName = $"{nameof(EnvironmentVariableTarget)}.{nameof(EnvironmentVariableTarget.User)} [key: {environment.Key}]",
                    VariableDescription = "User Environment Variables",
                    VariableValue = environment.Value?.ToString()
                });
            }
        }
        catch (SecurityException ex)
        {
            list.Add(new SystemVariable
            {
                VariableName = $"EXCEPTION! {nameof(EnvironmentVariableTarget)}.{nameof(EnvironmentVariableTarget.User)}]",
                VariableDescription = $"EXCEPTION for {nameof(EnvironmentVariableTarget)}.{nameof(EnvironmentVariableTarget.User)}",
                VariableValue = $"Message: {ex.Message}\nGranted Set: {ex.GrantedSet}\nPermission State: {ex.PermissionState}\nRefused State: {ex.RefusedSet}"
            });
        }

        #endregion

        return list;
    }

    static readonly Lazy<ICollection<SystemVariable>> EnvironmentVariables =
        new(ListEnvironmentVariables, LazyThreadSafetyMode.PublicationOnly);
}
