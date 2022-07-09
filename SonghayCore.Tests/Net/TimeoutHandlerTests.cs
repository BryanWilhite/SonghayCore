using Songhay.Net;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests.Net;

public class TimeoutHandlerTests
{
    public TimeoutHandlerTests(ITestOutputHelper helper)
    {
        this._testOutputHelper = helper;
    }

    [Theory(Skip = "slowwly server is down")]
    [InlineData("http://slowwly.robertomurray.co.uk/delay/3000/url/http://www.google.co.uk", 1)]
    public async Task ShouldCancel(string location, int timeInSeconds)
    {
        var handler = new TimeoutHandler
        {
            RequestTimeout = TimeSpan.FromSeconds(10),
            InnerHandler = new HttpClientHandler()
        };

        using (var cts = new CancellationTokenSource())
        using (var client = new HttpClient(handler))
        {
            try
            {
                client.Timeout = Timeout.InfiniteTimeSpan;

                this._testOutputHelper.WriteLine($"calling `{location}`...");
                var request = new HttpRequestMessage(HttpMethod.Get, location);

                cts.CancelAfter(TimeSpan.FromSeconds(timeInSeconds));

                using var response = await client
                    .SendAsync(request, cts.Token)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception ex)
            {
                Assert.IsType<TaskCanceledException>(ex);
            }
        }
    }

    [Theory(Skip = "slowwly server is down")]
    [InlineData("http://slowwly.robertomurray.co.uk/delay/3000/url/http://www.google.co.uk", 1)]
    public async Task ShouldTimeout(string location, int timeInSeconds)
    {
        var handler = new TimeoutHandler
        {
            RequestTimeout = TimeSpan.FromSeconds(timeInSeconds),
            InnerHandler = new HttpClientHandler()
        };

        using (var cts = new CancellationTokenSource())
        using (var client = new HttpClient(handler))
        {
            try
            {
                client.Timeout = Timeout.InfiniteTimeSpan;

                this._testOutputHelper.WriteLine($"calling `{location}`...");
                var request = new HttpRequestMessage(HttpMethod.Get, location);

                using var response = await client
                    .SendAsync(request, cts.Token)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception ex)
            {
                Assert.IsType<TimeoutException>(ex);
            }
        }
    }

    readonly ITestOutputHelper _testOutputHelper;
}