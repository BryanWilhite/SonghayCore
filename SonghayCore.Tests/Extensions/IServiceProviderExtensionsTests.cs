namespace MyNamespace
{
    public interface IMyService
    {
        string GetThatThing();
    }

    public class MyService : IMyService
    {
        public string GetThatThing() => nameof(MyService);
    }
}

namespace Songhay.Tests.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using MyNamespace;

    // ReSharper disable once InconsistentNaming
    public class IServiceProviderExtensionsTests
    {
        // ReSharper disable once InconsistentNaming
        public static IEnumerable<object[]> TestData =
        [
            [
                Host
                    .CreateDefaultBuilder([])
                    .ConfigureServices((hostContext, services) => { services.AddTransient<IMyService, MyService>(); })
                    .Build()
            ]
        ];

        [Theory]
        [MemberData(nameof(TestData))]
        public void GetRequiredServiceWithAssertion_Test(IHost host)
        {
            IMyService actual = host.Services.GetRequiredServiceWithAssertion<IMyService>();
            Assert.NotNull(actual);
            Assert.Equal(nameof(MyService), actual.GetThatThing());
        }
    }
}
