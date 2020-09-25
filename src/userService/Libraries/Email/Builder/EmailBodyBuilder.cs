using MimeKit;
using MimeKit.Text;

namespace UserService.Libraries.Email.Builder
{
    public class EmailBodyBuilder : EmailMessageBuilder
    {
        public EmailBodyBuilder(MimeMessage message) => this.message = message;

        public EmailBodyBuilder AsSimpleText(string content)
        {
            message.Body = new TextPart(TextFormat.Text) {Text = content};
            return this;
        }

        public EmailBodyBuilder AsHtml(string htmlFormattedContent)
        {
            message.Body = new TextPart(TextFormat.Html) {Text = htmlFormattedContent};
            return this;
        }
    }
}