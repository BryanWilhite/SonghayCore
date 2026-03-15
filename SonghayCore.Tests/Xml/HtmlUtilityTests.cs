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
    [InlineData("<meta>", "<meta />")]
    [InlineData("<meta >", "<meta />")]
    [InlineData("<meta />", "<meta />")]
    [InlineData("<link rel=\"stylesheet\" type=\"text/css\" href=\"style.css?a=42&b=true\">",
        "<link rel=\"stylesheet\" type=\"text/css\" href=\"style.css?a=42&amp;b=true\" />")]
    [InlineData("<br>", "<br />")]
    [InlineData("<br >", "<br />")]
    [InlineData("<br />", "<br />")]
    [InlineData("<hr>", "<hr />")]
    [InlineData("<hr class\"foo\">", "<hr class\"foo\" />")]
    [InlineData("<hr   />", "<hr />")]
    [InlineData("<img src=\"https://myserver.org/images-engine/foo.jpg?m=42&t=y\">",
        "<img src=\"https://myserver.org/images-engine/foo.jpg?m=42&amp;t=y\" />")]
    [InlineData("<option selected>", "<option selected=\"selected\">")]
    [InlineData("<option value=\"dog\" selected>Dog</option>", "<option value=\"dog\" selected=\"selected\">Dog</option>")]
    [InlineData("<option value=\"dog\" selected disabled>Dog</option>", "<option value=\"dog\" selected=\"selected\" disabled=\"disabled\">Dog</option>")]
    [InlineData("<option value=\"dog\" selected badattr disabled>Dog</option>", "<option value=\"dog\" selected=\"selected\" badattr disabled=\"disabled\">Dog</option>")]

    [InlineData(
        """
        <html xmlns="http://www.w3.org/1999/xhtml">
            <head>
                <meta charset='utf-8'>
                <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                <meta name='viewport' content='width=device-width, initial-scale=1'>
                <link rel='stylesheet' type='text/css' media='screen' href='_bundles/styles.min.css'>
                <link rel="SHORTCUT ICON" href="images/favicon.ico">

                <script src='_bundles/scripts.min.js?m=42&t=y'></script>

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
        """,
        """
        <html>
            <head>
                <meta charset='utf-8' />
                <meta http-equiv='X-UA-Compatible' content='IE=edge' />
                <meta name='viewport' content='width=device-width, initial-scale=1' />
                <link rel='stylesheet' type='text/css' media='screen' href='_bundles/styles.min.css' />
                <link rel="SHORTCUT ICON" href="images/favicon.ico" />

                <script src='_bundles/scripts.min.js?m=42&amp;t=y'></script>

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
        """)]
    public void ConvertToXml_Test(string input, string expected)
    {
        string? actual = HtmlUtility.ConvertToXml(input);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("<a />", "<a></a>")]
    [InlineData("<a id=\"one\" />", "<a id=\"one\"></a>")]
    [InlineData("<iframe />", "<iframe></iframe>")]
    [InlineData("<iframe id=\"one\" />", "<iframe id=\"one\"></iframe>")]
    [InlineData("<td />", "<td></td>")]
    [InlineData("<td id=\"one\" />", "<td id=\"one\"></td>")]
    [InlineData("<th />", "<th></th>")]
    [InlineData("<th id=\"one\" />", "<th id=\"one\"></th>")]
    [InlineData("<script />", "<script></script>")]
    [InlineData("<script id=\"one\" />", "<script id=\"one\"></script>")]
    public void FormatXhtmlElements_Test(string input, string expected)
    {
        string? actual = HtmlUtility.FormatXhtmlElements(input);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("<a href=\"#\">link</a><span>text</span>", "a", "\r\n", 4, "link")]
    [InlineData("<a href=\"#\">link</a><span>text</span>", "span", "\r\n", 4, "text")]
    [InlineData(
        """
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
        """,
        "body", "\n", 4,
        """

        <select>
            <option selected>Dog</option>
            <option value="cat">Cat</option>
        </select>
        <select>
            <option value="dog" selected>Dog</option>
            <option value="cat">Cat</option>
        </select>

        """
        )]
    public void GetInnerXml_Test(string input, string? elementName, string newLine, byte numberOfChars, string expected)
    {
        string? actual = HtmlUtility.GetInnerXml(input, elementName, newLine, numberOfChars);

        Assert.Equal(expected, actual);
    }
}
