using Microsoft.Extensions.DependencyInjection;

namespace Songhay.Tests.Extensions;

/// <summary>
/// Extensions of <see cref="IServiceProvider"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IServiceProviderExtensions
{
    /// <summary>
    /// Gets the required <see cref="IServiceProvider"/> service
    /// with the assertion that it is not null.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <param name="serviceProvider">The service provider.</param>
    /// <remarks>
    /// This member is convenient for returning global-state (non-scoped) services
    /// like those registered with <see cref="IServiceCollection.AddSingleton"/>
    /// (e.g. Microsoft’s <c>IConfiguration</c> contract).
    /// 
    /// This member also saves one line of code for the following scoped <c>ServiceProvider</c> pattern:
    /// <code>
    /// using IServiceScope scope = _factory.Services.CreateScope();
    /// 
    /// IMyRepo myRepo = scope.ServiceProvider.GetRequiredService{IMyRepo}();
    /// Assert.NotNull(myRepo);
    /// </code>
    /// where <c>_factory</c> is an instance of an xUnit fixture
    /// like <c>IClassFixture{WebApplicationFactory{Program}}</c>.
    /// </remarks>
    public static TService GetRequiredServiceWithAssertion<TService>(this IServiceProvider? serviceProvider) where TService : notnull
    {
        Assert.NotNull(serviceProvider);

        TService? service = serviceProvider.GetRequiredService<TService>();

        Assert.NotNull(service);

        return service;
    }
}
