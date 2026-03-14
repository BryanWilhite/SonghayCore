using Songhay.Xml;

namespace Songhay.Tests.Xml;

public class HtmlUtilityTests
{
    [Theory]
    [InlineData("<base></base>", "<base>")]
    [InlineData("<base>  </base>", "<base>")]
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

    [InlineData(
        """
        <html lang="en">
            <head>
                <meta charset='utf-8' />
                <meta http-equiv='X-UA-Compatible' content='IE=edge' />
                <meta name='viewport' content='width=device-width, initial-scale=1'></meta>
                <link rel='stylesheet' type='text/css' media='screen' href='_bundles/styles.min.css'></link>
                <link rel="SHORTCUT ICON" href="images/favicon.ico" />

                <script src='_bundles/scripts.min.js'></script>

                <title>Demo</title>
            </head>

            <body>
                <select>
                    <option selected="selected">Dog</option>
                    <option value="cat">Cat</option>
                </select>
                <select>
                    <option value="dog" selected="selected">Dog</option>
                    <option value="cat">Cat</option>
                </select>
            </body>
        </html>
        """,
    """
        <html>
            <head>
                <meta charset='utf-8'>
                <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                <meta name='viewport' content='width=device-width, initial-scale=1'>
                <link rel='stylesheet' type='text/css' media='screen' href='_bundles/styles.min.css'>
                <link rel="SHORTCUT ICON" href="images/favicon.ico">

                <script src='_bundles/scripts.min.js'></script>

                <title>Demo</title>
            </head>

            <body>
                <select>
                    <option selected>Dog</option>
                    <option value="cat">Cat</option>
                </select>
                <select>
                    <option value="dog" selected>Dog</option>
                    <option value="cat">Cat</option>
                </select>
            </body>
        </html>
        """)]
    public void ConvertToHtml_Test(string input, string expected)
    {
        string? actual = HtmlUtility.ConvertToHtml(input);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("<html xmlns=\"http://www.w3.org/1999/xhtml\">", "<html>")]
    [InlineData("<html  xmlns=\"http://www.w3.org/1999/xhtml\"  >", "<html>")]
    [InlineData("<option selected>", "<option selected=\"selected\">")]
    [InlineData("<br>", "<br />")]
    [InlineData("<br >", "<br />")]
    [InlineData("<br />", "<br />")]
    [InlineData("<hr>", "<hr />")]
    [InlineData("<hr class\"foo\">", "<hr class\"foo\" />")]
    [InlineData("<hr   />", "<hr />")]
    [InlineData("<img src=\"https://myserver.org/images-engine/foo.jpg?m=42&t=y\">",
        "<img src=\"https://myserver.org/images-engine/foo.jpg?m=42&amp;t=y\" />")]
    [InlineData("<link rel=\"stylesheet\" type=\"text/css\" href=\"style.css?a=42&b=true\">",
        "<link rel=\"stylesheet\" type=\"text/css\" href=\"style.css?a=42&amp;b=true\" />")]
    [InlineData("<meta>", "<meta />")]
    [InlineData("<meta >", "<meta />")]
    [InlineData("<meta />", "<meta />")]
    public void ConvertToXml_Test(string input, string expected)
    {
        string? actual = HtmlUtility.ConvertToXml(input);

        Assert.Equal(expected, actual);
    }
}