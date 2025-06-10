using System.Configuration;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="ITaggedInstance"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class ITaggedInstanceExtensions
{
    /// <summary>
    /// Throws <see cref="ConfigurationErrorsException"/>
    /// when <see cref="IsExpectedInstance"/> returns <c>false</c>.
    /// </summary>
    /// <param name="instance">the <see cref="ITaggedInstance"/></param>
    /// <param name="instanceTag">the conventional identifier</param>
    /// <exception cref="ConfigurationErrorsException"></exception>
    public static void EnsureExpectedInstance([NotNull] this ITaggedInstance? instance, string? instanceTag)
    {
        if (!instance.IsExpectedInstance(instanceTag))
        {
            throw new ConfigurationErrorsException($"`{instanceTag}` was expected. Found `{instance?.InstanceTag}` instead.");
        }
    }

    /// <summary>
    /// Returns <c>true</c> when <see cref="ITaggedInstance.InstanceTag"/> equals the specified identifier.
    /// </summary>
    /// <param name="instance">the <see cref="ITaggedInstance"/></param>
    /// <param name="instanceTag">the conventional identifier</param>
    public static bool IsExpectedInstance([NotNullWhen(true)] this ITaggedInstance? instance, string? instanceTag) =>
        instance != null
        && !string.IsNullOrWhiteSpace(instanceTag)
        && instance.InstanceTag.EqualsInvariant(instanceTag);
}
