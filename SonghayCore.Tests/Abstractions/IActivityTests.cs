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

        logger.LogInformation("{s}", configuration[key]);
    }
}

public class MyActivityWithInputAndOutput : IActivity<int, string>
{
    public string? Start(int input)
    {
        return input switch
        {
            4 => "walls",
            16 => "sweet",
            42 => "meaning of life",
            _ => null
        };
    }
}

public class MyOtherActivityWithInputAndOutput : IActivity<int, string>
{
    public string? Start(int input)
    {
        return input switch
        {
            4 => "four",
            16 => "sixteen",
            42 => "forty-two",
            _ => null
        };
    }
}

public class MyOutputActivity(
    [FromKeyedServices(nameof(MyActivityWithInputAndOutput))] IActivity<int, string> ioActivity,
    [FromKeyedServices(nameof(MyOtherActivityWithInputAndOutput))] IActivity<int, string> otherIoActivity) : IActivityOutputOnly<string[]>
{
    public string[] Start()
    {
        List<string?> output =
        [
            otherIoActivity.Start(4),
            ioActivity.Start(4),
            ioActivity.Start(16),
            otherIoActivity.Start(16),
            ioActivity.Start(42),
            otherIoActivity.Start(42),
        ];

        return output.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray()!;
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

    [Fact]
    public void ShouldRunMyOutputActivity()
    {
        ServiceCollection services = new();

        services.AddKeyedTransient<IActivity<int, string>, MyActivityWithInputAndOutput>(nameof(MyActivityWithInputAndOutput));
        services.AddKeyedTransient<IActivity<int, string>, MyOtherActivityWithInputAndOutput>(nameof(MyOtherActivityWithInputAndOutput));
        services.AddTransient<IActivityOutputOnly<string[]>, MyOutputActivity>();

        ServiceProvider provider = services.BuildServiceProvider();

        IActivityOutputOnly<string[]> activity = provider.GetRequiredService<IActivityOutputOnly<string[]>>();
        var actual = activity.Start();

        Assert.NotNull(actual);

        foreach (string s in actual)
        {
            testOutputHelper.WriteLine(s);
        }
    }
}