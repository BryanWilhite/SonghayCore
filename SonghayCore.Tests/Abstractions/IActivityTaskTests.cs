using Meziantou.Extensions.Logging.Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Songhay.Abstractions;

namespace Songhay.Tests.Abstractions;

public class MyActivityTaskWithInput(IConfiguration configuration, ILogger<MyActivityTaskWithInput> logger) : IActivityTask<string>
{
    public async Task StartAsync(string? input)
    {
        await Task.Run(() =>
        {
            const string key = "actual";

            configuration[key] = $"Hello {input}!";

            logger.LogInformation("{s}", configuration[key]);
        });
    }
}

public class MyActivityTaskWithInputAndOutput : IActivityTask<int, string>
{
    public async Task<string?> StartAsync(int input)
    {
        return await Task.FromResult(input switch
        {
            4 => "walls",
            16 => "sweet",
            42 => "meaning of life",
            _ => null
        });
    }
}

public class MyOtherActivityTaskWithInputAndOutput : IActivityTask<int, string>
{
    public async Task<string?> StartAsync(int input)
    {
        return await Task.FromResult(input switch
        {
            4 => "four",
            16 => "sixteen",
            42 => "forty-two",
            _ => null
        });
    }
}

public class MyOutputActivityTask(
    [FromKeyedServices(nameof(MyActivityTaskWithInputAndOutput))]
    IActivityTask<int, string> ioActivity,
    [FromKeyedServices(nameof(MyOtherActivityTaskWithInputAndOutput))]
    IActivityTask<int, string> otherIoActivity) : IActivityOutputOnlyTask<string[]>
{
    public async Task<string[]?> StartAsync()
    {
        var output = await Task.WhenAll(
            otherIoActivity.StartAsync(4),
            ioActivity.StartAsync(4),
            ioActivity.StartAsync(16),
            otherIoActivity.StartAsync(16),
            otherIoActivity.StartAsync(42),
            ioActivity.StartAsync(42)
        );

        return output.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray()!;
    }
}

public class IActivityTaskTestsIActivityTests(ITestOutputHelper testOutputHelper)
{
    [Theory]
    [InlineData(null, "Hello !")]
    [InlineData("world", "Hello world!")]
    public async Task ShouldRunMyActivityWithInput(string? input, string? expected)
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
        services.AddTransient<IActivityTask<string>, MyActivityTaskWithInput>();

        ServiceProvider provider = services.BuildServiceProvider();

        IActivityTask<string> activity = provider.GetRequiredService<IActivityTask<string>>();

        await activity.StartAsync(input);

        Assert.Equal(expected, configuration[actual]);
    }

    [Fact]
    public async Task ShouldRunMyOutputActivity()
    {
        ServiceCollection services = new();

        services.AddKeyedTransient<IActivityTask<int, string>, MyActivityTaskWithInputAndOutput>(nameof(MyActivityTaskWithInputAndOutput));
        services.AddKeyedTransient<IActivityTask<int, string>, MyOtherActivityTaskWithInputAndOutput>(nameof(MyOtherActivityTaskWithInputAndOutput));
        services.AddTransient<IActivityOutputOnlyTask<string[]>, MyOutputActivityTask>();

        ServiceProvider provider = services.BuildServiceProvider();

        IActivityOutputOnlyTask<string[]> activity = provider.GetRequiredService<IActivityOutputOnlyTask<string[]>>();
        var actual = await activity.StartAsync();

        Assert.NotNull(actual);

        foreach (string s in actual)
        {
            testOutputHelper.WriteLine(s);
        }
    }
}