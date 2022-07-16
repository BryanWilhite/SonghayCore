namespace Songhay.Models;

/// <summary>
/// Selected MIME types for this Studio
/// </summary>
/// <remarks>
/// Multipurpose Internet Mail Extensions: https://tools.ietf.org/html/rfc6838
/// See: https://www.iana.org/assignments/media-types/media-types.xhtml
/// </remarks>
public static class MimeTypes
{
    /// <summary>
    /// MIME media type name: application
    /// MIME subtype name: atom+xml
    /// [ https://www.iana.org/assignments/media-types/application/atom+xml ]
    /// </summary>
    public const string ApplicationAtomXml = "application/atom+xml";

    /// <summary>
    /// MIME media type name: application
    /// Media subtype name: x-www-form-urlencoded
    /// [ https://www.iana.org/assignments/media-types/application/x-www-form-urlencoded ]
    /// </summary>
    public const string ApplicationFormUrlEncoded = "application/x-www-form-urlencoded";

    /// <summary>
    /// MIME media type name: application
    /// Subtype name: json
    /// [ https://www.iana.org/assignments/media-types/application/json ]
    /// </summary>
    public const string ApplicationJson = "application/json";

    /// <summary>
    /// MIME media type name: application
    /// The <c>octet-stream</c> subtype is used to indicate that a body contains arbitrary binary data.
    /// [ https://www.iana.org/assignments/media-types/application/octet-stream ]
    /// </summary>
    public const string ApplicationOctetStream = "application/octet-stream";

    /// <summary>
    /// MIME media type name: application
    /// </summary>
    /// <remarks>
    /// This MIME type is not documented by www.iana.org.
    /// What did Dave Winer do to those people?
    /// </remarks>
    public const string ApplicationRssXml = "application/rss+xml";

    /// <summary>
    /// MIME media type name: application
    /// MIME subtype name: vnd.ms-fontobject
    /// [ https://www.iana.org/assignments/media-types/application/vnd.ms-fontobject ]
    /// </summary>
    public const string ApplicationVndMsFontObject = "application/vnd.ms-fontobject";

    /// <summary>
    /// MIME media type name: application
    /// </summary>
    /// <remarks>
    /// This MIME type is not documented by www.iana.org.
    /// [ https://www.iana.org/assignments/media-types/font/otf ]
    /// </remarks>
    public const string ApplicationXFontOtf = "application/x-font-otf";

    /// <summary>
    /// MIME media type name: application
    /// </summary>
    /// <remarks>
    /// This MIME type is not documented by www.iana.org.
    /// [ https://www.iana.org/assignments/media-types/font/ttf ]
    /// </remarks>
    public const string ApplicationXFontTtf = "application/x-font-ttf";

    /// <summary>
    /// MIME media type name: application
    /// </summary>
    /// <remarks>
    /// This MIME type is not documented by www.iana.org.
    /// [ https://www.iana.org/assignments/media-types/font/woff ]
    /// </remarks>
    public const string ApplicationXFontWoff = "application/x-font-woff";

    /// <summary>
    /// MIME media type name: application
    /// Subtype name:  xml
    /// [ https://www.iana.org/assignments/media-types/application/xml ]
    /// </summary>
    public const string ApplicationXml = "application/xml";

    /// <summary>
    /// The image GIF format.
    /// </summary>
    /// <remarks>
    /// This MIME type is not documented by www.iana.org.
    ///
    /// [ https://www.iana.org/go/rfc2046 ]
    /// [ https://developer.mozilla.org/en-US/docs/Web/Media/Formats/Image_types#gif_graphics_interchange_format ]
    /// </remarks>
    public const string ImageGif = "image/gif";

    /// <summary>
    /// The image JPEG format.
    /// </summary>
    /// <remarks>
    /// This MIME type is not documented by www.iana.org.
    ///
    /// [ https://www.iana.org/go/rfc2046 ]
    /// [ https://developer.mozilla.org/en-US/docs/Web/Media/Formats/Image_types#jpeg_joint_photographic_experts_group_image ]
    /// </remarks>
    public const string ImageJpeg = "image/jpeg";

    /// <summary>
    /// Media type name: image
    /// Media subtype name: png
    /// [ https://www.iana.org/assignments/media-types/image/png ]
    /// [ https://developer.mozilla.org/en-US/docs/Web/Media/Formats/Image_types#png_portable_network_graphics ]
    /// </summary>
    public const string ImagePng = "image/png";

    /// <summary>
    /// Media type name: image
    /// Subtype name: svg+xml
    /// [ https://www.iana.org/assignments/media-types/image/svg+xml ]
    /// [ https://developer.mozilla.org/en-US/docs/Web/Media/Formats/Image_types#svg_scalable_vector_graphics ]
    /// </summary>
    public const string ImageSvgXml = "image/svg+xml";

    /// <summary>
    /// The Web Picture format.
    /// </summary>
    /// <remarks>
    /// This MIME type is not documented by www.iana.org.
    ///
    /// [ https://developer.mozilla.org/en-US/docs/Web/Media/Formats/Image_types#webp_image ]
    /// </remarks>
    public const string ImageWebP = "image/webp";

    /// <summary>
    /// Microsoft Icon.
    /// </summary>
    /// <remarks>
    /// This MIME type is not documented by www.iana.org.
    ///
    /// [ https://developer.mozilla.org/en-US/docs/Web/Media/Formats/Image_types#ico_microsoft_windows_icon ]
    /// </remarks>
    public const string ImageXIcon = "image/x-icon";

    /// <summary>
    /// Type name: text
    /// Subtype name: css
    /// [ https://www.iana.org/assignments/media-types/text/css ]
    /// </summary>
    public const string TextCss = "text/css";

    /// <summary>
    /// Type name: text
    /// Subtype name: Standards Tree - html
    /// [ https://www.iana.org/assignments/media-types/text/html ]
    /// </summary>
    public const string TextHtml = "text/html";

    /// <summary>
    /// Type name: text
    /// Subtype name: javascript
    /// [ https://www.iana.org/assignments/media-types/text/javascript ]
    /// </summary>
    public const string TextJavascript = "text/javascript";

    /// <summary>
    /// Text, (generally ASCII or ISO 8859-n).
    /// </summary>
    /// <remarks>
    /// This MIME type is not documented by www.iana.org.
    ///
    /// [ https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types ]
    /// </remarks>
    public const string TextPlain = "text/plain";

    /// <summary>
    /// Type name: text
    /// Subtype name: xml
    /// [ https://www.iana.org/assignments/media-types/text/xml ]
    /// </summary>
    public const string TextXml = "text/xml";
}
