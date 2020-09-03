using LibraryAPI.IServices;
using LibraryAPI.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;

namespace LibraryAPI.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly SMTPSettings _smtpsettings;

        public EmailServices(IOptions<SMTPSettings> smtpsettings)
        {
            _smtpsettings = smtpsettings.Value;
        }
        public async Task Send(string to, string subject, string html, string from = null)
        {
            // create message
            MimeMessage email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(from ?? _smtpsettings.EmailFrom);
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_smtpsettings.SmtpHost, _smtpsettings.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_smtpsettings.SmtpUser, _smtpsettings.SmtpPass);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
