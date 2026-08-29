using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Songhay.S3.Activities;
using Songhay.S3.Hosting;

namespace Songhay.S3.Extensions;

/// <summary>
/// Extensions of <see cref="IServiceCollection"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Calls <see cref="AddS3HostedServiceDependencies{THostedService}(IServiceCollection)"/>
    /// and <see cref="ServiceCollectionHostedServiceExtensions.AddHostedService{THostedService}(IServiceCollection)"/>
    /// for the domain-specific class,
    /// sub-classing <see cref="BackgroundService"/>.
    /// </summary>
    /// <typeparam name="THostedService">the domain-specific class,
    /// sub-classing <see cref="BackgroundService"/></typeparam>
    /// <param name="services">the ambient services collected</param>
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
        string procedureName = typeof(THostedService).Name;

        switch (procedureName)
        {
            case nameof(AmazonS3Service):
                services
                    .AddKeyedTransient<IActivityTask<(string setKey, string bucketMetaKey, string bucketKey)>, AmazonS3DeleteS3ObjectActivity>(nameof(AmazonS3DeleteS3ObjectActivity))
                    .AddKeyedTransient<IActivityTask<(string setKey, string bucketMetaKey, string bucketKey), string?>, AmazonS3DownloadToStringActivity>(nameof(AmazonS3DownloadToStringActivity))
                    .AddKeyedTransient<IActivityTask<(string setKey, string bucketMetaKey), string?>, AmazonS3ListBucketObjectsWithPaginationActivity>(nameof(AmazonS3ListBucketObjectsWithPaginationActivity))
                    .AddKeyedTransient<IActivityTask<(string setKey, string bucketMetaKey, string bucketKey, string content, string contentMimeType)>, AmazonS3UploadStringActivity>(nameof(AmazonS3UploadStringActivity));

                break;
        }

        return services;
    }
}
