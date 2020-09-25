using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UserService.Domain.Events;
using UserService.Libraries.Email;
using UserService.Libraries.Email.Builder;

namespace UserService.EventHandlers.UserRegistered
{
    public class SendWelcomeEmailEventHandler : INotificationHandler<UserRegisteredDomainEvent>
    {
        private readonly EmailSender _emailSender;

        public SendWelcomeEmailEventHandler(EmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
        {
            var email = new EmailMessageBuilder()
                .Configure
                .From("Richard from CompanyX", "richardleecba@gmail.com")
                .WithSubject("Welcome to CompanyX 👋")
                .To("richardleecba@gmail.com")
                .WithBody
                .AsHtml(EmailContent())
                .Build();

            _emailSender.SendAsync(email);

            await Task.CompletedTask; // There is no need to wait the email be delivered sometimes...
        }

        private string EmailContent() => "<h1>The content goes here</h1>";
    }
}