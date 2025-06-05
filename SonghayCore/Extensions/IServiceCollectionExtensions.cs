using Microsoft.Extensions.DependencyInjection;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="IServiceCollection"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Adds the conventional <see cref="ProgramMetadata"/> instance as a singleton.
    /// </summary>
    /// <param name="services">the <see cref="IServiceCollection"/></param>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    public static IServiceCollection AddProgramMetadata(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        ProgramMetadata programMetadata = configuration.BindNewInstance<ProgramMetadata>();
        programMetadata.EnsureProgramMetadata();

        services.AddSingleton(programMetadata);

        return services;
    }

}
