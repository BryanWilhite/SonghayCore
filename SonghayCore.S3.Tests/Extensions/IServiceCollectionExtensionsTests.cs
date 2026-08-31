using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Songhay.S3.Extensions;

namespace Songhay.S3.Tests.Extensions;

public class IServiceCollectionExtensionsTests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void ShouldAddAnyConfiguredHostOptions()
    {
        //arrange:
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("./appsettings.json")
            .Build();

        testOutputHelper.WriteLine($"has {nameof(HostOptions)} section? {configuration.HasKey(nameof(HostOptions))}");

        HostOptions expected = configuration.BindNewInstance<HostOptions>().ToReferenceTypeValueOrThrow();

        IServiceProvider provider = new ServiceCollection()
            .AddSingleton(configuration)
            .AddAnyConfiguredHostOptions(configuration)
            .BuildServiceProvider();

        //act:
        IOptions<HostOptions> actual = provider.GetRequiredService<IOptions<HostOptions>>();

        //assert:
        Assert.Equivalent(expected, actual.Value);
    }

    [Fact]
    public void ShouldNotAddAnyConfiguredHostOptions()
    {
        //arrange:
        IConfiguration configuration = new ConfigurationBuilder().Build();

        testOutputHelper.WriteLine($"has {nameof(HostOptions)} section? {configuration.HasKey(nameof(HostOptions))}");

        IServiceProvider provider = new ServiceCollection()
            .AddSingleton(configuration)
            .AddAnyConfiguredHostOptions(configuration)
            .BuildServiceProvider();

        //act:
        IOptions<HostOptions>? actual = provider.GetService<IOptions<HostOptions>>();

        //assert:
        Assert.Null(actual);
    }
}
