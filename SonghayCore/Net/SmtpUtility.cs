#if NET452 || NET462

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace Songhay.Net
{
    /// <summary>
    /// SMTP Utility
    /// </summary>
    public static class SmtpUtility
    {
        static SmtpUtility()
        {
            smtpHost = ConfigurationManager.AppSettings.Get("SmtpHost");
            smtpUserToken = ConfigurationManager.AppSettings.Get("SmtpUserToken");
        }

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
        /// Sends the mail.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="message">The message.</param>
        public static void SendMail(string from, string to, string subject, string message)
        {
            SendMail(from, to, subject, message, attachments: null);
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="message">The message.</param>
        /// <param name="attachment">The attachment.</param>
        public static void SendMail(string from, string to, string subject, string message, string attachment)
        {
            var attachments = GetAttachment(new string[] { attachment });
            SendMail(from, to, subject, message, attachments);
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="message">The message.</param>
        /// <param name="attachments">The attachments.</param>
        public static void SendMail(string from, string to, string subject, string message, ICollection<Attachment> attachments)
        {
            SmtpClient client = new SmtpClient(smtpHost);

            var msg = new MailMessage();
            msg.From = new MailAddress(from);
            msg.To.Add(to);
            msg.Subject = subject;
            msg.SubjectEncoding = Encoding.UTF8;
            msg.Body = message;
            msg.BodyEncoding = Encoding.UTF8;

            if (attachments != null) foreach (var i in attachments) msg.Attachments.Add(i);

            client.SendAsync(msg, "Sending message");
        }

        static readonly string smtpHost;
        static readonly object smtpUserToken;
    }

}

#endif
