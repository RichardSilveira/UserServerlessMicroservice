using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UserService.Domain.Events;

namespace UserService.EventHandlers.UserRegistered
{
    public class SendWelcomeEmailEventHandler : INotificationHandler<UserRegisteredDomainEvent>
    {
        public Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
            //Todo: 
        }
    }
}