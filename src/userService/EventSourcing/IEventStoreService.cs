using EventStore.ClientAPI;

/* Can be share across all your Bounded Contexts
 It's good have it separated of your app because you may or may not work with Event Sourcing
Plus, if you work with Event Sourcing is a good choice that all your microservices share the 
 underlying infrastructure for store the events. */
namespace UserService.EventSourcing
{
    public interface IEventStoreService
    {
        IEventStoreConnection GetConnection();
    }

    public class EventStoreService : IEventStoreService
    {
        private readonly IEventStoreConnection _connection; //todo: Update to Singleton with Lazy

        public EventStoreService()
        {
            _connection = EventStoreConnection.Create("");

            _connection.ConnectAsync();
        }

        public IEventStoreConnection GetConnection() => _connection;//todo: Move to appsettings - var connection = EventStoreConnection.Create(settings, new Uri("tcp://admin:changeit@127.0.0.1:1113"));
    }
}