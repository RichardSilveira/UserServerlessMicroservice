using MimeKit;

namespace UserService.Libraries.Email.Builder
{
    public class EmailConfigurationBuilder : EmailMessageBuilder
    {
        public EmailConfigurationBuilder(MimeMessage message) => this.message = message;

        public EmailConfigurationBuilder WithSubject(string subject)
        {
            message.Subject = subject;
            return this;
        }

        public EmailConfigurationBuilder AsUrgent()
        {
            message.Priority = MessagePriority.Urgent;
            return this;
        }

        public EmailConfigurationBuilder From(MailboxAddress source)
        {
            message.From.Add(source);
            return this;
        }

        public EmailConfigurationBuilder From(string name, string address)
        {
            message.From.Add(new MailboxAddress(name, address));
            return this;
        }

        public EmailConfigurationBuilder To(params string[] addresses)
        {
            foreach (var address in addresses)
            {
                message.To.Add(new MailboxAddress("", address));
            }

            return this;
        }
    }
}