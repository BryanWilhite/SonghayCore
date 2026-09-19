using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Songhay.Abstractions;
using Songhay.Models;

namespace Songhay.Tests.Abstractions;

public class MyActivityTaskWithInput(IConfiguration configuration, ILogger<MyActivityTaskWithInput> logger) : IActivityTask<string>
{
    public async Task StartAsync(string? input, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            const string key = "actual";

            configuration[key] = $"Hello {input}!";

            logger.LogInformation("{s}", configuration[key]);

        }, cancellationToken);
    }
}

public class MyActivityTaskWithInputAndOutput : IActivityTask<int, ProgramOutputResult<string?>>
{
    public async Task<ProgramOutputResult<string?>> StartAsync(int input, CancellationToken cancellationToken)
    {
        string? output = await Task.FromResult(input switch
        {
            4 => "walls",
            16 => "sweet",
            42 => "meaning of life",
            _ => null
        });

        return new ProgramOutputResult<string?>(true, "This one is fine.", output);
    }
}

public class MyOtherActivityTaskWithInputAndOutput : IActivityTask<int, ProgramOutputResult<string?>>
{
    public async Task<ProgramOutputResult<string?>> StartAsync(int input, CancellationToken cancellationToken)
    {
        string? output =  await Task.FromResult(input switch
        {
            4 => "four",
            16 => "sixteen",
            42 => "forty-two",
            _ => null
        });

        return new ProgramOutputResult<string?>(true, "This other one is fine.", output);
    }
}

public class MyOutputActivityTask(
    [FromKeyedServices(nameof(MyActivityTaskWithInputAndOutput))]
    IActivityTask<int, string> ioActivity,
    [FromKeyedServices(nameof(MyOtherActivityTaskWithInputAndOutput))]
    IActivityTask<int, string> otherIoActivity) : IActivityOutputOnlyTask<ProgramOutputResult<string[]>>
{
    public async Task<ProgramOutputResult<string[]>> StartAsync(CancellationToken cancellationToken)
    {
        string[] aggregate = await Task.WhenAll(
            otherIoActivity.StartAsync(4, cancellationToken),
            ioActivity.StartAsync(4, cancellationToken),
            ioActivity.StartAsync(16, cancellationToken),
            otherIoActivity.StartAsync(16, cancellationToken),
            otherIoActivity.StartAsync(42, cancellationToken),
            ioActivity.StartAsync(4, cancellationToken)
        );

        string[] output = [.. aggregate.Where(s => !string.IsNullOrWhiteSpace(s))];

        return new ProgramOutputResult<string[]>(true, "Looks like they all worked out.", output);
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
                [actual] = null,
            })
            .Build();

        ServiceCollection services = new();

        services.AddSingleton(configuration);
        services.AddLogging();
        services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(testOutputHelper, appendScope: false));
        services.AddTransient<IActivityTask<string>, MyActivityTaskWithInput>();

        ServiceProvider provider = services.BuildServiceProvider();

        IActivityTask<string> activity = provider.GetRequiredService<IActivityTask<string>>();

        await activity.StartAsync(input, CancellationToken.None);

        Assert.Equal(expected, configuration[actual]);
    }

    [Fact]
    public async Task ShouldRunMyOutputActivity()
    {
        ServiceCollection services = new();

        services.AddKeyedTransient<IActivityTask<int, ProgramOutputResult<string?>>, MyActivityTaskWithInputAndOutput>(nameof(MyActivityTaskWithInputAndOutput));
        services.AddKeyedTransient<IActivityTask<int, ProgramOutputResult<string?>>, MyOtherActivityTaskWithInputAndOutput>(nameof(MyOtherActivityTaskWithInputAndOutput));
        services.AddTransient<IActivityOutputOnlyTask<ProgramOutputResult<string[]>>, MyOutputActivityTask>();

        ServiceProvider provider = services.BuildServiceProvider();

        IActivityOutputOnlyTask<string[]> activity = provider.GetRequiredService<IActivityOutputOnlyTask<string[]>>();
        var actual = await activity.StartAsync(CancellationToken.None);

        Assert.NotNull(actual);

        foreach (string s in actual)
        {
            testOutputHelper.WriteLine(s);
        }
    }
}
