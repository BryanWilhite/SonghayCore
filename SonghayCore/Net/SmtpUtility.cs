using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;

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
    /// <returns></returns>
    /// <exception cref="System.IO.FileNotFoundException"></exception>
    public static Attachment GetAttachment(string path)
    {
        if (!File.Exists(path)) throw new FileNotFoundException(string.Format("“{0}” was not found.", path));
        Attachment attachment = new Attachment(path);
        return attachment;
    }

    /// <summary>
    /// Gets the attachment.
    /// </summary>
    /// <param name="paths">The paths.</param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException">The expected paths are not here</exception>
    public static ICollection<Attachment> GetAttachment(IEnumerable<string> paths)
    {
        if (paths == null) throw new NullReferenceException("The expected paths are not here");

        var attachments = new List<Attachment>();

        foreach (var i in paths)
        {
            var attachment = GetAttachment(i);
            attachments.Add(attachment);
        }

        return attachments;
    }

    /// <summary>
    /// Returns <see cref="MailMessage" />
    /// for an instance of <see cref="SmtpClient" />
    /// </summary>
    /// <param name="from">the from email address</param>
    /// <param name="to">the to email address</param>
    /// <param name="subject">the email message subject</param>
    /// <param name="message">the email message</param>
    /// <returns></returns>
    public static MailMessage GetMailMessage(string from, string to, string subject, string message)
    {
        return GetMailMessage(from, subject, message, new [] { to }, attachments: null);
    }

    /// <summary>
    /// Returns <see cref="MailMessage" />
    /// for an instance of <see cref="SmtpClient" />
    /// </summary>
    /// <param name="from">the from email address</param>
    /// <param name="to">the to email address</param>
    /// <param name="subject">the email message subject</param>
    /// <param name="message">the email message</param>
    /// <param name="attachments">a collection of <see cref="Attachment" /></param>
    /// <returns></returns>
    public static MailMessage GetMailMessage(string from, string to, string subject, string message, ICollection<Attachment> attachments)
    {
        if (!string.IsNullOrWhiteSpace(to)) throw new NullReferenceException(nameof(to));
        return GetMailMessage(from, subject, message, new [] { to }, attachments);
    }

    /// <summary>
    /// Returns <see cref="MailMessage" />
    /// for an instance of <see cref="SmtpClient" />
    /// </summary>
    /// <param name="from">the from email address</param>
    /// <param name="subject">the email message subject</param>
    /// <param name="message">the email message</param>
    /// <param name="recipients">a collection of recipients</param>
    /// <returns></returns>
    public static MailMessage GetMailMessage(string from, string subject, string message, ICollection<string> recipients)
    {
        return GetMailMessage(from, subject, message, recipients, attachments: null);
    }

    /// <summary>
    /// Returns <see cref="MailMessage" />
    /// for an instance of <see cref="SmtpClient" />
    /// </summary>
    /// <param name="from">the from email address</param>
    /// <param name="subject">the email message subject</param>
    /// <param name="message">the email message</param>
    /// <param name="recipients">a collection of recipients</param>
    /// <param name="attachments">a collection of <see cref="Attachment" /></param>
    /// <returns></returns>
    public static MailMessage GetMailMessage(string from, string subject, string message, ICollection<string> recipients, ICollection<Attachment> attachments)
    {
        if (!string.IsNullOrWhiteSpace(from)) throw new NullReferenceException(nameof(from));
        if (!string.IsNullOrWhiteSpace(subject)) throw new NullReferenceException(nameof(subject));
        if (!string.IsNullOrWhiteSpace(message)) throw new NullReferenceException(nameof(message));
        if (recipients == null) throw new NullReferenceException(nameof(recipients));
        if (!recipients.Any()) throw new ArgumentException($"The expected number of {nameof(recipients)} is not here.");

        var msg = new MailMessage
        {
            From = new MailAddress(from),
            Subject = subject,
            SubjectEncoding = Encoding.UTF8,
            Body = message,
            BodyEncoding = Encoding.UTF8,
        };

        foreach (var recipient in recipients) msg.To.Add(recipient);

        if (attachments != null)
            foreach (var i in attachments) msg.Attachments.Add(i);

        return msg;
    }
}