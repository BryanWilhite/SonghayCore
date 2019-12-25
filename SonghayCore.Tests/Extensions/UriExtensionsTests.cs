using Songhay.Extensions;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests.Extensions
{
    public class UriExtensionsTests
    {
        public UriExtensionsTests(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
        }

        [Theory]
        [InlineData("https://t.co/bS1b8WklHh")]
        [InlineData("https://t.co/DdK08h4AZh")]
        [InlineData("https://t.co/ug1txCNmU6")] //returns shortened URI
        [InlineData("https://t.co/wkNNKx9jNT")]
        [InlineData("https://t.co/XyBCmZEAw9")]
        [InlineData("https://t.co/4UcW8F1GaI")] //returns null `response.Headers.Location`
        [InlineData("https://t.co/5vyJpSPebD")] //returns null `response.Headers.Location`
        [InlineData("https://t.co/bUngFnJVgv")] //returns null `response.Headers.Location`
        [InlineData("https://t.co/G52mArMTRy")] //returns null `response.Headers.Location`
        [InlineData("https://t.co/iFy5V2MR40")] //returns null `response.Headers.Location`
        [InlineData("https://t.co/Ps1CeMr7lh")] //returns null `response.Headers.Location`
        [InlineData("https://t.co/rxViWQCIdf")] //returns null `response.Headers.Location`
        [InlineData("https://t.co/uicVLDfohp")] //returns null `response.Headers.Location`
        [InlineData("https://t.co/vKonTficpv")] //returns null `response.Headers.Location`
        public async Task ToExpandedUriAsync_Test(string location)
        {
            this._testOutputHelper.WriteLine($"expanding `{location}`...");
            var uri = new Uri(location);

            var expandedUri = await uri.ToExpandedUriAsync();
            this._testOutputHelper.WriteLine($"expanded to `{expandedUri.OriginalString}`.");
        }

        readonly ITestOutputHelper _testOutputHelper;
    }
}
