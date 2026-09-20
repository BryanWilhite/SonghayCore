using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Songhay.Abstractions;
using Songhay.Models;

namespace Songhay.Tests.Abstractions;

public class MyActivityTaskWithInput(IConfiguration configuration, ILogger<MyActivityTaskWithInput> logger) : IActivityTask<string>
{
    public Task StartAsync(string? input, CancellationToken cancellationToken)
    {
        return Task.Run(() =>
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
    IActivityTask<int, ProgramOutputResult<string?>> ioActivity,
    [FromKeyedServices(nameof(MyOtherActivityTaskWithInputAndOutput))]
    IActivityTask<int, ProgramOutputResult<string?>> otherIoActivity) : IActivityOutputOnlyTask<ProgramOutputResult<string[]>>
{
    public async Task<ProgramOutputResult<string[]>> StartAsync(CancellationToken cancellationToken)
    {
        ProgramOutputResult<string?>[] results = await Task.WhenAll(
            otherIoActivity.StartAsync(4, cancellationToken),
            ioActivity.StartAsync(4, cancellationToken),
            ioActivity.StartAsync(16, cancellationToken),
            otherIoActivity.StartAsync(16, cancellationToken),
            otherIoActivity.StartAsync(42, cancellationToken),
            ioActivity.StartAsync(4, cancellationToken)
        );

        string[] aggregate =
            [..
                results
                    .Where(result => !string.IsNullOrWhiteSpace(result.Output))
                    .Select(result => result.Output)
                    .OfType<string>()
            ];

        return new ProgramOutputResult<string[]>(true, "Looks like they all worked out.", aggregate);
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

        IServiceProvider provider = services.BuildServiceProvider();

        var activity = provider.GetRequiredService<IActivityOutputOnlyTask<ProgramOutputResult<string[]>>>();
        ProgramOutputResult<string[]> actual = await activity.StartAsync(CancellationToken.None);

        Assert.NotNull(actual);

        foreach (string s in actual.Output.ToReferenceTypeValueOrThrow())
        {
            testOutputHelper.WriteLine(s);
        }
    }
}
