using Microsoft.Extensions.DependencyInjection;
using Songhay.Activities;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="IServiceCollection"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Adds the dependencies associated
    /// with the domain-specific class,
    /// implementing <see cref="IActivityKeyedTaskGroup{TOutput}"/>
    /// by reading the name of this class.
    /// </summary>
    /// <typeparam name="TActivityGroup">the domain-specific class,
    /// implementing <see cref="IActivityKeyedTaskGroup{TOutput}"/></typeparam>
    /// <param name="services">the ambient services collected</param>
    public static IServiceCollection AddActivityKeyedTaskGroup<TActivityGroup>(this IServiceCollection services) where TActivityGroup: IActivityKeyedTaskGroup<EndpointResult>
    {
        string groupName = typeof(TActivityGroup).Name;
        switch(groupName)
        {
            case nameof(ProgramFileActivityGroup):

                services
                    .AddTransient<IActivityTask<StorageActivityInput?, EndpointResult>, ProgramFileDeleteActivity>()
                    .AddTransient<IActivityTask<StorageActivityInput?, EndpointContentResult<string?>>, ProgramFileReadActivity>()
                    .AddTransient<IActivityTask<StorageActivityInput?, EndpointContentResult<IReadOnlyCollection<StorageObject>>>, ProgramFileListActivity>()
                    .AddTransient<IActivityTask<StorageActivityInput<string?>?, EndpointResult>, ProgramFileSaveActivity>()
                    .AddTransient<IActivityKeyedTaskGroup<EndpointResult>, ProgramFileActivityGroup>();

                break;
        }

        return services;
    }

    /// <summary>
    /// Adds the conventional <see cref="ProgramMetadata"/> instance as a singleton.
    /// </summary>
    /// <param name="services">the <see cref="IServiceCollection"/></param>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    public static IServiceCollection AddProgramMetadata(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        ProgramMetadata? programMetadata = configuration.BindNewInstance<ProgramMetadata>();
        programMetadata.EnsureProgramMetadata();

        services.AddSingleton(programMetadata);

        return services;
    }

}
