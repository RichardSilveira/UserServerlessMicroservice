using System.Net.Mail;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace UserService.Libraries.Email
{
    public class EmailSender
    {
        private readonly SmtpSettings _settings;

        public EmailSender(SmtpSettings settings)
        {
            _settings = settings;
        }

        public void Send(MimeMessage message)
        {
            var (host, port, userName, password, useSsl) = _settings;

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(host, port, useSsl);
                client.Authenticate(userName, password);

                client.Send(message);
            }
        }

        public async Task SendAsync(MimeMessage message)
        {
            var (host, port, userName, password, useSsl) = _settings;

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(host, port, useSsl);
                client.Authenticate(userName, password);

                await client.SendAsync(message);
            }
        }
    }
}