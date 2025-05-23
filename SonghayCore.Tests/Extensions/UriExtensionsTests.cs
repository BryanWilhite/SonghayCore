﻿namespace Songhay.Tests.Extensions;

public class UriExtensionsTests(ITestOutputHelper helper)
{
    [Trait(TestScalars.XunitCategory, TestScalars.XunitCategoryIntegrationManualTest)]
    [SkippableTheory]
    [InlineData("http://ow.ly/i/8Tq32")] //does not expand
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
        Skip.If(TestScalars.IsNotDebugging, TestScalars.ReasonForSkippingWhenNotDebugging);

        helper.WriteLine($"expanding `{location}`...");
        var uri = new Uri(location);

        var expandedUri = await uri.ToExpandedUriAsync();
        helper.WriteLine($"expanded to `{expandedUri.ToReferenceTypeValueOrThrow().OriginalString}`.");
    }
}