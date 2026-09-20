using Microsoft.Extensions.DependencyInjection;

using Songhay.Models;
using Songhay.Web.Models;

namespace Songhay.Web.Extensions;

/// <summary>
/// Extensions of <see cref="IServiceCollection"/>
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Adds the instance of <see cref="RestApiMetadata"/>
    /// used for <see cref="ApiKeyAuthenticationHandler"/>
    /// </summary>
    /// <param name="services">the <see cref="IServiceCollection"/></param>
    /// <param name="restApiMetadata">the <see cref="ApiKeyAuthenticationHandler"/></param>
    public static IServiceCollection AddRestApiMetadataForApiKey(this IServiceCollection services, RestApiMetadata restApiMetadata)
    {
        services
            .AddKeyedSingleton(ApiKeyConstants.DepKeyForRestApiMetadata, restApiMetadata);

        return services;
    }
}
