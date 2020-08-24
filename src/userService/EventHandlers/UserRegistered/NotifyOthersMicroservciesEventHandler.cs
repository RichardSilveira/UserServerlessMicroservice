using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UserService.Domain.Events;

namespace UserService.EventHandlers.UserRegistered
{
    public class NotifyOthersMicroservciesEventHandler : INotificationHandler<UserRegisteredDomainEvent>
    {
        public Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
        {
            //todo: Notify via AWS EventBridge
            throw new System.NotImplementedException();
        }
    }
}