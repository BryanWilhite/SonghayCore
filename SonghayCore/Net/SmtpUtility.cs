using System.Net.Mail;

namespace Songhay.Net;

/// <summary>
/// SMTP Utility
/// </summary>
public static class SmtpUtility
{
    /// <summary>
    /// Gets the attachment.
    /// </summary>
    /// <param name="path">The path.</param>
    public static Attachment GetAttachment(string? path)
    {
        if (!File.Exists(path)) throw new FileNotFoundException($"“{path}” was not found.");

        var attachment = new Attachment(path);
        return attachment;
    }

    /// <summary>
    /// Gets the attachment.
    /// </summary>
    /// <param name="paths">The paths.</param>
    public static ICollection<Attachment> GetAttachment(IEnumerable<string>? paths) =>
        paths == null
            ? Enumerable.Empty<Attachment>().ToList()
            : paths.Select(GetAttachment).ToList();

    /// <summary>
    /// Returns <see cref="MailMessage" />
    /// for an instance of <see cref="SmtpClient" />
    /// </summary>
    /// <param name="from">the from email address</param>
    /// <param name="to">the to email address</param>
    /// <param name="subject">the email message subject</param>
    /// <param name="message">the email message</param>
    public static MailMessage GetMailMessage(string from, string to, string subject, string message) =>
        GetMailMessage(from, subject, message, new[] {to}, attachments: null);

    /// <summary>
    /// Returns <see cref="MailMessage" />
    /// for an instance of <see cref="SmtpClient" />
    /// </summary>
    /// <param name="from">the from email address</param>
    /// <param name="to">the to email address</param>
    /// <param name="subject">the email message subject</param>
    /// <param name="message">the email message</param>
    /// <param name="attachments">a collection of <see cref="Attachment" /></param>
    public static MailMessage GetMailMessage(string from, string to, string subject, string message,
        ICollection<Attachment> attachments) =>
        !string.IsNullOrWhiteSpace(to)
            ? throw new NullReferenceException(nameof(to))
            : GetMailMessage(from, subject, message, new[] {to}, attachments);

    /// <summary>
    /// Returns <see cref="MailMessage" />
    /// for an instance of <see cref="SmtpClient" />
    /// </summary>
    /// <param name="from">the from email address</param>
    /// <param name="subject">the email message subject</param>
    /// <param name="message">the email message</param>
    /// <param name="recipients">a collection of recipients</param>
    public static MailMessage GetMailMessage(string from, string subject, string message,
        ICollection<string> recipients) => GetMailMessage(from, subject, message, recipients, attachments: null);

    /// <summary>
    /// Returns <see cref="MailMessage" />
    /// for an instance of <see cref="SmtpClient" />
    /// </summary>
    /// <param name="from">the from email address</param>
    /// <param name="subject">the email message subject</param>
    /// <param name="message">the email message</param>
    /// <param name="recipients">a collection of recipients</param>
    /// <param name="attachments">a collection of <see cref="Attachment" /></param>
    public static MailMessage GetMailMessage(string? from, string? subject, string? message,
        ICollection<string>? recipients, ICollection<Attachment>? attachments)
    {
        from.ThrowWhenNullOrWhiteSpace();
        subject.ThrowWhenNullOrWhiteSpace();
        message.ThrowWhenNullOrWhiteSpace();
        recipients.ThrowWhenNullOrEmpty();

        var msg = new MailMessage
        {
            From = new MailAddress(from),
            Subject = subject,
            SubjectEncoding = Encoding.UTF8,
            Body = message,
            BodyEncoding = Encoding.UTF8,
        };

        foreach (var recipient in recipients) msg.To.Add(recipient);

        if (attachments == null) return msg;

        foreach (var i in attachments) msg.Attachments.Add(i);

        return msg;
    }
}
