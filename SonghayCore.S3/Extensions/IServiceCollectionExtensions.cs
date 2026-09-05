using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Songhay.S3.Activities;
using Songhay.S3.Hosting;
using Songhay.S3.Models;

namespace Songhay.S3.Extensions;

/// <summary>
/// Extensions of <see cref="IServiceCollection"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IServiceCollectionExtensions
{

    /// <summary>
    /// Adds the dependencies associated
    /// with the domain-specific class,
    /// implementing <see cref="BackgroundService"/>
    /// by reading the name of this class.
    /// </summary>
    /// <typeparam name="TActivityGroup">the domain-specific class,
    /// sub-classing <see cref="BackgroundService"/></typeparam>
    /// <param name="services">the ambient services collected</param>
    public static IServiceCollection AddActivityGroup<TActivityGroup>(this IServiceCollection services) where TActivityGroup: class
    {
        string groupName = typeof(TActivityGroup).Name;
        switch(groupName)
        {
            case nameof(AmazonS3ActivityGroup):

                services
                    .AddKeyedTransient<IActivityTask<(string setKey, string bucketMetaKey, string bucketKey)>, AmazonS3DeleteS3ObjectActivity>(nameof(AmazonS3DeleteS3ObjectActivity))
                    .AddKeyedTransient<IActivityTask<(string setKey, string bucketMetaKey, string bucketKey), string?>, AmazonS3DownloadToStringActivity>(nameof(AmazonS3DownloadToStringActivity))
                    .AddKeyedTransient<IActivityTask<(string setKey, string bucketMetaKey, string? bucketKeyPrefix), string?>, AmazonS3ListBucketObjectsWithPaginationActivity>(nameof(AmazonS3ListBucketObjectsWithPaginationActivity))
                    .AddKeyedTransient<IActivityTask<(string setKey, string bucketMetaKey, string bucketKey, string content, string contentMimeType)>, AmazonS3UploadStringActivity>(nameof(AmazonS3UploadStringActivity))
                    .AddTransient<IActivityKeyedTaskGroup, AmazonS3ActivityGroup>();

                break;
        }

        return services;
    }

    /// <summary>
    /// Returns <see cref="IServiceCollection"/>
    /// with any available <see cref="HostOptions"/>
    /// configured in <c>appsettings.json</c>
    /// </summary>
    /// <param name="services">the <see cref="IServiceCollection"/></param>
    /// <param name="configuration">the <see cref="IConfiguration"/></param>
    public static IServiceCollection AddAnyConfiguredHostOptions(this IServiceCollection services, IConfiguration configuration)
    {
        IConfigurationSection hostOptions = configuration.GetSection(nameof(HostOptions));

        if (!hostOptions.Exists()) return services;

        services.Configure<HostOptions>(hostOptions);

        return services;
    }

    /// <summary>
    /// Calls <see cref="AddS3HostedServiceDependencies{THostedService}(IServiceCollection)"/>
    /// and <see cref="ServiceCollectionHostedServiceExtensions.AddHostedService{THostedService}(IServiceCollection)"/>
    /// for the domain-specific class,
    /// sub-classing <see cref="BackgroundService"/>.
    /// </summary>
    /// <typeparam name="THostedService">the domain-specific class,
    /// sub-classing <see cref="BackgroundService"/></typeparam>
    /// <param name="services">the <see cref="IServiceCollection"/></param>
    public static IServiceCollection AddS3HostedService<THostedService>(this IServiceCollection services) where THostedService : class, IHostedService
    {
        ArgumentNullException.ThrowIfNull(services);

        services
            .AddS3HostedServiceDependencies<THostedService>()
            .AddHostedService<THostedService>();

        return services;
    }

    /// <summary>
    /// Adds the dependencies associated
    /// with the domain-specific class,
    /// sub-classing <see cref="BackgroundService"/>
    /// by reading the name of this class.
    /// </summary>
    /// <typeparam name="THostedService">the domain-specific class,
    /// sub-classing <see cref="BackgroundService"/></typeparam>
    /// <param name="services">the ambient services collected</param>
    public static IServiceCollection AddS3HostedServiceDependencies<THostedService>(this IServiceCollection services) where THostedService : class, IHostedService
    {
        string serviceName = typeof(THostedService).Name;

        switch (serviceName)
        {
            case nameof(AmazonS3Service):

                services.AddActivityGroup<AmazonS3ActivityGroup>();

                break;
        }

        return services;
    }
}
