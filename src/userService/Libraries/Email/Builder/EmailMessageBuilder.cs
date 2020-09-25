using MimeKit;

namespace UserService.Libraries.Email.Builder
{
    public class EmailMessageBuilder
    {
        protected MimeMessage message = new MimeMessage();

        public EmailConfigurationBuilder Configure => new EmailConfigurationBuilder(message);
        public EmailBodyBuilder WithBody => new EmailBodyBuilder(message);

        public MimeMessage Build() => message;
    }
}