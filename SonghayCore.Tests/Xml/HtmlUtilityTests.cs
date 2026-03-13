using Songhay.Xml;

namespace Songhay.Tests.Xml;

public class HtmlUtilityTests
{
    [Theory]
    [InlineData("<base></base>", "<base>")]
    [InlineData("<base target=\"_top\" href=\"https://example.com/\"></base>",
        "<base target=\"_top\" href=\"https://example.com/\">")]
    [InlineData("<isindex></isindex>", "<isindex>")]
    [InlineData("<isindex prompt=\"highlight me\"></isindex>", "<isindex prompt=\"highlight me\">")]
    [InlineData("<link rel=\"stylesheet\" type=\"text/css\" href=\"style.css\"></link>", "<link rel=\"stylesheet\" type=\"text/css\" href=\"style.css\">")]
    [InlineData("<meta></meta>", "<meta>")]
    [InlineData("<meta http-equiv='X-UA-Compatible' content='IE=edge'></meta>", "<meta http-equiv='X-UA-Compatible' content='IE=edge'>")]

    [InlineData("<html lang=\"en\">", "<html>")]

    [InlineData("<meta />", "<meta>")]
    [InlineData("<meta http-equiv='X-UA-Compatible' content='IE=edge' />", "<meta http-equiv='X-UA-Compatible' content='IE=edge'>")]

    [InlineData("<option selected=\"selected\">", "<option selected>")]
    [InlineData("<option value=\"dog\" selected=\"selected\">Dog</option>", "<option value=\"dog\" selected>Dog</option>")]
    public void ConvertToHtml_Test(string input, string expected)
    {
        string? actual = HtmlUtility.ConvertToHtml(input);

        Assert.Equal(expected, actual);
    }
}