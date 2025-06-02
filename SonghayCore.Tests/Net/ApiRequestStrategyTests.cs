using Songhay.Abstractions;
using Songhay.Net;

namespace Songhay.Tests.Net;

public class ApiRequestStrategyTests
{
    [Theory]
    [InlineData("https://microsoft.com/build")]
    public void GenerateHttpRequestMessage_String_Test(string location)
    {
        IApiRequestStrategy strategy = new ApiRequestStrategy(messageGetter: null, instanceTag: "default");

        HttpRequestMessage actual = strategy.GenerateHttpRequestMessage(location);

        Assert.Equal(location, actual.RequestUri?.AbsoluteUri);
    }

    [Theory]
    [InlineData("https://microsoft.com/build")]
    public void GenerateHttpRequestMessage_Uri_Test(string location)
    {
        Uri uri = new(location);

        IApiRequestStrategy strategy = new ApiRequestStrategy(messageGetter: null, instanceTag: "default");

        HttpRequestMessage actual = strategy.GenerateHttpRequestMessage(uri);

        Assert.Equal(location, actual.RequestUri?.AbsoluteUri);
    }

    [Theory]
    [InlineData("https://microsoft.com/build")]
    public void GenerateHttpRequestMessage_UriBuilder_Test(string location)
    {
        UriBuilder builder = new(location);

        IApiRequestStrategy strategy = new ApiRequestStrategy(messageGetter: null, instanceTag: "default");

        HttpRequestMessage actual = strategy.GenerateHttpRequestMessage(builder);

        Assert.Equal(location, actual.RequestUri?.AbsoluteUri);
    }

    [Theory]
    [InlineData("https://microsoft.com/build")]
    public void GenerateHttpRequestMessage_StringTuple_Test(string location)
    {
        var t = new Tuple<HttpMethod, string>(HttpMethod.Patch, location);
        IApiRequestStrategy strategy = new ApiRequestStrategy(messageGetter: null, instanceTag: "default");

        HttpRequestMessage actual = strategy.GenerateHttpRequestMessage(t);

        Assert.Equal(t.Item2, actual.RequestUri?.AbsoluteUri);
        Assert.Equal(t.Item1, actual.Method);
    }

    [Theory]
    [InlineData("https://microsoft.com/build")]
    public void GenerateHttpRequestMessage_StringValueTuple_Test(string location)
    {
        (HttpMethod method, string loc) t = (HttpMethod.Patch, location);
        IApiRequestStrategy strategy = new ApiRequestStrategy(messageGetter: null, instanceTag: "default");

        HttpRequestMessage actual = strategy.GenerateHttpRequestMessage(t);

        Assert.Equal(t.loc, actual.RequestUri?.AbsoluteUri);
        Assert.Equal(t.method, actual.Method);
    }

    [Theory]
    [InlineData("https://microsoft.com/build")]
    public void GenerateHttpRequestMessage_UriValueTuple_Test(string location)
    {
        (HttpMethod method, Uri uri) t = (HttpMethod.Patch, new Uri(location));
        IApiRequestStrategy strategy = new ApiRequestStrategy(messageGetter: null, instanceTag: "default");

        HttpRequestMessage actual = strategy.GenerateHttpRequestMessage(t);

        Assert.Equal(t.uri, actual.RequestUri);
        Assert.Equal(t.method, actual.Method);
    }

    [Theory]
    [InlineData("https://microsoft.com/build")]
    public void GenerateHttpRequestMessage_UriBuilderValueTuple_Test(string location)
    {
        (HttpMethod method, UriBuilder builder) t = (HttpMethod.Patch, new UriBuilder(location));
        IApiRequestStrategy strategy = new ApiRequestStrategy(messageGetter: null, instanceTag: "default");

        HttpRequestMessage actual = strategy.GenerateHttpRequestMessage(t);

        Assert.Equal(t.builder.Uri, actual.RequestUri);
        Assert.Equal(t.method, actual.Method);
    }
}
