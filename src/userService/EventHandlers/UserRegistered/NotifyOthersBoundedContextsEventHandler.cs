using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UserService.Domain.Events;

namespace UserService.EventHandlers.UserRegistered
{
    public class NotifyOthersBoundedContextsEventHandler : INotificationHandler<UserRegisteredDomainEvent>
    {
        public Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
        {
            //todo: Notify via AWS SQS (maybe SNS + SQS through fanout strategy)
            throw new System.NotImplementedException();
        }
    }
}