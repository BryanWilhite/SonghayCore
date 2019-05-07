#if NETSTANDARD

using Microsoft.Extensions.Configuration;

namespace Songhay.Models
{
    /// <summary>
    /// Defines <see cref="IConfigurationRoot"/> support
    /// for an <see cref="IActivity"/>.
    /// </summary>
    public interface IActivityConfigurationSupport
    {
        /// <summary>
        /// Adds the configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        void AddConfiguration(IConfigurationRoot configuration);
    }
}

#endif
