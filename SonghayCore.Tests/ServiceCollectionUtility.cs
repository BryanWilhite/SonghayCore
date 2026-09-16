using Microsoft.Extensions.DependencyInjection;

namespace Songhay.Tests;

/// <summary>
/// Shared automated test routines for <see cref="ServiceCollection"/>
/// </summary>
public static class ServiceCollectionUtility
{
    public static IHttpClientFactory GetHttpClientFactory(ServiceCollection? serviceCollection)
    {
        serviceCollection ??= new ServiceCollection();

        serviceCollection.AddHttpClient(); // registers IHttpClientFactory

        ServiceProvider provider = serviceCollection.BuildServiceProvider();
        IHttpClientFactory factory = provider.GetRequiredService<IHttpClientFactory>();

        return factory;
    }
}
