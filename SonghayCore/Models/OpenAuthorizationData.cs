namespace Songhay.Models;

/// <summary>
/// Defines Authorization Information for OAuth 1.0.
/// </summary>
public class OpenAuthorizationData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenAuthorizationData"/> class.
    /// </summary>
    public OpenAuthorizationData()
        : this(null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenAuthorizationData"/> class.
    /// </summary>
    /// <param name="data">The data.</param>
    public OpenAuthorizationData(NameValueCollection data)
        : this(null, data)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenAuthorizationData" /> class.
    /// </summary>
    /// <param name="nonce">The nonce.</param>
    /// <param name="data">The data.</param>
    public OpenAuthorizationData(string? nonce, NameValueCollection? data)
    {
        Nonce = string.IsNullOrWhiteSpace(nonce)
            ? Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()))
            : nonce;
        SignatureMethod = "HMAC-SHA1";
        var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        TimeStamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();
        Version = "1.0";

        if (data == null) return;
        ConsumerKey = data["TwitterConsumerKey"];
        ConsumerSecret = data["TwitterConsumerSecret"];
        Token = data["TwitterToken"];
        TokenSecret = data["TwitterTokenSecret"];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenAuthorizationData" /> class.
    /// </summary>
    /// <param name="nonce">The nonce.</param>
    public OpenAuthorizationData(string nonce) : this()
    {
        Nonce = nonce;
    }

    /// <summary>
    /// Gets or sets the consumer key.
    /// </summary>
    /// <value>
    /// The consumer key.
    /// </value>
    public string? ConsumerKey { get; set; }

    /// <summary>
    /// Gets or sets the consumer secret.
    /// </summary>
    /// <value>
    /// The consumer secret.
    /// </value>
    public string? ConsumerSecret { get; set; }

    /// <summary>
    /// Gets the nonce.
    /// </summary>
    /// <value>
    /// The nonce.
    /// </value>
    public string? Nonce { get; set; }

    /// <summary>
    /// Gets or sets the time stamp.
    /// </summary>
    /// <value>
    /// The time stamp.
    /// </value>
    public string? TimeStamp { get; set; }

    /// <summary>
    /// Gets or sets the token.
    /// </summary>
    /// <value>
    /// The token.
    /// </value>
    public string? Token { get; set; }

    /// <summary>
    /// Gets or sets the token secret.
    /// </summary>
    /// <value>
    /// The token secret.
    /// </value>
    public string? TokenSecret { get; set; }

    /// <summary>
    /// Gets the signature method.
    /// </summary>
    /// <value>
    /// The signature method.
    /// </value>
    public string? SignatureMethod { get; set; }

    /// <summary>
    /// Gets or sets the version.
    /// </summary>
    /// <value>
    /// The version.
    /// </value>
    public string? Version { get; set; }
}
