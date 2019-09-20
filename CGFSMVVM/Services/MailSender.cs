using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CGFSMVVM.Helpers;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace CGFSMVVM.Services
{
    /// <summary>
    /// Mail sender.
    /// </summary>
    public static class MailSender
    {
        /// <summary>
        /// Sendsmtps the mail.
        /// </summary>
        /// <returns>The mail.</returns>
        /// <param name="texter">Texter.</param>
        /// <param name="header">Header.</param>
        /// <param name="image">Image.</param>
        /// <param name="hotelCode">Hotel code.</param>
        /// <param name="resNo">Res no.</param>
        public static async Task<bool> SendsmtpMail(string texter, string header,byte[] image,string hotelCode, string resNo)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Cinnamon Guest Feedback App", "administrator@cinnamonhotels.com"));
                message.To.Add(new MailboxAddress("Thimira", "thimira@cinnamonhotels.com"));
                message.Subject = "CGFS APP";

                //message.Body = new TextPart(TextFormat.Html)
                //{
                //    Text = "<html>" +
                //            "<h1>Cinnamon Guest Feedback App</h1>" +
                //            "<h2>" + header + "</h2>" +
                //            "<p>" + texter + "</p>" +
                //            "</html>"
                //};

                // create our message text, just like before (except don't set it as the message.Body)
                var body = new TextPart(TextFormat.Html)
                {
                    Text = "<html>" +
                            "<h1>Cinnamon Guest Feedback App</h1>" +
                            "<h2>" + header + "</h2>" +
                            "<p>" + texter + "</p>" +
                            "</html>"
                };

                MemoryStream memoryStream = new MemoryStream(image);

                // create an image attachment for the file located at path
                var attachment = new MimePart("image", "jpg")
                {
                    Content = new MimeContent(memoryStream, ContentEncoding.Default),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = $"{hotelCode}_{resNo}.jpg"
                };

                // now create the multipart/mixed container to hold the message text and the
                // image attachment
                var multipart = new Multipart("mixed");
                multipart.Add(body);
                multipart.Add(attachment);

                // now set the multipart/mixed as the message body
                message.Body = multipart;


                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync("smtp.office365.com", 587, false);

                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync("cin_amd@jkintranet.com", Settings.SMTPPassword);

                    await client.SendAsync(message);

                    Console.WriteLine("E mail Sent");

                    return true;

                }
            }
            catch (Exception)
            {

                return false;
            }
        }

    }
}
