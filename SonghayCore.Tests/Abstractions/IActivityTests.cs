using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Meziantou.Extensions.Logging.Xunit;
using Songhay.Abstractions;

namespace Songhay.Tests.Abstractions;

public class MyActivityWithInput(IConfiguration configuration, ILogger<MyActivityWithInput> logger) : IActivity<string>
{
    public void Start(string? input)
    {
        const string key = "actual";

        configuration[key] = $"Hello {input}!";

        logger.LogInformation(configuration[key]);
    }
}

public class MyActivityWithInputAndOutput : IActivity<int,string>
{
    public string? Start(int input)
    {
        return input switch
        {
            42 => "meaning of life",
            _ => null
        };
    }
}

public class IActivityTests(ITestOutputHelper testOutputHelper)
{
    [Theory]
    [InlineData(null, "Hello !")]
    [InlineData("world", "Hello world!")]
    public void ShouldRunMyActivityWithInput(string? input, string? expected)
    {
        const string actual = "actual";

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                [actual] = null 
            })
            .Build();

        ServiceCollection services = new();

        services.AddSingleton(configuration);
        services.AddLogging();
        services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(testOutputHelper, appendScope: false));
        services.AddTransient<IActivity<string>, MyActivityWithInput>();

        ServiceProvider provider = services.BuildServiceProvider();

        IActivity<string> activity = provider.GetRequiredService<IActivity<string>>();

        activity.Start(input);
        Assert.Equal(expected, configuration[actual]);
    }
}